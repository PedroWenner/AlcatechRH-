# Feature: Cadastro de Rubricas (Eventos de Folha)

## Descrição
Implementação do cadastro de rubricas (eventos) para o cálculo da folha de pagamento.

## Requisitos
- **Campos**:
  - Código (String/Inteiro)
  - Descrição
  - Tipo (Provento ou Desconto)
  - Incidência IR (Booleano)
  - Incidência Previdência (Booleano)
- **Backend**:
  - Entidade `Rubrica`
  - DTOs, Interface e Serviço
  - Controller API
- **Frontend**:
  - Tela de listagem e cadastro
  - Modal com campos solicitados

## Plano de Ação
1. [x] Criar branch `feature/issue-04-rubricas`
2. [ ] Criar Issue no Git
3. [ ] Definir Entidade e Enums no Backend
4. [ ] Criar Migração e atualizar Banco de Dados
5. [ ] Implementar Service e Controller
6. [ ] Criar tela no Frontend com suporte a CRUD
