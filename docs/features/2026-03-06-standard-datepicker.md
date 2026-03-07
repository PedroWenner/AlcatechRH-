# Feature: Standard DatePicker

## Description
Componentizar um seletor de data (DatePicker) estilizado para ser usado em todos os campos de data do sistema, substituindo o input `type="date"` padrão do navegador por uma interface mais moderna e consistente com a identidade visual do projeto.

## Requirements
- Interface amigável e moderna.
- Suporte a formatação de data (DD/MM/AAAA).
- Validação integrada.
- Customização de estilos via Tailwind CSS.
- Compatibilidade com campos de cadastro e filtros.

## Technical Details
- **Frontend**: Criar `src/components/common/DatePicker.tsx`.
- **Biblioteca**: Avaliar uso de `react-datepicker` ou implementação customizada com `lucide-react`.
- **Refactoring**: Atualizar `Employees.tsx` (Cadastro) e `AuditLogs.tsx` (Filtros).
