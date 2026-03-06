# PROJECT STRUCTURE

## Purpose

Define how C# backend projects must be organized to ensure:

- clear separation of responsibilities
- maintainable codebase
- predictable navigation
- scalability of the system

A well-defined project structure improves collaboration and long-term maintainability.

---

# Solution Structure

The solution must be organized into multiple projects representing system layers.

Typical structure:

Application  
Domain  
Infrastructure  
API  

Each project must represent a specific architectural responsibility.

---

# Domain Project

The Domain project contains the **core business model and rules** of the system.

Responsibilities include:

- domain entities
- value objects
- domain services
- domain exceptions
- domain interfaces

The domain must not depend on any other project.

Example folders:

Entities  
ValueObjects  
Services  
Exceptions  
Interfaces  

---

# Application Project

The Application project coordinates **use cases and workflows**.

Responsibilities include:

- application services
- use cases
- DTOs
- repository interfaces
- unit of work interfaces
- FluentValidation validators

The application layer orchestrates domain operations.

Example folders:

Services  
UseCases  
DTOs  
Interfaces  

Dependencies allowed:

Domain

---

# Infrastructure Project

The Infrastructure project contains **technical implementations**.

Responsibilities include:

- repository implementations
- database access
- ORM configurations
- external integrations
- messaging infrastructure
- file services

Example folders:

Persistence  
Repositories  
ExternalServices  
Messaging  
Storage  

Dependencies allowed:

Application  
Domain

---

# API Project

The API project exposes the system through HTTP endpoints.

Responsibilities include:

- controllers
- request models
- response models
- middleware
- filters

The API layer acts as the **system entry point**.

Example folders:

Controllers  
Requests  
Responses  
Middlewares  
Filters  

Dependencies allowed:

Application

---

# Folder Organization Principles

Within each project, folders must group related components.

Folders must represent **logical responsibilities** rather than technical categories when possible.

Avoid overly deep folder hierarchies.

Prefer clear and simple navigation.

---

# File Responsibility

Each file should contain **one primary class** whenever possible.

Files must be named after the main class they contain.

This improves discoverability and maintainability.

---

# Dependency Direction

Project dependencies must follow the architecture direction.

Allowed dependency flow:

API → Application → Domain  
Infrastructure → Application  
Infrastructure → Domain  

The Domain project must never depend on other layers.

---

# Scalability

As the system grows, modules may be separated by domain areas.

Example domain modules:

Employees  
Payroll  
Benefits  
Departments  

Each module may contain its own internal folders.

---

# Structural Goal

The project structure must make it easy for developers to:

- understand system boundaries
- locate code quickly
- maintain architectural discipline
- evolve the system safely