# Clean Architecture Documentation for Rescue System

## 1. Overview

This project follows the **Clean Architecture** pattern.

Clean Architecture organizes code into layers with clear separation of concerns, ensuring:

* Maintainability
* Scalability
* Testability
* Independence from frameworks

---

## 2. Core Principle

> **Dependency Rule**:
> Inner layers must NOT depend on outer layers.

```id="dep-rule"
WebAPI → Application → Domain
           ↑
     Infrastructure
```

* Dependencies always point **inward**
* Outer layers depend on inner layers
* Inner layers know nothing about outer layers

---

## 3. Architecture Layers

---

## 3.1 Domain Layer (Core)

### Purpose

Contains **business logic and entities**

### Responsibilities

* Define entities
* Define enums
* Define business rules

### Example

```csharp
public class RescueRequest
{
    public Guid Id { get; set; }
    public string Description { get; set; }
}
```

### Notes

* No dependency on any framework
* Pure C# logic only

---

## 3.2 Application Layer

### Purpose

Implements **use-cases (business workflows)**

### Responsibilities

* CQRS (Commands & Queries)
* DTOs
* Interfaces (Repository, Services)
* Business logic orchestration

---

### Structure

```
Application/
 ├── Common/
 │    ├── Responses/
 │    ├── Exceptions/
 │    └── Interfaces/
 │
 ├── Features/
 │    ├── Users/
 │    ├── RescueRequests/
 │    ├── Missions/
 │    └── Reports/
```

---

### Example (CQRS)

```csharp
public class GetUserByIdQuery : IRequest<ApiResponse<UserDto>>
{
    public Guid Id { get; set; }
}
```

---

## 3.3 Infrastructure Layer

### Purpose

Handles **external concerns**

### Responsibilities

* Database (EF Core)
* File storage
* External APIs
* Identity implementation

---

### Example

```csharp
public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await _context.Users.FindAsync(id);
    }
}
```

---

## 3.4 WebAPI Layer (Presentation)

### Purpose

Handles HTTP requests and responses

### Responsibilities

* Controllers
* Authentication / Authorization
* Request validation (basic)

---

### Example

```csharp
[HttpGet("{id}")]
public async Task<IActionResult> Get(Guid id)
{
    var result = await _mediator.Send(new GetUserByIdQuery { Id = id });
    return StatusCode(result.StatusCode, result);
}
```

---

## 4. Data Flow

```id="flow-clean"
Client
   ↓
Controller (WebAPI)
   ↓
Application (CQRS Handler)
   ↓
Domain (Entities)
   ↓
Infrastructure (Database)
```

---

## 5. Dependency Flow

| Layer          | Depends On           |
| -------------- | -------------------- |
| WebAPI         | Application          |
| Application    | Domain               |
| Infrastructure | Application + Domain |
| Domain         | (None)               |

---

## 6. Integration with CQRS

Clean Architecture is combined with CQRS:

* Application layer uses:

  * Commands → write
  * Queries → read

---

## 7. Identity & Authorization

The system uses **ASP.NET Identity** with **Role-Based Authorization**

### Roles

* Citizen
* Rescuer
* Dispatcher
* Commander

### Example

```csharp
[Authorize(Roles = "Dispatcher")]
```

---

## 8. Key Principles

### Separation of Concerns

Each layer has a single responsibility

---

### Independence

* Domain is independent of frameworks
* Application does not depend on EF Core

---

### Testability

* Business logic can be tested without database

---

### Maintainability

* Easy to modify without affecting other layers

---

## 9. Best Practices

✅ Use DTOs instead of Entities in API
✅ Keep Controllers thin
✅ Use Interfaces for repositories
✅ Use Dependency Injection
✅ Organize by Feature

---

## 10. Common Mistakes

❌ Putting business logic in Controller
❌ Letting Application depend on Infrastructure
❌ Returning Entity directly to API
❌ Mixing layers

---

## 11. Project Structure Example

```
src/
 ├── Domain/
 ├── Application/
 ├── Infrastructure/
 └── WebAPI/
```

---

## 12. Summary

Clean Architecture helps build systems that are:

* Scalable
* Flexible
* Easy to test
* Easy to maintain

It is especially suitable for large systems like the **Rescue System**.
