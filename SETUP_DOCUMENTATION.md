# Rescue System - Setup Documentation

## Overview
Complete implementation of User and Role management with ASP.NET Core Identity, database setup, and CRUD controller with Swagger API documentation.

---

## 1. ENTITIES SETUP

### User Entity
- **File**: `src/RescueSystem.Domain/Entities/User.cs`
- **Inherits from**: `IdentityUser<Guid>`
- **Custom Properties**:
  - `FullName`: User's full name
  - `PhoneNumber`: Contact phone number
  - `Address`: User's address
  - `DateOfBirth`: Birth date
  - `Avatar`: Profile avatar URL
  - `IsActive`: Boolean flag for activation status
  - `CreatedAt`: Timestamp of creation
  - `UpdatedAt`: Timestamp of last update

### Role Entity
- **File**: `src/RescueSystem.Domain/Entities/Role.cs`
- **Inherits from**: `IdentityRole<Guid>`
- **Custom Properties**:
  - `Description`: Role description
  - `CreatedAt`: Timestamp of creation
  - `UpdatedAt`: Timestamp of last update

---

## 2. DATABASE CONFIGURATION

### ApplicationDbContext
- **File**: `src/RescueSystem.Infrastructure/Data/ApplicationDbContext.cs`
- **Key Features**:
  - Inherits from `IdentityDbContext<User, Role, Guid>`
  - Configured for SQL Server
  - All entity tables properly mapped:
    - `Users`
    - `Roles`
    - `UserRoles`
    - `UserClaims`
    - `UserLogins`
    - `RoleClaims`
    - `UserTokens`
  - Automatic database creation on startup

### Connection String
- **File**: `src/RescueSystem.Api/appsettings.json`
- **Default**: `Server=(localdb)\\mssqllocaldb;Database=RescueSystemDb;Trusted_Connection=true;`

### Infrastructure Configuration
- **File**: `src/RescueSystem.Infrastructure/DependencyInjection.cs`
- Identity options:
  - Password minimum length: 6 characters
  - No digit requirement
  - No uppercase requirement
  - No special character requirement
  - Unique email required

---

## 3. DTOs (Data Transfer Objects)

### CreateUserDto
- `FullName`: User's full name
- `Email`: Email address
- `UserName`: Username
- `PhoneNumber`: Phone number
- `Address`: Address
- `DateOfBirth`: Date of birth
- `Avatar`: Avatar URL
- `Password`: User password
- `Roles`: List of role names to assign

### UpdateUserDto
- `FullName`: User's full name
- `PhoneNumber`: Phone number
- `Address`: Address
- `DateOfBirth`: Date of birth
- `Avatar`: Avatar URL
- `IsActive`: Activation status
- `Roles`: List of role names to assign

### UserDto (Read)
- All user properties including:
  - `Id`: User GUID
  - `CreatedAt`, `UpdatedAt`: Timestamps
  - `Roles`: List of assigned roles

---

## 4. USER SERVICE

### IUserService Interface & Implementation
- **File**: `src/RescueSystem.Application/Services/UserService.cs`

**Methods**:
- `CreateUserAsync(CreateUserDto)`: Create new user with roles
- `GetUserByIdAsync(Guid)`: Retrieve user by ID
- `GetAllUsersAsync()`: Get all users
- `UpdateUserAsync(Guid, UpdateUserDto)`: Update user information
- `DeleteUserAsync(Guid)`: Delete user
- `ChangePasswordAsync(Guid, string, string)`: Change password

---

## 5. USERS CONTROLLER (CRUD API)

### Endpoints
- **File**: `src/RescueSystem.Api/Controllers/UsersController.cs`

| HTTP Method | Endpoint | Description |
|-------------|----------|-------------|
| POST | `/api/users` | Create new user |
| GET | `/api/users` | Get all users |
| GET | `/api/users/{userId}` | Get user by ID |
| PUT | `/api/users/{userId}` | Update user |
| DELETE | `/api/users/{userId}` | Delete user |
| POST | `/api/users/{userId}/change-password` | Change user password |

---

## 6. SWAGGER API DOCUMENTATION

### Configuration
- **File**: `src/RescueSystem.Api/Program.cs`
- **Swagger Endpoint**: `http://localhost:5000/` (root when in Development)
- **Swagger JSON**: `/swagger/v1/swagger.json`
- **UI**: Accessible at application root in Development environment

### Features
- Interactive API documentation
- Try-it-out functionality
- Automatic endpoint discovery

---

## 7. PROJECT STRUCTURE

```
src/
├── RescueSystem.Domain/
│   └── Entities/
│       ├── User.cs (IdentityUser<Guid>)
│       └── Role.cs (IdentityRole<Guid>)
├── RescueSystem.Application/
│   ├── DTOs/
│   │   └── User/
│   │       ├── CreateUserDto.cs
│   │       ├── UpdateUserDto.cs
│   │       └── UserDto.cs
│   ├── Services/
│   │   └── UserService.cs
│   └── DependencyInjection.cs
├── RescueSystem.Infrastructure/
│   ├── Data/
│   │   └── ApplicationDbContext.cs
│   └── DependencyInjection.cs
└── RescueSystem.Api/
    ├── Controllers/
    │   └── UsersController.cs
    ├── Program.cs
    └── appsettings.json
```

---

## 8. PACKAGES INSTALLED

- `Microsoft.AspNetCore.Identity.EntityFrameworkCore`: 10.0.0
- `Microsoft.EntityFrameworkCore.SqlServer`: 10.0.0
- `Swashbuckle.AspNetCore`: 6.4.0
- `Microsoft.OpenApi`: 2.0.0

---

## 9. RUNNING THE APPLICATION

1. **Restore packages**:
   ```bash
   dotnet restore
   ```

2. **Build**:
   ```bash
   dotnet build
   ```

3. **Run**:
   ```bash
   dotnet run
   ```

4. **Access Swagger UI**:
   - Navigate to `http://localhost:5000` in your browser (development only)
   - Or go directly to `/swagger/ui`

---

## 10. EXAMPLE API CALLS

### Create User
```bash
curl -X POST "http://localhost:5000/api/users" \
  -H "Content-Type: application/json" \
  -d '{
    "fullName": "John Doe",
    "email": "john@example.com",
    "userName": "johndoe",
    "phoneNumber": "1234567890",
    "address": "123 Main St",
    "dateOfBirth": "1990-01-01",
    "avatar": "https://example.com/avatar.jpg",
    "password": "Password123",
    "roles": ["Admin"]
  }'
```

### Get All Users
```bash
curl -X GET "http://localhost:5000/api/users"
```

### Get User by ID
```bash
curl -X GET "http://localhost:5000/api/users/{userId}"
```

### Update User
```bash
curl -X PUT "http://localhost:5000/api/users/{userId}" \
  -H "Content-Type: application/json" \
  -d '{
    "fullName": "Jane Doe",
    "phoneNumber": "0987654321",
    "address": "456 Oak St",
    "dateOfBirth": "1995-05-15",
    "avatar": "https://example.com/new-avatar.jpg",
    "isActive": true,
    "roles": ["User"]
  }'
```

### Change Password
```bash
curl -X POST "http://localhost:5000/api/users/{userId}/change-password" \
  -H "Content-Type: application/json" \
  -d '{
    "currentPassword": "Password123",
    "newPassword": "NewPassword456"
  }'
```

### Delete User
```bash
curl -X DELETE "http://localhost:5000/api/users/{userId}"
```

---

## 11. NEXT STEPS

1. **Authentication**: Implement JWT authentication for endpoints
2. **Authorization**: Add role-based authorization policies
3. **Validation**: Add FluentValidation for DTOs
4. **Logging**: Enhance logging with Serilog
5. **Testing**: Add unit and integration tests
6. **Migration**: Create EF Core migrations for production deployment

---

**Status**: ✅ Complete and Ready to Use
