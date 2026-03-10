# Resumo da Requisição

**Origem:** Task Router Workflow
**Tipo:** Feature
**Título:** Padronização de Tipos de Vínculos e Formas de Ingresso (Enums)
**Descrição:** Implementar uma estrutura padronizada para representar as opções de "Regime Jurídico / Tipo de Vínculo" e "Forma de Ingresso" para os Cadastros de Servidores. Dada a baixa volatilidade, sugere-se a utilização de Enums diretamente no back-end, com rotas ou DTOs que exponham essas opções para o Front-End montar as caixas de seleção (`selects`).

**Regime Jurídico / Tipo de Vínculo:**
- Estatutário
- Celetista (CLT)
- Cargo Comissionado
- Contrato Temporário (PSS)
- Estagiário
- Jovem Aprendiz

**Forma de Ingresso:**
- Concurso Público
- Processo Seletivo Simplificado
- Livre Nomeação/Exoneração
- Mandato

**Pipeline de Resolução Proposto:**
1. **DEV Specialist**: 
   - Criar os Enums (`RegimeJuridicoEnum` e `FormaIngressoEnum`) no projeto Domain.
   - Criar Controller/Endpoint (ex: `EnumsController`) para fornecer os pares Chave/Valor ao Front-End.
2. **FRONTEND Specialist**: 
   - Criar serviço para consultar os Enums.
   - Construir o UI dropdown / select padrão para uso no form de "Cadastro de Servidor" utilizando esses enums.
3. **QA / Product Owner**: Validação e Homologação.
