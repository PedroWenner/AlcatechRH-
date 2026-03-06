# FULL DEVELOPMENT WORKFLOW

## Purpose
Automatizar todo o fluxo desde solicitação do usuário até deploy em produção.

## Guidelines

1. **Recepção da Solicitação**
   - Usuário solicita nova feature, bugfix ou hotfix
   - Task Router classifica tipo de solicitação

2. **Documentação Automática**
   - Criar arquivo em pasta correspondente (`docs/features`, `docs/bugfixes`, `docs/hotfixes`)  
   - Preencher template com:
     - História do usuário  
     - Critérios de aceitação  
     - Prioridade  
     - Notas adicionais

3. **Branch Git**
   - Criar branch com padrão:
     - `feature/issue-XX-descricao-curta`  
     - `bugfix/issue-XX-descricao-curta`  
     - `hotfix/issue-XX-descricao-curta`  
   - Base:
     - Feature/bugfix → `develop`  
     - Hotfix → `main`

4. **Envio para Especialista**
   - Dev → implementação, testes e commits usando **Conventional Commits**  
   - QA → valida padrões, testes, segurança e documentação  
   - PO → valida homologação e aprova merge para produção

5. **Pull Request**
   - Abrir PR contra `develop` com template preenchido  
   - QA revisa e aprova  
   - Merge em `develop` após aprovação

6. **Deploy e Homologação**
   - Deploy automático em homologação após merge em `develop`  
   - PO testa e valida  
   - Merge final para `main` (produção) autorizado pelo PO

7. **Monitoramento e Alertas**
   - Task Router acompanha cada etapa  
   - Alertas automáticos para falhas ou bloqueios  
   - Logs de commit, PR, merge e deploy mantidos para auditoria