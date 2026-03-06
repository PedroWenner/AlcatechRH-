# UNIT OF WORK

## Purpose

Define how multiple repository operations must be coordinated using the **Unit of Work pattern**.

The objective is to ensure:

- transactional consistency
- atomic operations
- coordinated persistence of changes
- control over database transactions

The Unit of Work manages a set of repository operations as **a single logical transaction**.

---

# Concept

The Unit of Work pattern tracks changes made to entities during a business operation and ensures that all changes are committed to the database as a single unit.

This means that:

- multiple repository operations can be grouped
- changes are committed together
- failures can trigger a rollback

This guarantees data consistency.

---

# Unit of Work Responsibility

The Unit of Work is responsible for:

- coordinating repository operations
- managing transaction boundaries
- committing changes to the persistence layer
- rolling back changes in case of failure

It acts as a **transaction manager for the application layer**.

---

# Interface Location

The Unit of Work interface must be defined in the **Application layer**.

The interface describes:

- the repositories available in the current context
- the commit operation

The interface must not include persistence implementation details.

---

# Implementation Location

The Unit of Work implementation must exist in the **Infrastructure layer**.

Infrastructure implementations handle:

- database transactions
- ORM session management
- connection lifecycle

The implementation coordinates repository instances and transaction control.

---

# Repository Coordination

Repositories used within the same operation must share the same Unit of Work instance.

This ensures that:

- all repository operations participate in the same transaction
- changes are committed together

Repositories must not manage transactions independently.

---

# Transaction Boundary

A Unit of Work represents a **single transaction boundary**.

Typical flow:

1. Begin operation
2. Perform multiple repository operations
3. Commit changes

If any operation fails before commit, the transaction must be rolled back.

---

# Commit Operation

The commit operation persists all tracked changes to the database.

Commit should occur only after:

- all domain operations are completed
- application workflow is finished
- validation has succeeded

The application layer decides when the commit occurs.

---

# Consistency Guarantee

Using Unit of Work ensures that the system avoids partial updates.

Example scenario:

- create employee
- assign department
- create payroll record

All operations must succeed together.

If one fails, none of the changes should persist.

---

# Architectural Role

The Unit of Work serves as the **transaction coordination mechanism** between the application layer and the persistence layer.

It ensures that complex operations involving multiple repositories remain consistent.