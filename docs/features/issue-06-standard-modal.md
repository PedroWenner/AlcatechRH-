# Feature: Standard Modal Component

## Description
Criar um componente de Modal reutilizável e padronizado para ser usado em todo o sistema, permitindo consistência visual e facilidade de manutenção.

## Requirements
- Uso de `createPortal` para renderizar fora da hierarquia do DOM principal.
- Controle de visibilidade via prop `isOpen`.
- Função de fechar via prop `onClose` (disparada pelo botão fechar, backdrop ou tecla ESC).
- Suporte a diferentes tamanhos (sm, md, lg, xl, 4xl).
- Cabeçalho padronizado com título e botão de fechar.
- Rodapé opcional para botões de ação.

## Technical Details
- **Frontend**: Criar `src/components/common/Modal.tsx`.
- **Refactoring**: Atualizar `Employees.tsx` e `Cargos.tsx`.
- **Styling**: Utilizar Tailwind CSS para garantir responsividade e harmonia com o design atual.
