# GEMINI - Antigravity Workspace

> Este arquivo orquestra todos os agentes e suas skills no workspace, garantindo fluxo modular, rastreável e padronizado.

> Todos os nomes devem estar em **português**.

---

## 1. Backend Specialist
- agents/backend/csharp_backend_specialist.md
- skills/backend/clean_architecture.md
- skills/backend/layered_architecture.md
- skills/backend/rest_api_design.md
- skills/backend/request_validation.md
- skills/backend/repository_pattern.md
- skills/backend/unit_of_work.md
- skills/backend/exception_handling.md
- skills/backend/transaction_management.md
- skills/backend/coding_standards.md
- skills/backend/naming_conventions.md
- skills/backend/project_structure.md
- skills/backend/security_guidelines.md
- skills/backend/performance_guidelines.md

---

## 2. Database Specialist
- agents/database/postgres_database_specialist.md
- skills/database/postgres_schema_design.md
- skills/database/postgres_query_optimization.md
- skills/database/postgres_indexing.md
- skills/database/postgres_backup_restore.md
- skills/database/postgres_security.md

---

## 3. Frontend Specialist
- agents/frontend/react_frontend_specialist.md
- skills/frontend/react_components.md
- skills/frontend/tailwind_styling.md
- skills/frontend/error_handling_ui.md
- skills/frontend/state_management.md
- skills/frontend/responsive_design.md

---

## 4. QA Team
- agents/qa/qa_engineer.md
- skills/qa/automation_testing.md
- skills/qa/manual_testing.md
- skills/qa/security_testing.md
- skills/qa/performance_testing.md
- skills/qa/documentation_validation.md

---

## 5. DP Specialists
- agents/dp/folha_pagamento_specialist.md
- agents/dp/e_social_specialist.md
- skills/dp/folha_calculation.md
- skills/dp/folha_integration.md
- skills/dp/folha_validation.md
- skills/dp/benefits_integration.md
- skills/dp/folha_reports.md
- skills/dp/e_social_integration.md
- skills/dp/e_social_validation.md
- skills/dp/e_social_reporting.md
- standards/dp/calculation_rules.md
- standards/dp/validation_rules.md
- standards/dp/naming_conventions.md
- standards/dp/documentation_standards.md

---

## 6. Documentation Agent
- agents/documentation_agent.md
- skills/documentation/technical_docs.md
- skills/documentation/process_docs.md
- skills/documentation/template_standards.md
- skills/documentation/versioning.md

---

## 7. Feature Planner Agent
- agents/feature_planner_agent.md
- skills/feature_planner/feature_breakdown.md
- skills/feature_planner/priority_matrix.md
- skills/feature_planner/dependency_mapping.md
- skills/feature_planner/roadmap_generation.md

---

## 8. GitHub Issue Agent
- agents/github_issue_agent.md
- skills/github/issues_creation.md
- skills/github/issues_update.md
- skills/github/issues_labels.md
- skills/github/issues_reporting.md

---

## 9. Architecture Agent
- agents/architecture_agent.md
- skills/architecture/clean_architecture.md
- skills/architecture/layered_architecture.md
- skills/architecture/rest_api_design.md
- skills/architecture/component_guidelines.md

---

## 10. Product Owner Agent
- agents/product_owner_agent.md
- skills/product_owner/backlog_management.md
- skills/product_owner/acceptance_criteria.md
- skills/product_owner/stakeholder_alignment.md
- skills/product_owner/release_planning.md

---

## 11. Task Router Agent
- agents/task_router_integrated.md
- skills/task_router/task_classification.md
- skills/task_router/agent_matching.md
- skills/task_router/priority_routing.md
- skills/task_router/monitoring.md

---

## 12. Development Workflow Agent
- agents/development_workflow_agent.md
- skills/workflow/documentation.md
- skills/workflow/branch_creation.md
- skills/workflow/task_routing.md
- skills/workflow/conventional_commits.md
- skills/workflow/pull_request.md
- skills/workflow/deploy.md
- skills/workflow/full_flow.md
- standards/workflow/branch_strategy.md

---

## Execution Order

1. **Usuário solicita** → Task Router classifica e cria documentação automaticamente  
2. **Dev Specialist** implementa, testa e faz commits seguindo Conventional Commits  
3. **QA Specialist** valida padrões, segurança, testes e documentação  
4. **PO Agent** valida homologação e aprova merge para produção  
5. **Workflow Agent** garante merge em develop/homologação/main e deploy controlado  

> Todas as tarefas seguem rastreabilidade completa com logs, branches padronizadas e integração entre agentes.