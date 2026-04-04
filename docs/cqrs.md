# CQRS Structure Documentation for Rescue System

## 1. Overview

This project follows the **CQRS (Command Query Responsibility Segregation)** pattern within **Clean Architecture**.

CQRS separates application logic into:

* **Commands** → modify data
* **Queries** → retrieve data

This improves:

* Maintainability
* Scalability
* Code clarity

---

## 2. High-Level Flow

````
Client
   ↓
Controller (WebAPI)
   ↓
MediatR (Send Command/Query)
   ↓
Application Layer (Handler)
   ↓
Domain / Repository
   ↓
Database
``` id="flow001"

---

## 3. Application Layer Structure

````

Application/
├── Common/
│    ├── Responses/
│    │     └── ApiResponse.cs
│    ├── Exceptions/
│    └── Interfaces/
│
├── Features/
│    ├── Users/
│    │    ├── Commands/
│    │    ├── Queries/
│    │    ├── DTOs/
│    │    └── Handlers/
│    │
│    ├── RescueRequests/
│    ├── Missions/
│    └── Reports/
│
└── DependencyInjection.cs

````id="structure001"

---

## 4. Command (Write Operations)

### Definition
A **Command** represents an action that changes the system state.

### Characteristics
- Modifies data
- Does NOT return complex data
- Returns:
  - ID
  - ApiResponse
  - Boolean

---

### Example

```csharp
public class CreateRescueRequestCommand : IRequest<ApiResponse<Guid>>
{
    public string Description { get; set; }
    public string Location { get; set; }
}
````

---

### Handler

```csharp
public class CreateRescueRequestHandler 
    : IRequestHandler<CreateRescueRequestCommand, ApiResponse<Guid>>
{
    public async Task<ApiResponse<Guid>> Handle(
        CreateRescueRequestCommand request,
        CancellationToken cancellationToken)
    {
        // Business logic here

        return ApiResponse<Guid>.SuccessResponse(Guid.NewGuid(), "Request created");
    }
}
```

---

## 5. Query (Read Operations)

### Definition

A **Query** retrieves data without modifying system state.

### Characteristics

* Read-only
* Returns DTOs
* No side effects

---

### Example

```csharp
public class GetRescueRequestByIdQuery 
    : IRequest<ApiResponse<RescueRequestDto>>
{
    public Guid Id { get; set; }
}
```

---

### Handler

```csharp
public class GetRescueRequestByIdHandler 
    : IRequestHandler<GetRescueRequestByIdQuery, ApiResponse<RescueRequestDto>>
{
    public async Task<ApiResponse<RescueRequestDto>> Handle(
        GetRescueRequestByIdQuery request,
        CancellationToken cancellationToken)
    {
        // Fetch data

        return ApiResponse<RescueRequestDto>.SuccessResponse(dto);
    }
}
```

---

## 6. DTOs (Data Transfer Objects)

DTOs are used to:

* Transfer data between layers
* Avoid exposing Domain Entities

### Rules

* Do NOT return Entity directly
* Keep DTOs simple and flat

---

## 7. ApiResponse Standard

All responses must follow a unified structure:

```json
{
  "success": true,
  "message": "Success",
  "data": {},
  "statusCode": 200
}
```

---

## 8. Controller Integration

Controllers should be thin and contain no business logic.

### Example

```csharp
[HttpPost]
public async Task<IActionResult> Create(CreateRescueRequestCommand command)
{
    var result = await _mediator.Send(command);
    return StatusCode(result.StatusCode, result);
}
```

---

## 9. Role-Based Authorization

The system uses **ASP.NET Identity Roles**:

* Citizen → create requests
* Rescuer → handle missions
* Dispatcher → assign tasks
* Commander → monitor system

### Example

```csharp
[Authorize(Roles = "Dispatcher")]
```

---

## 10. Key Principles

### Separation of Concerns

* Commands = write
* Queries = read

### Single Responsibility

* Each handler handles ONE use-case

### No Business Logic in Controllers

* Controllers only forward requests

### Dependency Rule

* Application depends on Domain
* Infrastructure implements interfaces

---

## 11. Best Practices

✅ Use MediatR for dispatching
✅ Use ApiResponse for consistency
✅ Use DTOs for output
✅ Organize by Feature

---

## 12. Common Mistakes

❌ Mixing Command and Query
❌ Returning Entity directly
❌ Writing logic in Controller
❌ Skipping ApiResponse

---

## 13. Example Feature Structure

```
Features/
 ├── RescueRequests/
 │    ├── Commands/
 │    │     └── CreateRescueRequestCommand.cs
 │    │
 │    ├── Queries/
 │    │     └── GetRescueRequestByIdQuery.cs
 │    │
 │    ├── DTOs/
 │    │     └── RescueRequestDto.cs
 │    │
 │    └── Handlers/
 │          ├── CreateRescueRequestHandler.cs
 │          └── GetRescueRequestByIdHandler.cs
```

---

## 14. Summary

| Type    | Purpose       |
| ------- | ------------- |
| Command | Modify data   |
| Query   | Retrieve data |

CQRS ensures:

* Clean separation
* Better scalability
* Easier maintenance
