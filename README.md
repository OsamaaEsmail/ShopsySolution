# 🛒 Shopsy — Modular Monolith eCommerce API

A production-ready **modular monolith** eCommerce REST API built with **.NET 9**, following **Clean Architecture** and **CQRS** patterns. Designed to demonstrate enterprise-level backend development practices.

## 🏗️ Architecture

```
ShopsySolution/
├── Shopsy.API/                    # Composition Root — Controllers, Middleware, DI
├── Shopsy.BuildingBlocks/         # Shared abstractions — Result, CQRS, Pagination
├── Shopsy.Tests/                  # Unit Tests — xUnit, Moq, FluentAssertions
│
├── User.Module/                   # Authentication, Authorization, Roles, Permissions
│   ├── User.Domain/
│   ├── User.Application/
│   └── User.Infrastructure/
│
├── Catalog.Module/                # Products, Categories, SubCategories, Vendors, Stocks
│   ├── Catalog.Domain/
│   ├── Catalog.Application/
│   └── Catalog.Infrastructure/
│
├── Cart.Module/                   # Shopping Cart management
│   ├── Cart.Domain/
│   ├── Cart.Application/
│   └── Cart.Infrastructure/
│
├── Order.Module/                  # Orders, Payments, Addresses, Stock Deduction
│   ├── Order.Domain/
│   ├── Order.Application/
│   └── Order.Infrastructure/
│
└── Sales.Module/                  # Sales campaigns with product discounts
    ├── Sales.Domain/
    ├── Sales.Application/
    └── Sales.Infrastructure/
```

Each module follows **Clean Architecture** with Domain → Application → Infrastructure layers, has its own **DbContext** with a separate database schema, and communicates through shared IDs with cross-module foreign key constraints.


## ✨ Features

### Core
- **Modular Monolith** — 5 independent modules sharing a single database with separate schemas
- **Clean Architecture** — Domain, Application, Infrastructure layers per module
- **CQRS** — Command/Query Separation with MediatR
- **Result Pattern** — No exceptions for business logic; typed `Result<T>` responses
- **Pagination** — `PaginatedList<T>` on all list endpoints

### Authentication & Authorization
- **JWT Bearer** authentication with refresh tokens
- **Permission-based** authorization via custom `[HasPermission]` attribute
- **Role management** with granular permission assignment
- **Email confirmation** and password reset flows

### Modules
- **Catalog** — Full CRUD for Products, Categories, SubCategories, Vendors, Stocks with seed data
- **Cart** — Add/Remove/Clear items, auto-create basket, quantity merge for same product+color+size
- **Orders** — Checkout with billing/shipping addresses, automatic stock deduction via raw SQL, order status workflow (Pending → Confirmed → Shipped → Delivered / Cancelled)
- **Payments** — Payment creation linked to orders with duplicate prevention
- **Sales** — Time-based sales campaigns with product discount prices

### Cross-Cutting Concerns
- **Serilog** — Structured logging with Console + File sinks, daily rotation
- **Rate Limiting** — 100 req/min general, 10 req/min for auth endpoints
- **Health Checks** — SQL Server + API health monitoring at `/health`
- **Global Exception Handling** — ProblemDetails responses for all unhandled exceptions
- **CORS** — Environment-based policies (AllowAll for dev, specific origins for prod)
- **User Activity Logging** — Middleware tracking user actions via JWT claims
- **FluentValidation** — Request validation on all commands
- **Mapster** — Object mapping with explicit configuration per module

### Testing
- **74 Unit Tests** — xUnit + Moq + FluentAssertions + InMemory Database
- Tests cover all service layers: Product, Category, SubCategory, Vendor, Stock, Cart, Order, Payment, Sale

## 🛠️ Tech Stack

| Layer | Technology |
|-------|-----------|
| Framework | .NET 9, ASP.NET Core |
| Database | SQL Server (Docker) |
| ORM | Entity Framework Core 9 |
| Auth | ASP.NET Identity + JWT Bearer |
| CQRS | MediatR |
| Validation | FluentValidation |
| Mapping | Mapster |
| Logging | Serilog (Console + File) |
| Testing | xUnit, Moq, FluentAssertions |
| API Docs | Swagger / OpenAPI with versioning |

## 📡 API Endpoints

### Auth
| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/Auth/login` | Login |
| POST | `/api/Auth/register` | Register |
| POST | `/api/Auth/refresh` | Refresh token |

### Products
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/Products?pageNumber=1&pageSize=10` | Get all (paginated) |
| GET | `/api/Products/{id}` | Get by ID |
| POST | `/api/Products` | Create |
| PUT | `/api/Products/{id}` | Update |
| DELETE | `/api/Products/{id}` | Delete |

### Categories
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/Categories` | Get all (paginated) |
| GET | `/api/Categories/{id}` | Get by ID |
| POST | `/api/Categories` | Create |
| PUT | `/api/Categories/{id}` | Update |
| DELETE | `/api/Categories/{id}` | Delete |

### Cart
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/Carts/my-cart` | Get current user's cart |
| POST | `/api/Carts/add` | Add item to cart |
| DELETE | `/api/Carts/remove/{cartItemId}` | Remove item |
| DELETE | `/api/Carts/clear` | Clear cart |

### Orders
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/Orders/my-orders` | Get current user's orders |
| GET | `/api/Orders/{id}` | Get order details |
| POST | `/api/Orders` | Create order (auto deducts stock) |
| PUT | `/api/Orders/{id}/status` | Update order status |
| DELETE | `/api/Orders/{id}` | Delete order |

### Payments
| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/Payments` | Create payment for order |

### Sales
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/Sales/active` | Get active sales (paginated) |
| GET | `/api/Sales/{id}` | Get by ID |
| POST | `/api/Sales` | Create |
| PUT | `/api/Sales/{id}` | Update |
| DELETE | `/api/Sales/{id}` | Delete |

### Health
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/health` | Full health check |
| GET | `/health/database` | Database health check |

## 🗄️ Database Schema

Each module owns its schema:

| Schema | Tables |
|--------|--------|
| `users` | AspNetUsers, AspNetRoles, RefreshTokens, ... |
| `catalog` | Products, Categories, SubCategories, Vendors, Stocks, ProductImages |
| `cart` | Carts, CartItems |
| `order` | Orders, OrderItems, BillingAddresses, ShippingAddresses, Payments |
| `sales` | Sales, SaleItems |


## 🧪 Running Tests

```bash
dotnet test
```

```
Passed! - Failed: 0, Passed: 74, Skipped: 0, Total: 74
```

## 📁 Key Design Decisions

- **Modular Monolith over Microservices** — Simpler deployment while maintaining module boundaries; can be split into microservices later
- **Shared Database, Separate Schemas** — Each module has its own DbContext and schema, cross-module FKs via raw SQL migrations
- **Service Pattern over Repository** — Services inject DbContext directly, keeping the code simple without unnecessary abstraction layers
- **Permission-based Auth** — Granular permissions (e.g., `products:read`, `orders:add`) assigned to roles, checked via custom `[HasPermission]` attribute
- **Result Pattern** — All service methods return `Result<T>` instead of throwing exceptions for business logic failures
- **Auditable Entities** — `CreatedById`, `CreatedByName`, `UpdatedById`, `UpdatedByName` auto-populated from JWT claims via `SaveChangesAsync` override

