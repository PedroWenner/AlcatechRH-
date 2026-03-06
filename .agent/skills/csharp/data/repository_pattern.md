# REPOSITORY PATTERN

## Purpose

Define how data access must be abstracted using the **Repository Pattern**.

The objective is to:

- isolate data access logic
- protect domain and application layers from persistence details
- improve testability
- allow infrastructure changes without affecting business logic

Repositories act as the **data access boundary** of the application.

---

# Repository Concept

A repository represents a collection-like interface used to access domain entities.

The repository abstracts operations such as:

- retrieving entities
- persisting entities
- updating entities
- removing entities

The application layer interacts with repositories instead of directly accessing the database.

---

# Repository Interface Location

Repository interfaces must be defined in the **Application layer**.

This ensures that the application defines the contracts required for data access.

Example responsibility of the interface:

- define available operations
- describe entity access patterns

The interface must not contain implementation details.

---

# Repository Implementation Location

Repository implementations must exist in the **Infrastructure layer**.

Infrastructure is responsible for interacting with:

- relational databases
- ORM tools
- query builders
- external data sources

Examples include implementations using:

- Entity Framework
- Dapper
- SQL queries

---

# Entity Focus

Repositories must operate on **domain entities**.

Each repository is responsible for managing one main aggregate or entity group.

Examples:

EmployeeRepository  
PayrollRepository  
VacationRepository  

Repositories should represent meaningful domain access points.

---

# Abstraction of Persistence

Repositories must hide persistence details from the rest of the system.

The application and domain layers must not know:

- database technology
- ORM tools
- query language
- table structures

Only the repository implementation interacts with these details.

---

# Data Access Responsibility

Repositories are responsible only for **data persistence and retrieval**.

They must not contain:

- business rules
- domain calculations
- application workflows

Business logic must remain in the Domain layer.

---

# Query Responsibility

Repositories may expose methods that represent common data access patterns.

Examples include:

- retrieving an entity by identifier
- retrieving entities based on specific criteria
- retrieving collections of entities

Complex queries may also be implemented when necessary.

---

# Separation from Domain Logic

Repositories must never implement domain rules.

The repository should only:

- retrieve entities
- persist entities
- remove entities

All business decisions must occur before calling the repository.

---

# Testability

The use of repository interfaces allows:

- mocking during unit tests
- testing application services without database dependency
- isolating persistence concerns

This improves reliability and test coverage.

---

# Architectural Role

Repositories serve as the **boundary between the application and the persistence layer**.

They ensure that domain and application logic remain independent from database technologies.