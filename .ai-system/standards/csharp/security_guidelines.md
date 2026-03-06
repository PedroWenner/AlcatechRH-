# SECURITY GUIDELINES

## Purpose

Define security principles that must be followed during backend system development.

The objective is to ensure:

- protection of sensitive data
- prevention of common vulnerabilities
- safe system interactions
- secure handling of external inputs

Security must be considered during **all stages of development**.

---

# Secure by Default

All systems must be designed with security as a default principle.

This means:

- deny access by default
- validate all external input
- expose only what is necessary
- avoid unnecessary system privileges

Systems must never assume that incoming data is safe.

---

# Input Validation

All external inputs must be validated.

Sources include:

- HTTP requests
- query parameters
- headers
- uploaded files
- external integrations

Validation must ensure:

- correct data types
- acceptable value ranges
- allowed formats
- safe content

Unvalidated input must never be trusted.

---

# Sensitive Data Protection

Sensitive data must be protected at all times.

Examples include:

- passwords
- authentication tokens
- personal identification numbers
- financial information
- private user data

Sensitive data must never be exposed unnecessarily.

---

# Password Handling

Passwords must never be stored in plain text.

Passwords must be stored using secure hashing algorithms.

The system must never:

- log passwords
- return passwords in responses
- expose password data internally

---

# Data Exposure

APIs must return only the data required for the specific operation.

Unnecessary internal fields must not be exposed.

Examples of data that must not be exposed:

- internal identifiers
- internal system flags
- database metadata
- sensitive internal state

---

# Authorization Enforcement

Every operation that accesses protected resources must verify permissions.

Access must be granted only when the requester has explicit authorization.

Unauthorized access must always be denied.

---

# Error Information Disclosure

Error messages must not expose sensitive system information.

Examples of sensitive information include:

- database queries
- stack traces
- file paths
- internal architecture details

Errors returned to clients must remain generic.

Detailed information must be kept internally.

---

# Secure Configuration

Sensitive configuration values must not be hardcoded.

Examples include:

- database credentials
- API keys
- encryption secrets
- service tokens

These values must be stored securely in configuration systems.

---

# Dependency Security

External libraries and dependencies must be monitored for security vulnerabilities.

Outdated dependencies must be updated regularly.

Only trusted libraries should be used.

---

# Logging Safety

Logs must not contain sensitive data.

Sensitive information must never appear in logs, including:

- passwords
- authentication tokens
- private user data
- payment information

Logs must focus on operational and diagnostic information.

---

# Principle of Least Privilege

Systems must operate using the minimum permissions required.

This applies to:

- database access
- service integrations
- file system access
- infrastructure permissions

Excessive privileges increase security risks.

---

# Security Goal

Security practices must ensure that the system remains protected against:

- unauthorized access
- data exposure
- malicious inputs
- system misuse

Security must be treated as a **continuous responsibility throughout development**.