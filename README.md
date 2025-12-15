# 🚛 Truck Dispatcher - Complete Logistics Management System

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![Angular](https://img.shields.io/badge/Angular-21-DD0031?logo=angular)](https://angular.io/)
[![SQL Server](https://img.shields.io/badge/SQL%20Server-2022-CC2927?logo=microsoftsqlserver)](https://www.microsoft.com/sql-server)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)

A modern, full-stack logistics management platform designed for trucking companies to streamline their entire workflow—from client ordering to delivery completion. Built with Clean Architecture principles and industry best practices.

## 📋 Table of Contents

- [Overview](#-overview)
- [Key Features](#-key-features)
- [Technology Stack](#-technology-stack)
- [Architecture](#-architecture)
- [Project Structure](#-project-structure)
- [Getting Started](#-getting-started)
- [User Roles](#-user-roles)
- [Development Status](#-development-status)
- [Screenshots](#-screenshots)
- [Contributing](#-contributing)
- [License](#-license)

## 🎯 Overview

Truck Dispatcher is a comprehensive logistics management system that replaces traditional paper-based workflows with a modern digital platform. The system manages four distinct user roles, each with tailored interfaces and capabilities, creating a seamless workflow from product ordering through shipment creation to final delivery.

### What Makes This Different?

Unlike generic e-commerce platforms, this system is purpose-built for logistics operations:
- **Dispatcher-centric workflow**: Orders require approval before shipment creation
- **Resource management**: Assign trucks, trailers, and drivers to specific routes
- **Real-time tracking**: GPS integration for live shipment monitoring
- **Role-based access**: Four specialized interfaces (Admin, Dispatcher, Driver, Client)
- **Complete audit trail**: Track every step from order placement to delivery completion

## ✨ Key Features

### Core Functionality
- 🔐 **JWT Authentication** with refresh token rotation
- 👥 **Multi-role System**: Admin, Dispatcher, Driver, Client
- 📦 **Order Management**: Client ordering with dispatcher approval workflow
- 🚚 **Shipment Creation**: Assign routes, vehicles, and drivers
- 📍 **GPS Tracking**: Real-time location tracking with Leaflet maps
- 💬 **Real-time Chat**: SignalR-powered messaging between users
- 📊 **Inventory Management**: Track products, categories, and stock levels
- 🚛 **Vehicle Management**: Trucks and trailers with maintenance tracking
- 📱 **Responsive Design**: Works seamlessly on desktop and mobile

### Technical Highlights
- **Clean Architecture**: Clear separation of concerns (Domain, Application, Infrastructure, API)
- **CQRS Pattern**: Using MediatR for command/query separation
- **Entity Framework Core**: Code-first approach with migrations
- **Soft Delete**: Audit-friendly data management
- **Automated Testing**: Unit and integration test coverage
- **API Documentation**: Swagger/OpenAPI integration
- **Internationalization**: Multi-language support (English, Bosnian)

## 🛠 Technology Stack

### Backend
- **Framework**: .NET 8.0
- **Architecture**: Clean Architecture + CQRS
- **ORM**: Entity Framework Core 8.0
- **Database**: Microsoft SQL Server 2022
- **Authentication**: JWT with refresh tokens
- **Validation**: FluentValidation
- **Mediator**: MediatR
- **Real-time**: SignalR (planned)

### Frontend
- **Framework**: Angular 21 (Standalone Components)
- **UI Library**: Angular Material + Tailwind CSS
- **State Management**: RxJS + Signals
- **HTTP**: Angular HttpClient with interceptors
- **Routing**: Angular Router with guards
- **Maps**: Leaflet (planned)
- **Forms**: Reactive Forms with validators

### DevOps & Tools
- **Version Control**: Git + GitHub
- **Project Management**: Azure DevOps
- **Database Migrations**: EF Core Migrations
- **API Testing**: Swagger UI
- **Development**: Visual Studio 2022 / VS Code

## 🏗 Architecture

### Backend Architecture

```
Dispatcher.Backend/
├── Dispatcher.API/              # Entry point, Controllers, Middleware
│   ├── Controllers/             # REST API endpoints
│   ├── Middleware/              # Custom middleware
│   └── DependencyInjection.cs   # Service registration
│
├── Dispatcher.Application/      # Business logic, CQRS handlers
│   ├── Auth/                    # Authentication commands/queries
│   ├── Orders/                  # Order management
│   ├── Shipments/              # Shipment operations
│   ├── Vehicles/               # Vehicle management
│   └── Abstractions/           # Interfaces
│
├── Dispatcher.Domain/           # Entities, Business rules
│   ├── Entities/
│   │   ├── Identity/           # User, Role, RefreshToken
│   │   ├── Vehicles/           # Truck, Trailer, VehicleStatus
│   │   ├── Orders/             # Order, OrderItem
│   │   ├── Shipments/          # Shipment, Route
│   │   ├── Inventory/          # Product, Category
│   │   ├── Chat/               # Message, Notification
│   │   └── Dispatches/         # Dispatch assignments
│   └── Common/                 # Base entities, interfaces
│
└── Dispatcher.Infrastructure/   # Data access, External services
    ├── Database/
    │   ├── DatabaseContext.cs  # EF Core DbContext
    │   ├── Configurations/     # Entity configurations
    │   └── Seeders/            # Data seeding
    └── Services/               # External integrations
```

### Frontend Architecture

```
Dispatcher.Frontend/
├── src/
│   ├── app/
│   │   ├── api-services/       # HTTP services (1:1 with backend)
│   │   │   ├── auth/
│   │   │   ├── orders/
│   │   │   ├── products/
│   │   │   └── ...
│   │   │
│   │   ├── core/               # Core functionality
│   │   │   ├── components/     # Reusable base components
│   │   │   ├── guards/         # Route guards
│   │   │   ├── interceptors/   # HTTP interceptors
│   │   │   ├── models/         # Shared models
│   │   │   └── services/       # Core services (auth, state)
│   │   │
│   │   ├── modules/            # Feature modules
│   │   │   ├── admin/          # Admin interface
│   │   │   ├── auth/           # Login/Register
│   │   │   ├── dispatcher/     # Dispatcher interface (planned)
│   │   │   ├── driver/         # Driver interface (planned)
│   │   │   └── client/         # Client interface (planned)
│   │   │
│   │   └── shared/             # Shared components/utilities
│   │
│   ├── assets/                 # Static assets
│   └── environments/           # Environment configs
```

## 📁 Project Structure

### Database Entities

**Core Entities:**
- `UserEntity` - System users with role-based access
- `RefreshTokenEntity` - JWT refresh token management
- `TruckEntity` - Commercial vehicles
- `TrailerEntity` - Cargo trailers
- `VehicleStatusEntity` - Vehicle availability status
- `OrderEntity` - Client orders
- `OrderItemEntity` - Individual order items
- `ShipmentEntity` - Approved orders ready for transport
- `RouteEntity` - Delivery routes
- `DispatchEntity` - Shipment assignments (truck + driver + route)
- `InventoryEntity` - Product catalog
- `MessageEntity` - Chat messages
- `NotificationEntity` - System notifications
- `PhotoEntity` - Image uploads with metadata

### Key Features by Entity

**Users & Authentication:**
- Multi-role system (Admin, Dispatcher, Driver, Client)
- JWT authentication with refresh tokens
- Password hashing with security best practices
- Account lockout and two-factor authentication support

**Vehicle Management:**
- Trucks: License plate, VIN, make/model, capacity
- Trailers: Registration, capacity, maintenance tracking
- Status tracking: Available, In Transit, Maintenance
- GPS device integration

**Order Management:**
- Client order placement
- Multi-item orders with inventory validation
- Order status workflow: Pending → Approved → InTransit → Delivered
- Dispatcher approval required before shipment

**Shipment & Dispatch:**
- Route planning with origin/destination
- Resource assignment (truck, trailer, driver)
- Scheduled vs. actual departure/arrival times
- Delivery confirmation with signature and photos

## 🚀 Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Node.js 20+](https://nodejs.org/) and npm
- [SQL Server 2022](https://www.microsoft.com/sql-server) or [SQL Server Express](https://www.microsoft.com/sql-server/sql-server-downloads) (free)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)

### Backend Setup

1. **Clone the repository**
   ```bash
   git clone https://github.com/EldarSarajlic/Truck_Dispatcher.git
   cd Truck_Dispatcher/Dispatcher.Backend
   ```

2. **Configure the database**
   - Update `appsettings.json` with your SQL Server connection string:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Database=DispatcherDb;Trusted_Connection=True;TrustServerCertificate=True"
     }
   }
   ```
   
   Or with SQL Server authentication:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Database=DispatcherDb;User Id=sa;Password=YourPassword;TrustServerCertificate=True"
     }
   }
   ```

3. **Run migrations**
   ```bash
   dotnet ef database update --project Dispatcher.Infrastructure
   ```

4. **Start the API**
   ```bash
   cd Dispatcher.API
   dotnet run
   ```
   
   The API will be available at `https://localhost:7260`

### Frontend Setup

1. **Navigate to frontend directory**
   ```bash
   cd Truck_Dispatcher/Dispatcher.Frontend
   ```

2. **Install dependencies**
   ```bash
   npm install
   ```

3. **Configure environment**
   - Update `src/environments/environment.ts` with your API URL:
   ```typescript
   export const environment = {
     production: false,
     apiUrl: 'https://localhost:7260/api'
   };
   ```

4. **Start development server**
   ```bash
   npm start
   ```
   
   The application will open at `http://localhost:4200`

### Default Users

After seeding, the following test accounts are available:

| Email | Password | Role | Description |
|-------|----------|------|-------------|
| admin@dispatcher.local | Admin123! | Admin | Full system access |
| dispatcher@dispatcher.local | Dispatcher123! | Dispatcher | Order approval, shipment creation |
| driver@dispatcher.local | Driver123! | Driver | Delivery management |
| client@test.com | Client123! | Client | Product ordering |

## 👥 User Roles

### 🔧 Admin
**Responsibilities:**
- User management (create, edit, disable users)
- Vehicle management (trucks, trailers)
- System configuration
- Inventory management
- Analytics and reporting

**Key Features:**
- Complete CRUD operations on all entities
- System-wide analytics dashboard
- User role assignment
- Vehicle maintenance tracking

### 📋 Dispatcher
**Responsibilities:**
- Review and approve client orders
- Create shipments from approved orders
- Assign resources (trucks, drivers, routes)
- Monitor active deliveries
- Communicate with drivers and clients

**Key Features:**
- Order approval workflow
- Dispatch board for resource allocation
- Real-time tracking dashboard
- Route planning
- Shipment status updates

### 🚛 Driver
**Responsibilities:**
- View assigned deliveries
- Update delivery status
- Complete delivery confirmations
- Upload delivery photos
- Report issues

**Key Features:**
- Personal assignment list
- GPS navigation integration
- Signature capture
- Photo upload for proof of delivery
- Real-time status updates

### 🛒 Client
**Responsibilities:**
- Browse product catalog
- Place orders
- Track order status
- View order history
- Communicate with dispatchers

**Key Features:**
- Product catalog with search/filter
- Shopping cart functionality
- Order tracking with GPS
- Order history
- Real-time notifications

## 📊 Development Status

### ✅ Completed Features

**Backend:**
- [x] Clean Architecture project structure
- [x] Entity Framework Core setup with PostgreSQL
- [x] All core entities and relationships
- [x] JWT authentication with refresh tokens
- [x] User registration and login endpoints
- [x] CRUD operations for Products, Categories, Orders
- [x] Database seeding with test data
- [x] Soft delete implementation
- [x] Audit trail (CreatedAt, ModifiedAt)
- [x] FluentValidation integration

**Frontend:**
- [x] Angular 21 project setup with standalone components
- [x] Material Design + Tailwind CSS integration
- [x] Authentication system (login, logout, token refresh)
- [x] Protected routes with auth guards
- [x] API service layer (auth, products, orders)
- [x] Admin product management (list, add, edit, delete)
- [x] Order management with status filters
- [x] Internationalization (English, Bosnian)
- [x] Responsive layout with sidebar navigation
- [x] Loading indicators and error handling

### 🚧 In Progress

- [ ] Dispatcher module (order approval, shipment creation)
- [ ] Driver module (delivery assignments)
- [ ] Client module (product catalog, ordering)
- [ ] GPS tracking with Leaflet maps
- [ ] Real-time chat with SignalR
- [ ] Vehicle management UI
- [ ] User management UI

### 📅 Planned Features

- [ ] Mobile app (React Native or Flutter)
- [ ] Advanced analytics and reporting
- [ ] Route optimization algorithms
- [ ] Automated notifications (SMS, email)
- [ ] Document generation (invoices, delivery notes)
- [ ] Integration with accounting systems
- [ ] Multi-tenant support
- [ ] API rate limiting and caching

## 📸 Screenshots

### Login Page
Modern authentication interface with email/password validation and remember me functionality.

### Admin Dashboard
Overview of system statistics, recent orders, vehicle status, and quick actions.

### Order Management
Searchable, filterable order list with status badges and detailed order views.

### GPS Tracking (Coming Soon)
Real-time map showing active deliveries with driver locations and estimated arrival times.

## 🤝 Contributing

This is an academic project developed as part of a university coursework. While it's not open for external contributions, feedback and suggestions are welcome!

### Development Team
- **Eldar Sarajlić** - Full-stack Developer
- **Haris Šarić** - Frontend/Backend Development
- **Ali Mustafić** - Frontend/Backend Development
- **Academic Supervisors** - Adil Joldić, Azra Smajić

### Learning Objectives
This project demonstrates:
- Modern software architecture principles
- Full-stack development skills
- Real-world problem-solving
- Team collaboration using Agile methodology
- Professional development practices

---

## 🙏 Acknowledgments

- University faculty for project guidance
- Open-source community for excellent tools and libraries
- Team members for their dedication and hard work

---

**Built with ❤️ using .NET and Angular**
