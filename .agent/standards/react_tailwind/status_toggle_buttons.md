# Status Toggle Buttons Standard

All Active/Inactive UI status toggles within tables and lists MUST use the explicit `CheckCircle` and `XCircle` semantic icons from the `lucide-react` library.

The coloring scheme should strictly be:
- **Active state to be marked Inactive**: Render the `XCircle` icon with Red styling (`text-red-500 hover:text-red-700`).
- **Inactive state to be marked Active**: Render the `CheckCircle` icon with Green styling (`text-green-500 hover:text-green-700`).

## Example Usage

```tsx
import { CheckCircle, XCircle } from 'lucide-react';

// Inside your Table columns renderer:
<button 
  title={item.ativo ? "Inativar" : "Ativar"} 
  onClick={() => handleToggleStatus(item)} 
  className={item.ativo ? "text-red-500 hover:text-red-700" : "text-green-500 hover:text-green-700"}
>
  {item.ativo ? <XCircle size={18} /> : <CheckCircle size={18} />}
</button>
```

**CRITICAL**: Do NOT use arbitrary icons like `Power` or text labels for this specific action in standard List views. Conform to the `CheckCircle` and `XCircle` unified pattern.
