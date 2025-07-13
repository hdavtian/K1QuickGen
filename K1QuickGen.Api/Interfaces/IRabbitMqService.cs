using System;

namespace K1QuickGen.Api.Interfaces
{
    public interface IRabbitMqService
    {
        bool IsConnected();
        void Publish<T>(T message) where T : class;
        void StartConsuming();
        // Overload to support custom message processing
        void StartConsuming(Action<string> messageProcessor);
    }
}
