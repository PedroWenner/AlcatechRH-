# Padrão de Resposta API (.NET)

Para garantir consistência e facilitar o tratamento de erros no frontend, todas as APIs deste projeto devem seguir o padrão `OperationResult`.

## 1. Estrutura do Resultado (Backend)

Localização: `DPManagement.Application.Common.OperationResult.cs`

Toda resposta deve ser envolvida pela classe `OperationResult` ou `OperationResult<T>`.

```csharp
public class OperationResult
{
    public bool Success { get; set; }        // Indica se a operação ocorreu como esperado
    public string Message { get; set; }      // Mensagem de sucesso ou erro amigável
    public List<string> Errors { get; set; } // Lista de erros detalhados (opcional)
}
```

## 2. Implementação nos Serviços

Os serviços **não devem lançar exceções** para erros de validação de negócio (ex: código duplicado, registro não encontrado). Em vez disso, devem retornar um `OperationResult.Failure`.

Exemplo:
```csharp
public async Task<OperationResult> UpdateAsync(Guid id, Dto dto)
{
    var entity = await _context.Entities.FindAsync(id);
    if (entity == null) return OperationResult.Failure("Registro não encontrado.");
    
    // ... lógica ...
    
    return OperationResult.Ok("Atualizado com sucesso.");
}
```

## 3. Implementação nos Controllers

Os controllers devem verificar a propriedade `Success` e retornar o status HTTP adequado.

- `Success == true`: Retornar `Ok(result)` ou `CreatedAtAction(...)`.
- `Success == false`: Retornar `BadRequest(result)` ou `NotFound(result)`.

```csharp
[HttpPut("{id}")]
public async Task<IActionResult> Update(Guid id, [FromBody] Dto dto)
{
    var result = await _service.UpdateAsync(id, dto);
    return result.Success ? Ok(result) : BadRequest(result);
}
```

## 4. Tratamento Global de Erros

O middleware `GlobalExceptionHandler` captura exceções não tratadas e as formata automaticamente no padrão `OperationResult` com status 500, garantindo que o frontend sempre receba um JSON válido.

## 5. Frontend (React)

O frontend deve ler a propriedade `success` da resposta para decidir como exibir os alertas.

```typescript
const response = await api.get('/endpoint');
const resData = response.data;

if (resData.success) {
  // sucesso: usa resData.data
} else {
  // erro: exibe resData.message ou resData.errors
}
```
