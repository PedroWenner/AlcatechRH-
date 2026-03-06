# BRANCH STRATEGY

## Purpose
Padronizar criação e gerenciamento de branches para todas as solicitações.

## Guidelines
- **main** → produção (protegida, só recebe merge de homologação)  
- **homologação** → recebe merge de `develop` após revisão  
- **develop** → desenvolvimento integrado (recebe merge de `feature/*` e `bugfix/*`)  
- **feature/** → `feature/issue-XX-descricao-curta`  
- **bugfix/** → `bugfix/issue-XX-descricao-curta`  
- **hotfix/** → `hotfix/issue-XX-descricao-curta` (a partir de `main`)  
- Usar Conventional Commits referenciando issue em cada commit  
- PRs devem sempre ser revisados pelo QA antes do merge