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

## 3. Implementação nos Controllers

Os controllers devem verificar a propriedade `Success` e retornar o status HTTP adequado (OK ou BadRequest/NotFound).

## 4. Tratamento Global de Erros

O middleware `GlobalExceptionHandler` captura exceções não tratadas e as formata automaticamente no padrão `OperationResult` com status 500.

## 5. Frontend (React)

O frontend deve ler a propriedade `success` da resposta para decidir como exibir os alertas.
