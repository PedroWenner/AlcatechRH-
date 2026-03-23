# Bugfix: Erro no carregamento de Centros de Custos

## Descrição
O erro ocorria devido a referências circulares entre as entidades `CentroCusto` e `Orgao` durante a serialização JSON no backend, além de uma divergência entre as propriedades esperadas pelo frontend (`orgaoNome`) e o que era retornado pelo domínio.

## Causa Raiz
1.  **Circularidade**: A entidade `CentroCusto` possui uma referência a `Orgao`, que por sua vez possui uma coleção de `CentroCusto`. O serializador padrão (`System.Text.Json`) falha ao encontrar esses ciclos sem configuração específica.
2.  **Mapeamento**: O frontend esperava `orgaoNome` como uma propriedade direta no objeto, mas recebia a entidade de domínio completa onde o nome estava dentro do objeto `Orgao`.

## Solução
Implementação de DTOs (`CentroCustoDto` e `CentroCustoRequestDto`) na camada de Application para desacoplar a API do domínio e garantir que apenas os dados necessários (e no formato correto) sejam enviados ao frontend.

## Arquivos Alterados
-   `DPManagement.Application/DTOs/CentroCustoDtos.cs` [NOVO]
-   `DPManagement.Application/Interfaces/ICentroCustoService.cs` [MODIFICADO]
-   `DPManagement.Infrastructure/Services/CentroCustoService.cs` [MODIFICADO]
-   `DPManagement.API/Controllers/CentroCustosController.cs` [MODIFICADO]
-   `DPManagement.API/Controllers/CentroCustosController.Dto.cs` [EXCLUÍDO]
