# REST API DESIGN

## Purpose

Define how HTTP APIs must be designed following **REST architectural principles**.

The objective is to ensure APIs that are:

- consistent
- predictable
- easy to consume
- scalable
- maintainable

---

# REST Principles

All APIs must follow core REST principles.

A REST API must be:

Stateless  
Resource-based  
Uniform  
Cacheable when applicable  

Each request must contain all information required for processing.

---

# Resource-Oriented Design

APIs must be designed around **resources**, not actions.

A resource represents a domain entity or concept.

Examples:

employees  
payrolls  
vacations  
departments  

Endpoints must represent resources.

Correct example:

/employees

Incorrect example:

/getEmployees

---

# HTTP Methods

Each operation must use the appropriate HTTP method.

GET  
Used to retrieve resources.

POST  
Used to create new resources.

PUT  
Used to replace an existing resource.

PATCH  
Used to partially update a resource.

DELETE  
Used to remove a resource.

---

# Endpoint Structure

Endpoints must follow a consistent structure.

Example:

/employees  
/employees/{id}  
/employees/{id}/vacations  

Plural nouns must be used for resource names.

Avoid verbs in URLs.

---

# Resource Hierarchies

When resources belong to another resource, hierarchical routes may be used.

Example:

/employees/{id}/vacations

This indicates vacations belonging to a specific employee.

Avoid deep nesting beyond two levels.

---

# Query Parameters

Query parameters must be used for:

- filtering
- sorting
- pagination

Example:

/employees?department=hr  
/employees?page=1&pageSize=20  
/employees?sort=name

Query parameters must not change the resource identity.

---

# Pagination

Endpoints returning collections must support pagination.

Large datasets must never be returned entirely by default.

Pagination parameters may include:

page  
pageSize  

---

# Resource Identification

Each resource must have a unique identifier.

Example:

/employees/{id}

The identifier must represent a single unique resource.

---

# Idempotency

Certain HTTP operations must be idempotent.

Idempotent operations produce the same result when executed multiple times.

PUT  
DELETE  

POST operations are generally not idempotent.

---

# Response Consistency

Responses must be consistent across the entire API.

Endpoints that represent collections must return collection structures.

Endpoints representing single resources must return single objects.

---

# Versioning

APIs must support versioning to avoid breaking clients.

Versioning can be implemented using:

URL versioning

Example:

/api/v1/employees

---

# Naming Conventions

Resource names must:

- use lowercase letters
- use hyphens for multi-word resources
- avoid abbreviations when possible

Example:

employee-benefits  
payroll-records  

Avoid:

emp  
getEmployeeData  

---

# API Design Goal

The design must prioritize:

- clarity
- consistency
- predictability

A developer consuming the API must be able to **understand and use it without documentation whenever possible**.