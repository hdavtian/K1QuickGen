using K1QuickGen.Api.Interfaces;
using K1QuickGen.Api.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace K1QuickGen.Api.Services
{
    /// <summary>
    /// Service for managing RabbitMQ messaging infrastructure.
    /// Handles connection setup, publishing messages to a queue, and consuming messages from a queue.
    /// This service abstracts the low-level RabbitMQ client operations and provides a clean interface
    /// for other services to send and receive messages asynchronously.
    /// 
    /// Typical usage includes publishing domain events or commands for background processing,
    /// and consuming messages for event-driven workflows or integration with other systems.
    /// </summary>
    public class RabbitMqService : IRabbitMqService, IDisposable
    {
        private readonly IConnection? _connection;
        private readonly IModel? _channel;

        private readonly RabbitMqSettings _settings;
        private readonly ILogger<RabbitMqService> _logger;
        private bool _disposed;

        /// <summary>
        /// Constructor. Initializes a new instance of the <see cref="RabbitMqService"/> class using Dependency Injection (DI).
        /// Sets up the RabbitMQ connection and declares the target queue.
        /// </summary>
        /// <param name="options">The RabbitMQ settings injected via DI.</param>
        /// <param name="logger">The logger instance for logging RabbitMQ events, injected via DI.</param>
        public RabbitMqService(IOptions<RabbitMqSettings> options, ILogger<RabbitMqService> logger)
        {
            _settings = options.Value;
            _logger = logger;

            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = _settings.HostName,
                    UserName = _settings.UserName,
                    Password = _settings.Password
                };

                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();

                _channel.QueueDeclare(
                    queue: _settings.QueueName,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                _logger.LogInformation("RabbitMQ connection established successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to initialize RabbitMQ connection");
                throw;
            }
        }

        /// <summary>
        /// Publishes a message to the configured RabbitMQ queue.
        /// Serializes the message to JSON and marks it as persistent for reliability.
        /// </summary>
        /// <typeparam name="T">The type of the message to publish.</typeparam>
        /// <param name="message">The message object to send.</param>
        public void Publish<T>(T message) where T : class
        {
            if (_channel == null || _connection == null || !_connection.IsOpen)
            {
                _logger.LogError("Cannot publish message: RabbitMQ connection is not open");
                return;
            }

            try
            {
                string json = System.Text.Json.JsonSerializer.Serialize(message);
                var body = Encoding.UTF8.GetBytes(json);

                var properties = _channel.CreateBasicProperties();
                properties.Persistent = true; // Make message persistent

                _channel.BasicPublish(
                    exchange: "",
                    routingKey: _settings.QueueName,
                    basicProperties: properties,
                    body: body);

                _logger.LogInformation($"Published message to queue '{_settings.QueueName}'");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to publish message to RabbitMQ");
                throw;
            }
        }

        /// <summary>
        /// Starts consuming messages from the configured RabbitMQ queue.
        /// Logs each received message and acknowledges processing.
        /// </summary>
        public void StartConsuming()
        {
            if (_channel == null || _connection == null || !_connection.IsOpen)
            {
                _logger.LogError("Cannot start consuming: RabbitMQ connection is not open");
                return;
            }

            try
            {
                _channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

                var consumer = new EventingBasicConsumer(_channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    try
                    {
                        // Process the message here
                        _logger.LogInformation($"Received message: {message}");

                        // Acknowledge message processing completion
                        _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error processing RabbitMQ message");
                        // Nack the message for redelivery
                        _channel.BasicNack(deliveryTag: ea.DeliveryTag, multiple: false, requeue: true);
                    }
                };

                // autoAck is set to false for manual acknowledgment
                _channel.BasicConsume(
                    queue: _settings.QueueName,
                    autoAck: false,
                    consumer: consumer);

                _logger.LogInformation($"Started consuming messages from queue '{_settings.QueueName}'");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to start consuming messages from RabbitMQ");
                throw;
            }
        }

        /// <summary>
        /// Starts consuming messages from the configured RabbitMQ queue using a custom message processor.
        /// The provided delegate is invoked for each received message.
        /// </summary>
        /// <param name="messageProcessor">A delegate to process each received message as a string.</param>
        public void StartConsuming(Action<string> messageProcessor)
        {
            if (_channel == null || _connection == null || !_connection.IsOpen)
            {
                _logger.LogError("Cannot start consuming: RabbitMQ connection is not open");
                return;
            }

            try
            {
                _channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

                var consumer = new EventingBasicConsumer(_channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    try
                    {
                        // Use the provided message processor
                        messageProcessor(message);

                        // Acknowledge message processing completion
                        _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error processing RabbitMQ message");
                        // Nack the message for redelivery
                        _channel.BasicNack(deliveryTag: ea.DeliveryTag, multiple: false, requeue: true);
                    }
                };

                // autoAck is set to false for manual acknowledgment
                _channel.BasicConsume(
                    queue: _settings.QueueName,
                    autoAck: false,
                    consumer: consumer);

                _logger.LogInformation($"Started consuming messages from queue '{_settings.QueueName}' with custom processor");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to start consuming messages from RabbitMQ");
                throw;
            }
        }

        /// <summary>
        /// Disposes the RabbitMQ connection and channel resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Protected dispose pattern implementation for releasing managed resources.
        /// </summary>
        /// <param name="disposing">Indicates whether managed resources should be disposed.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                _channel?.Close();
                _channel?.Dispose();
                _connection?.Close();
                _connection?.Dispose();
                _logger.LogInformation("RabbitMQ connection closed");
            }

            _disposed = true;
        }

        /// <summary>
        /// Checks if the RabbitMQ connection is currently open and available.
        /// </summary>
        /// <returns>True if the connection is open; otherwise, false.</returns>
        public bool IsConnected()
        {
            return _connection != null && _connection.IsOpen;
        }
    }
}