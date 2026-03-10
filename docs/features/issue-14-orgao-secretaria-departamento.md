# Feature: Estrutura Organizacional (Organograma)

**Issue**: Solicitação do usuário para controlar sub-níveis de "Órgão / Secretaria / Departamento".
**Branch**: `feature/orgao-secretaria-departamento`

## Objetivos:
- Criar Entidade unificada de `Orgao` contendo propriedade de Nível (1, 2, 3) e vínculo condicional com ela mesma (`OrgaoPaiId`).
- Criar API CRUD unificada para gestão organizacional, identificando automaticamente a árvore Pai-Filho.
- Adicionar no menu Cadastro a UI Dinâmica que controla de forma inteligente as listagens `Órgão -> Secretaria -> Departamento` num formulário singular.
