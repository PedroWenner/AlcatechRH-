# API Response Standard

All APIs must follow the same response structure.

This guarantees consistency across the system.

## Success Response
{
"success": true,
"message": "Operation completed successfully",
"data": {},
"errors": null
}

## Error Response
{
"success": false,
"message": "Validation error",
"data": null,
"errors": [
{
"field": "cpf",
"message": "CPF is invalid"
}
]
}

## Field Definitions

success  
Boolean indicating if the request succeeded.

message  
Human readable message.

data  
Payload returned by the API.

errors  
List of validation errors.

## HTTP Status Usage

200 → Success  
201 → Resource created  
400 → Validation error  
401 → Unauthorized  
403 → Forbidden  
404 → Not found  
500 → Internal error