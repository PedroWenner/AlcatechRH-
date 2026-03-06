# CLEAN ARCHITECTURE

## Purpose

Define how backend systems must be structured following **Clean Architecture principles**.

The goal is to ensure:

- clear separation of concerns
- maintainable codebase
- independent business rules
- testable domain logic
- replaceable infrastructure

---

# Architectural Layers

All backend systems must be organized into the following layers:

1. Domain
2. Application
3. Infrastructure
4. API

Dependencies must always flow **from outer layers to inner layers**.
API
↓
Application
↓
Domain

Infrastructure is used by Application through abstractions.

---

# Domain Layer

The **Domain layer** contains the core business logic of the system.

It must contain:

- Entities
- Value Objects
- Domain Services
- Business Rules
- Domain Exceptions
- Interfaces required by the domain

The domain layer must **not depend on any external framework or library**.

Forbidden dependencies:

- ASP.NET
- Entity Framework
- Database drivers
- External services
- Logging frameworks

The domain must be **pure C# code**.

---

# Application Layer

The **Application layer** orchestrates use cases.

Responsibilities:

- implement application services
- coordinate domain operations
- handle use cases
- call repositories through interfaces
- manage transactions through abstractions

The application layer may contain:

- Application Services
- Use Cases
- DTOs
- Interfaces for repositories
- Interfaces for external services

The application layer depends only on:

- Domain layer

It must not depend on infrastructure implementations.

---

# Infrastructure Layer

The **Infrastructure layer** provides implementations for external dependencies.

Responsibilities:

- database access
- repository implementations
- integrations with external services
- messaging systems
- file storage
- caching

Examples:

- Entity Framework repositories
- Dapper queries
- HTTP service integrations
- message brokers

Infrastructure depends on:

- Application layer
- Domain layer

---

# API Layer

The **API layer** exposes the application to external clients.

Responsibilities:

- receive requests
- map requests to application services
- return responses

The API layer must not contain business logic.

The API layer depends on:

- Application layer

---

# Dependency Rule

The most important rule of Clean Architecture:

**Inner layers must never depend on outer layers.**

Correct dependency direction:

API → Application → Domain
Infrastructure → Application → Domain

Incorrect dependency examples:

Domain → Infrastructure  
Domain → API  
Application → API  

These dependencies are forbidden.

---

# Dependency Inversion

Infrastructure implementations must be injected through interfaces.

Example:

Application layer defines:


IEmployeeRepository


Infrastructure layer implements:


EmployeeRepository : IEmployeeRepository


The application interacts only with the interface.

---

# Domain Isolation

All critical business rules must live in the **Domain layer**.

Business logic must never be placed inside:

- Controllers
- Repositories
- Infrastructure services
- API layer

---

# Benefits

Following this architecture provides:

- testable business logic
- infrastructure independence
- maintainable code
- easier refactoring
- scalable systems