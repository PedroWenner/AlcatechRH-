# API Error Standard

All API errors must follow a unified format.

## Structure
{
"success": false,
"message": "Error description",
"errors": []
}

## Validation Errors

Example:
{
"success": false,
"message": "Validation failed",
"errors": [
{
"field": "email",
"message": "Email is required"
}
]
}

## Logging

All errors must be logged.
Sensitive data must never be exposed in responses.