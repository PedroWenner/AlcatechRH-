# C# CODING STANDARDS

## Purpose

Define consistent coding practices for C# backend development.

The objective is to ensure:

- readable code
- maintainable code
- predictable structure
- consistent development style across the codebase

All backend code must follow these standards.

---

# General Principles

Code must prioritize:

- clarity over cleverness
- readability over brevity
- simplicity over complexity

Developers must write code that can be easily understood and maintained by other developers.

---

# Method Design

Methods must be small and focused.

Each method must perform **one clear responsibility**.

Methods should avoid excessive complexity.

Complex logic must be decomposed into smaller methods.

---

# Method Structure

Methods must follow a clear and consistent structure:

1. Input validation
2. Main logic
3. Return result

The flow of execution must be easy to follow.

---

# Exception Safety

All methods that perform operations capable of failing must handle exceptions appropriately.

Critical operations must ensure that exceptions are not silently ignored.

Exception handling must preserve original exception information.

---

# Class Design

Classes must represent a **single responsibility**.

Classes must avoid mixing unrelated concerns.

Large classes must be decomposed into smaller components.

---

# Dependency Management

Dependencies must be injected through constructors.

Classes must not instantiate their own dependencies directly.

This promotes:

- loose coupling
- easier testing
- clearer architecture

---

# Immutability Preference

Whenever possible, objects should be immutable after creation.

Immutable objects reduce unintended side effects and improve system reliability.

---

# Avoid Static State

Static mutable state must be avoided.

Static classes may only be used for:

- utilities
- pure helper functions
- stateless operations

---

# Readability

Code must be written to be easily readable.

Developers must avoid:

- deeply nested logic
- overly complex expressions
- ambiguous naming

Indentation and spacing must be consistent throughout the project.

---

# Magic Values

Hardcoded values must be avoided.

Constants or configuration must be used instead.

Examples include:

- numeric limits
- configuration values
- business constants

---

# Comments

Comments must be used when necessary to explain **why something is done**, not what the code already makes obvious.

Avoid redundant comments.

Prefer expressive code instead.

---

# Code Duplication

Duplicate code must be avoided.

Reusable logic must be extracted into shared components.

---

# Maintainability Goal

The codebase must remain:

- understandable
- predictable
- maintainable
- consistent

Every developer must be able to navigate and modify the system safely.