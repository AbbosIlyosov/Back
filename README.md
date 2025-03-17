# ServiCar Backend Repository

## Project Structure
```
ServiCar-Backend/
│── src/
│   ├── Application/         # Business Logic Layer (CQRS, MediatR)
│   ├── Domain/              # Entities & Enums
│   ├── Infrastructure/      # Database (EF Core), Identity, Logging
│   ├── WebAPI/              # API Controllers
│── tests/                   # Unit & Integration Tests
│── .gitignore               # Ignore unnecessary files
│── README.md                # Project Documentation
│── docker-compose.yml       # Containerization Setup
│── ServiCar.sln             # Solution File
```

## Tech Stack
- **.NET 8 Web API**
- **Entity Framework Core**
- **MediatR (CQRS)**
- **MS SQL Server**
- **Identity Server (JWT Authentication)**
- **Quartz.NET (Background Jobs)**
- **SignalR (Real-time Updates)**
- **Serilog (Logging)**
- **xUnit, Moq, FluentAssertions (Testing)**

## Installation & Setup
### 1. Clone the Repository
```sh
git clone https://github.com/yourusername/ServiCar-Backend.git
cd ServiCar-Backend
```

### 2. Setup Database (MS SQL Server)
- Update `appsettings.json` with your **database connection string**.
- Run database migration:
```sh
dotnet ef database update
```

### 3. Run the Application
```sh
dotnet run --project src/WebAPI
```

### 4. API Documentation
- Access Swagger UI at `http://localhost:5000/swagger`

## Key Features
✅ **User Authentication (JWT, Identity)**
✅ **Role-based Access (Admin, Worker, User)**
✅ **Appointments & Business Management**
✅ **Reviews & Ratings System**
✅ **Real-time Updates (SignalR)**
✅ **Pagination & Filtering**
✅ **Unit & Integration Testing**
✅ **Docker & CI/CD Setup**


## Contributing
1. Fork the repository
2. Create a feature branch (`git checkout -b feature-new`)
3. Commit changes (`git commit -m 'Add new feature'`)
4. Push to branch (`git push origin feature-new`)
5. Open a Pull Request
