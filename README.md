# 🚛 Truck Dispatcher — Logistics Management System

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![Angular](https://img.shields.io/badge/Angular-21-DD0031?logo=angular)](https://angular.dev/)
[![SQL Server](https://img.shields.io/badge/SQL%20Server-2022-CC2927?logo=microsoftsqlserver)](https://www.microsoft.com/sql-server)
[![Tailwind](https://img.shields.io/badge/Tailwind-v4-06B6D4?logo=tailwindcss)](https://tailwindcss.com/)
[![DaisyUI](https://img.shields.io/badge/DaisyUI-v5-5A0EF8?logo=daisyui)](https://daisyui.com/)

> **Status:** Ongoing

A full-stack logistics platform for trucking companies, replacing paper-based workflows with a digital system that manages the entire pipeline from client ordering through dispatch to delivery.

---

## Overview

The system is built around four user roles, each with a dedicated interface:

**Client** places an order → **Dispatcher** reviews and approves it → creates a **Shipment** → assigns a **Truck**, **Trailer**, and **Driver** via a **Dispatch** → **Driver** completes the delivery.

Orders and Shipments are intentionally separate entities with distinct status lifecycles and role ownership.

---

## Tech Stack

### Backend

| Layer | Technology |
|---|---|
| Framework | .NET 8, ASP.NET Core |
| Architecture | Clean Architecture + CQRS (MediatR) |
| ORM | Entity Framework Core 8 (Code-First) |
| Database | Microsoft SQL Server 2022 |
| Auth | JWT with refresh token rotation |
| Validation | FluentValidation |
| Real-time | SignalR |
| Docs | Swagger / OpenAPI |

### Frontend

| Layer | Technology |
|---|---|
| Framework | Angular 21 (NgModule-based, not standalone) |
| Styling | Three-layer system: Tailwind v4 (layout) → DaisyUI v5 (components) → Component SCSS (animations, pseudo-elements) |
| State | Angular Signals for local state, RxJS for HTTP streams |
| Icons | Phosphor Icons (duotone) |
| Charts | Chart.js |
| Maps | Leaflet.js |
| i18n | ngx-translate (English, Bosnian) |
| HTTP | Angular HttpClient with interceptors (auth, loading bar, error logging) |

### Tools

| Purpose | Tool |
|---|---|
| IDE | VS Code | VS
| Version Control | Git + GitHub |
| Git Client | SourceTree |
| Project Management | Azure DevOps (sprints, user stories) |
| API Testing | Swagger UI |
| Prototyping | Single-file HTML mockups |

---

## Architecture

### Backend — Clean Architecture (CQRS Pattern)

```
Dispatcher.Backend/
├── Dispatcher.API              → Controllers, middleware, DI registration
├── Dispatcher.Application      → CQRS handlers (Commands + Queries), FluentValidation
├── Dispatcher.Domain           → Entities, enums, business rules
├── Dispatcher.Infrastructure   → EF Core DbContext, migrations, seeders, external services
├── Dispatcher.Shared           → Cross-cutting helpers
└── Dispatcher.Tests            → Unit and integration tests
```

### Frontend — Feature Modules

```
Dispatcher.Frontend/src/app/
├── api-services/       → HTTP services (one per backend resource)
├── core/               → Guards, interceptors, auth services, models
├── modules/
│   ├── admin/          → Dashboard, Users, Vehicles, Trailers, Inventory
│   ├── auth/           → Login, Forgot Password
│   ├── client/         → Dashboard, Catalog, My Orders
│   ├── dispatcher/     → Dispatch Board, Shipments, Live Tracking, Orders
│   ├── driver/         → Dashboard, My Assignments
│   ├── settings/       → User settings
│   ├── shared/         → SharedModule (sidebar, pipes, common components)
│   └── not-found/      → 404 page
└── shared/             → Cross-module utilities
```

---

## Domain Entities

The core data model:

- **UserEntity** — multi-role (Admin, Dispatcher, Driver, Client) with JWT refresh tokens
- **OrderEntity / OrderItemEntity** — client orders linked to inventory, with priority and delivery info
- **ShipmentEntity** — created from an approved order, linked to a route (one-to-one with Order)
- **DispatchEntity** — assigns truck + driver + trailer to a shipment
- **RouteEntity** — origin and destination cities
- **TruckEntity / TrailerEntity** — vehicles with status tracking and maintenance records
- **InventoryEntity** — product catalog with stock levels
- **MessageEntity / NotificationEntity** — chat and system notifications
- **CityEntity / CountryEntity** — location reference data
- **ServiceCompanyEntity / TruckServiceAssignmentEntity** — maintenance service tracking

---

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Node.js 20+](https://nodejs.org/) and npm
- [SQL Server 2022](https://www.microsoft.com/sql-server) (Express edition is fine)

### Backend

```bash
git clone https://github.com/EldarSarajlic/Truck_Dispatcher.git
cd Truck_Dispatcher/Dispatcher.Backend

# Configure your connection string in appsettings.json
# Then run migrations and start:
dotnet ef database update --project Dispatcher.Infrastructure
cd Dispatcher.API
dotnet run
```

API runs at `https://localhost:7260` — Swagger UI at `/swagger`.

### Frontend

```bash
cd Truck_Dispatcher/Dispatcher.Frontend
npm install
npm start
```

App runs at `http://localhost:4200`.

### Seeded Test Accounts

| Email | Password | Role |
|---|---|---|
| admin@dispatcher.local | Admin123! | Admin |
| dispatcher@dispatcher.local | Dispatcher123! | Dispatcher |
| driver@dispatcher.local | Driver123! | Driver |
| client@test.com | Client123! | Client |

> **Note:** The data seeder uses rolling relative dates (1–6 days ago) so dashboard queries with 7-day windows always return fresh data. If data looks stale, drop the database and re-run migrations — the `AnyAsync()` guard in the seeder prevents re-seeding over existing data.

---

## User Roles

### Admin
System-wide management: users (CRUD + role assignment), trucks, trailers, inventory, and analytics dashboard with key metrics.

### Dispatcher
The central operational role: reviews and approves client orders, creates shipments with routes, assigns resources (truck, driver, trailer) via the dispatch board, and monitors active deliveries with live tracking.

### Driver
Receives assigned dispatches, updates delivery status in real time, and communicates with the dispatcher. Interface is designed to be minimal and mobile-friendly.

### Client
Self-service portal: browses the product catalog, places orders with delivery details, and tracks order status through to delivery.

---

## Frontend Patterns

The project follows strict conventions documented in `FRONTEND-GUIDE.md`:

- **Three-layer styling:** Tailwind for layout utilities, DaisyUI for component shapes (`card`, `badge`, `btn`, `modal`), SCSS for everything else (keyframes, pseudo-elements, hover systems)
- **Signals-first state:** All local component state uses `signal()` and `computed()`, RxJS is reserved for HTTP calls with `takeUntil(destroyed$)` cleanup
- **Three-state data loading:** Every data section handles loading (skeleton), error, and empty states using `@if` / `@else if` / `@else`
- **Angular 17+ control flow:** `@if` / `@for` exclusively — no `*ngIf` / `*ngFor`
- **Animation separation:** All `@keyframes` and visual polish live in SCSS files, never in templates
- **Component structure:** TypeScript (signals → computed → cleanup → lifecycle → methods) → HTML (control flow) → SCSS (animations)

---

## Team

| Name | 
|---|
| Eldar Sarajlić |
| Haris Šarić | 
| Ali Mustafić  |
| Academic supervisors: Adil Joldić, Azra Smajić |

