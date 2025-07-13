# K1QuickGen.Api

## Overview

K1QuickGen.Api is an ASP.NET Core Web API for managing U.S. partnership tax returns (Form 1065) and related partner submissions. The project demonstrates clean architecture, separation of concerns, and integration with RabbitMQ for asynchronous messaging and background processing.

---

## Key Features

- **Form 1065 Management:** Create, retrieve, and manage partnership tax returns.
- **Partner Submissions:** Add and manage partner information for each return.
- **PDF Generation:** Generate Form 1065 and K-1 PDFs on demand or via background jobs.
- **RabbitMQ Integration:** Asynchronous event publishing and background processing for scalable workflows.
- **MongoDB Integration:** Stores all tax return and partner data.
- **Health Checks:** API endpoint to verify RabbitMQ connectivity.

---

## Project Structure

- `Controllers/` - API endpoints for forms, partners, returns, and health checks.
- `Services/` - Business logic, messaging, and background processing.
- `Repositories/` - Data access logic for MongoDB.
- `Messages/` - Message contracts for RabbitMQ events and commands.
- `Middleware/` - Global error handling.
- `Interfaces/` - Service and repository contracts.
- `Models/` - Domain models.
- `Contracts/Dtos/` - Data transfer objects for API communication.

---

## Messaging & Background Processing

- **Event Publishing:**  
  Business services publish events (e.g., `Form1065SubmittedMessage`) to RabbitMQ after database operations.
- **Background Processing:**  
  `TaxFormMessageProcessor` runs as a hosted service, consuming messages and triggering background tasks (e.g., PDF generation).
- **Message Contracts:**  
  Message payloads are intentionally minimal, containing only the data needed for the event or command.

---

## Running the Project

1. **Prerequisites:**
   - [.NET 8 SDK](https://dotnet.microsoft.com/download)
   - [MongoDB](https://www.mongodb.com/try/download/community)
   - [RabbitMQ](https://www.rabbitmq.com/download.html) (or run via Docker)

2. **Configuration:**
   - Update `appsettings.json` for MongoDB and RabbitMQ connection strings as needed.

3. **Start the API:**

4. **Access Swagger UI:**
- Navigate to `https://localhost:{port}/swagger` for interactive API documentation and testing.

5. **RabbitMQ Management UI:**
- Visit [http://localhost:15672/](http://localhost:15672/) (default user: `guest`, password: `guest`).

---

## Useful Endpoints

- `POST /api/Form1065` - Create a new Form 1065
- `POST /api/Partners` - Add a partner to a Form 1065
- `GET /api/Form1065/{form1065Id}` - Get a specific Form 1065
- `GET /api/Form1065/generate-pdf/{form1065Id}` - Generate PDF for a Form 1065
- `GET /api/HealthCheck/rabbitmq` - Check RabbitMQ connectivity

---

## Development Notes

- **Dependency Injection:** All services, repositories, and background processors are registered via DI.
- **Error Handling:** Global error handling middleware returns standardized JSON error responses.
- **Extensibility:** Message contracts and background processing can be extended for additional workflows or integrations.
- **Testing:** Use Swagger or Postman to test API endpoints. Ensure RabbitMQ and MongoDB are running.

---

## Design Patterns Used

This project demonstrates several well-established software design patterns to ensure maintainability, scalability, and clarity:

- **Dependency Injection (DI):**  
  All controllers, services, repositories, and background processors are registered and resolved via DI, promoting loose coupling and testability.

- **Repository Pattern:**  
  Data access logic is encapsulated in repository classes (e.g., `Form1065Repository`, `PartnerSubmissionRepository`), abstracting MongoDB operations from business logic.

- **Service Layer Pattern:**  
  Business logic is centralized in service classes (e.g., `Form1065Service`, `PartnersSubmissionService`), keeping controllers thin and focused on HTTP concerns.

- **DTO (Data Transfer Object) Pattern:**  
  DTOs are used to transfer data between the API and clients, ensuring that only necessary data is exposed and received.

- **Observer / Publish-Subscribe Pattern:**  
  The system uses RabbitMQ to implement event-driven communication. Services publish events (such as form or partner submissions), and background processors (like `TaxFormMessageProcessor`) subscribe to and handle these events asynchronously.

- **Command Pattern:**  
  Command messages (e.g., `GeneratePdfCommand`) encapsulate requests for background actions, decoupling the request from its execution.

- **Background Service Pattern:**  
  The `TaxFormMessageProcessor` is implemented as a hosted background service, enabling continuous message consumption and processing outside the HTTP request lifecycle.

- **Factory Pattern (via DI):**  
  `IServiceScopeFactory` is used in background services to create new scopes for resolving scoped dependencies during message processing.

- **Error Handling Middleware:**  
  A custom middleware provides centralized exception handling and standardized error responses for all API endpoints.

These patterns collectively provide a robust, extensible, and maintainable foundation for the application's architecture.

---

## Troubleshooting

- **RabbitMQ Connection Issues:**  
Check the management UI and logs. Ensure the connection string in `appsettings.json` matches your RabbitMQ instance.
- **Null Fields in Database:**  
  Ensure you are sending the full DTO as shown in Swagger when creating or updating records.
- **Background Processing Not Triggering:**  
  Confirm that `TaxFormMessageProcessor` is running and that messages are being published to the correct queue.

---

## License

This project is for demonstration and educational purposes.