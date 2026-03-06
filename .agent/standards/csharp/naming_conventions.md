# NAMING CONVENTIONS

Todos os nomes devem estar em **português**.

## Purpose

Define naming conventions used in C# backend development.

The objective is to ensure:

- clear communication through code
- consistency across the codebase
- easy navigation and understanding of the system

All identifiers must follow consistent naming rules.

---

# General Principles

Names must be:

- descriptive
- meaningful
- easy to read
- easy to pronounce

Abbreviations should be avoided unless they are widely known.

Names must clearly express the purpose of the element.

---

# Class Names

Class names must use **PascalCase**.

Class names must represent a **noun or concept**.

Examples:

Employee  
PayrollService  
VacationRequest  
EmployeeRepository  

Class names must be singular.

Avoid generic names such as:

Manager  
Helper  
Utils  

---

# Interface Names

Interfaces must begin with the letter **I** and use PascalCase.

Examples:

IEmployeeRepository  
IPayrollService  
IUnitOfWork  

The interface name must clearly represent the contract.

---

# Method Names

Method names must use **PascalCase**.

Methods must represent **actions or behaviors**.

Examples:

CreateEmployee  
CalculatePayroll  
ApproveVacation  
GetEmployeeById  

Methods must begin with a verb.

---

# Variable Names

Variables must use **camelCase**.

Names must clearly describe the stored value.

Examples:

employeeId  
payrollAmount  
vacationStartDate  

Single-letter variable names should be avoided except for very small scopes.

---

# Parameter Names

Method parameters must use **camelCase**.

Examples:

employeeId  
departmentId  
startDate  

Parameter names must describe the expected value.

---

# Property Names

Properties must use **PascalCase**.

Examples:

EmployeeId  
FirstName  
LastName  
HireDate  

Properties must represent attributes of the object.

---

# Private Fields

Private fields must use **camelCase with a leading underscore**.

Examples:

_employeeRepository  
_payrollService  
_unitOfWork  

This clearly differentiates fields from local variables and parameters.

---

# Constants

Constants must use **PascalCase**.

Examples:

MaxRetryAttempts  
DefaultPageSize  
MaximumVacationDays  

Constants must represent fixed values that do not change during execution.

---

# Enum Names

Enum types must use **PascalCase**.

Enum members must also use **PascalCase**.

Example:

Enum:

EmployeeStatus

Members:

Active  
Inactive  
Suspended  

Enum names must represent a category of values.

---

# Boolean Naming

Boolean variables and properties must clearly express a true/false state.

Examples:

IsActive  
HasPermission  
CanApprove  

Boolean names should begin with:

Is  
Has  
Can  
Should  

---

# File Names

File names must match the primary class contained in the file.

Examples:

Employee.cs  
EmployeeRepository.cs  
PayrollService.cs  

Each file should contain one primary class whenever possible.

---

# Naming Goal

Consistent naming must make the codebase:

- self-explanatory
- easy to read
- easy to navigate
- easy to maintain