# REQUEST VALIDATION

## Purpose

Define how incoming API requests must be validated before reaching application logic.

The objective is to ensure:

- input integrity
- predictable system behavior
- protection against invalid or malformed data

Request validation must occur **at the API boundary**.

---

# Validation Scope

Request validation is responsible for validating:

- request structure
- required fields
- basic data formats
- data type correctness

Request validation must **not contain business rules**.

Business rules belong to the Domain layer.

---

# Validation Location

Request validation must occur in the **API layer**.

Common validation points include:

- request models
- DTO validation
- model binding validation
- validation filters

The API layer must prevent invalid requests from reaching the application layer.

---

# Required Fields

All required fields must be validated.

If a required field is missing or empty, the request must be rejected.

Examples of required validations:

- missing identifiers
- empty required strings
- null objects

---

# Data Type Validation

Incoming data must match the expected type.

Examples:

- numeric fields must contain numbers
- dates must follow a valid date format
- boolean fields must contain valid boolean values

Invalid types must result in request rejection.

---

# Format Validation

Certain fields require format validation.

Examples include:

- email addresses
- phone numbers
- document identifiers
- date formats

Only structural validation should occur here.

Semantic meaning must be validated later in the domain.

---

# Boundary Protection

Request validation protects the system boundary.

No request should enter the application layer if:

- required data is missing
- structure is invalid
- types are incorrect
- formats are invalid

This prevents unnecessary execution of application logic.

---

# Validation Responsibility Separation

Validation must follow this separation:

Request Validation  
Ensures request structure and basic correctness.

Domain Validation  
Ensures business rules and domain consistency.

Database Validation  
Ensures persistence constraints.

Each layer must validate only what belongs to its responsibility.

---

# Validation Consistency

Validation rules must be applied consistently across all endpoints.

Different endpoints receiving the same request model must apply the same validation rules.

This prevents inconsistent system behavior.

---

# Goal

The purpose of request validation is to ensure that **only structurally valid requests enter the system**, preserving system stability and reliability.