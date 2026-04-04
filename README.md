# 🚨 Rescue System - Hệ thống điều phối cứu hộ

Một hệ thống quản lý cứu hộ hiện đại được xây dựng trên nền tảng **Clean Architecture** và **.NET 10**, cung cấp các giải pháp toàn diện cho việc tổ chức, quản lý và điều phối các hoạt động cứu hộ.

---

## 📋 Mục Lục

- [Tổng Quan](#tổng-quan)
- [Công Nghệ](#công-nghệ)
- [Kiến Trúc](#kiến-trúc)
- [Cài Đặt](#cài-đặt)
- [Cấu Trúc Dự Án](#cấu-trúc-dự-án)
- [Hướng Dẫn Sử Dụng](#hướng-dẫn-sử-dụng)
- [API Documentation](#api-documentation)
- [Các Tính Năng](#các-tính-năng)
- [Đóng Góp](#đóng-góp)

---

## 🎯 Tổng Quan

**Rescue System** là một nền tảng quản lý toàn diện cho các hoạt động cứu hộ, hỗ trợ:

✅ Quản lý yêu cầu cứu hộ  
✅ Quản lý đội cứu hộ  
✅ Theo dõi các nhiệm vụ cứu hộ  
✅ Quản lý báo cáo và thống kê  
✅ Xác thực và phân quyền người dùng  

Hệ thống sử dụng **Clean Architecture** để đảm bảo tính bảo trì cao, khả năng mở rộng và độc lập với framework.

---

## 🛠️ Công Nghệ

| Công Nghệ | Phiên Bản | Mục Đích |
|-----------|----------|---------|
| **.NET** | 10.0 | Nền tảng chính |
| **ASP.NET Core** | 10.0 | Web API Framework |
| **Entity Framework Core** | 10.0 | ORM cho cơ sở dữ liệu |
| **ASP.NET Identity** | 10.0 | Xác thực & Phân quyền |
| **Swagger/OpenAPI** | 10.1.7 | Tài liệu API tương tác |
| **SQL Server** | - | Cơ sở dữ liệu |

---

## 🏗️ Kiến Trúc

### Clean Architecture Pattern

Dự án theo dõi **Clean Architecture** với 4 lớp chính:

```
┌─────────────────────────────────────┐
│      WebAPI (Presentation)          │ ← HTTP Requests
├─────────────────────────────────────┤
│    Application (Business Logic)     │ ← CQRS, Services
├─────────────────────────────────────┤
│    Domain (Core Business Rules)     │ ← Entities, Enums
├─────────────────────────────────────┤
│  Infrastructure (Data Access)       │ ← Database, EF Core
└─────────────────────────────────────┘
```

### Nguyên Tắc Dependency Rule

```
Dependencies ➜ Inward (Luôn hướng vào)

Domain Layer (lõi): Không phụ thuộc vào bất kỳ lớp nào
    ↑
Application Layer: Phụ thuộc vào Domain
    ↑
Infrastructure Layer: Phụ thuộc vào Domain + Application
    ↑
WebAPI Layer: Phụ thuộc vào Application
```

---

## 📁 Cấu Trúc Dự Án

```
RescueSystem/
├── src/
│   ├── RescueSystem.Domain/
│   │   └── Entities/
│   │       ├── User.cs                    # Entity người dùng
│   │       ├── Role.cs                    # Entity vai trò
│   │       └── Base/
│   │           └── BaseEntities.cs        # Entity cơ sở
│   │
│   ├── RescueSystem.Application/
│   │   ├── Common/
│   │   │   ├── Response/
│   │   │   │   └── ApiResponse.cs         # Mẫu phản hồi API
│   │   │   ├── Exception/
│   │   │   │   ├── BadRequestException.cs
│   │   │   │   ├── NotFoundException.cs
│   │   │   │   ├── UnauthorizedException.cs
│   │   │   │   └── InternalServerErrorException.cs
│   │   │   └── Interfaces/
│   │   ├── Features/
│   │   │   ├── Users/
│   │   │   ├── RescueRequests/
│   │   │   ├── Missions/
│   │   │   └── Reports/
│   │   ├── DTOs/
│   │   │   └── User/
│   │   │       ├── UserDto.cs
│   │   │       ├── CreateUserDto.cs
│   │   │       └── UpdateUserDto.cs
│   │   ├── Services/
│   │   │   └── UserService.cs             # Dịch vụ quản lý người dùng
│   │   └── DependencyInjection.cs         # Cấu hình DI
│   │
│   ├── RescueSystem.Infrastructure/
│   │   ├── Data/
│   │   │   └── ApplicationDbContext.cs    # DbContext chính
│   │   ├── Repositories/
│   │   ├── Services/
│   │   └── DependencyInjection.cs         # Cấu hình DI
│   │
│   └── RescueSystem.Api/
│       ├── Controllers/
│       │   └── UsersController.cs         # API Endpoints cho User
│       ├── Program.cs                     # Cấu hình ứng dụng
│       ├── appsettings.json               # Cấu hình
│       ├── appsettings.Development.json
│       └── launchSettings.json
│
├── docs/
│   └── clean_architecture.md              # Tài liệu Clean Architecture
│
└── README.md
```

---

## ⚙️ Cài Đặt

### Yêu Cầu

- **.NET 10 SDK** hoặc cao hơn
- **SQL Server** (hoặc chỉnh sửa connection string)
- **Visual Studio 2022** hoặc **Visual Studio Code**

### Bước 1: Clone Repository

```bash
git clone https://github.com/yourusername/rescue-system.git
cd rescue-system
```

### Bước 2: Cấu Hình Connection String

Mở `src/RescueSystem.Api/appsettings.json` và cập nhật connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=RescueSystemDb;Trusted_Connection=true;Encrypt=false;"
  }
}
```

### Bước 3: Áp Dụng Migration

```bash
cd src/RescueSystem.Infrastructure
dotnet ef database update
```

Hoặc sử dụng Package Manager Console trong Visual Studio:

```powershell
Update-Database
```

### Bước 4: Chạy Ứng Dụng

```bash
cd src/RescueSystem.Api
dotnet run
```

Ứng dụng sẽ khởi động tại: `https://localhost:5001`

---

## 🚀 Hướng Dẫn Sử Dụng

### Swagger UI

Swagger UI tự động được phục vụ tại **root** (`/`) khi chạy ở chế độ **Development**.

Truy cập: `https://localhost:5001/`

### Xác Thực

Hệ thống sử dụng **ASP.NET Identity** với **JWT** (tuỳ chọn).

Các vai trò có sẵn:
- **Citizen** - Công dân
- **Rescuer** - Người cứu hộ
- **Dispatcher** - Người điều phối
- **Commander** - Chỉ huy

---

## 📚 API Documentation

### Users Endpoints

#### Tạo Người Dùng
```http
POST /api/users
Content-Type: application/json

{
  "email": "user@example.com",
  "fullName": "John Doe",
  "phoneNumber": "0123456789",
  "address": "123 Street, City",
  "dateOfBirth": "1990-01-01"
}
```

#### Lấy Người Dùng
```http
GET /api/users/{id}
```

#### Cập Nhật Người Dùng
```http
PUT /api/users/{id}
Content-Type: application/json

{
  "fullName": "Jane Doe",
  "phoneNumber": "0987654321"
}
```

#### Xoá Người Dùng
```http
DELETE /api/users/{id}
```

---

## ✨ Các Tính Năng

### Hiện Tại
- ✅ Quản lý người dùng
- ✅ Xác thực ASP.NET Identity
- ✅ Phân quyền theo vai trò
- ✅ Swagger API Documentation
- ✅ Exception Handling
- ✅ API Response standardization

### Sắp Tới
- 🔄 Quản lý yêu cầu cứu hộ
- 🔄 Quản lý nhiệm vụ
- 🔄 Hệ thống báo cáo
- 🔄 Thông báo real-time (SignalR)
- 🔄 Logging & Monitoring

---

## 📖 Tài Liệu Clean Architecture

Chi tiết về kiến trúc có thể được tìm thấy trong:

📄 **[docs/clean_architecture.md](docs/clean_architecture.md)**

Tài liệu bao gồm:
- Nguyên tắc Dependency Rule
- Giải thích từng lớp
- Data Flow
- Best Practices
- Các lỗi phổ biến

---

## 🔧 Phát Triển

### Tạo Migration Mới

```bash
cd src/RescueSystem.Infrastructure
dotnet ef migrations add MigrationName
```

### Chạy Tests

```bash
dotnet test
```

### Build Solution

```bash
dotnet build
```

---

## 📝 Quy Tắc Code

### Đặt Tên
- **Classes/Interfaces**: `PascalCase`
- **Methods**: `PascalCase`
- **Variables**: `camelCase`
- **Constants**: `UPPER_CASE`

### Tổ Chức Lớp
1. Properties
2. Constructor
3. Public Methods
4. Private Methods

### Comment
Chỉ thêm comment khi logic không rõ ràng.

---

## 🤝 Đóng Góp

Chúng tôi chào đón những đóng góp! Vui lòng:

1. Fork repository
2. Tạo branch feature (`git checkout -b feature/AmazingFeature`)
3. Commit changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to branch (`git push origin feature/AmazingFeature`)
5. Mở Pull Request

---

## 📄 Giấy Phép

Dự án này được cấp phép theo **MIT License** - xem file [LICENSE](LICENSE) để biết chi tiết.

---

## 📧 Liên Hệ

**Email**: your-email@example.com  
**Website**: https://rescuesystem.example.com

---

## 🎓 Tham Khảo

- [Clean Architecture by Robert C. Martin](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [ASP.NET Core Documentation](https://learn.microsoft.com/en-us/aspnet/core/)
- [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/)

---

## 📊 Status

| Thành Phần | Status |
|-----------|--------|
| Domain Layer | ✅ Hoàn tất |
| Application Layer | 🔄 Đang phát triển |
| Infrastructure Layer | 🔄 Đang phát triển |
| WebAPI Layer | 🔄 Đang phát triển |
| Documentation | ✅ Hoàn tất |

---

**Last Updated**: 2024  
**Maintained by**: Your Team  
**Version**: 1.0.0
