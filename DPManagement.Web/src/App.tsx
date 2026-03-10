import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import { AuthProvider } from './hooks/useAuth';
import { AppLayout } from './components/AppLayout';
import Login from './pages/Login';
import Dashboard from './pages/Dashboard';
import Employees from './pages/Employees';
import Cargos from './pages/Cargos';
import Orgaos from './pages/Orgaos';
import AuditLogs from './pages/AuditLogs';
import Perfis from './pages/Perfis';
import Users from './pages/Users';
import CentroCustos from './pages/CentroCustos';
import Vinculos from './pages/Vinculos';
import { Toaster } from 'react-hot-toast';

function App() {
  return (
    <AuthProvider>
      <Toaster position="top-right" />
      <BrowserRouter>
        <Routes>
          <Route path="/login" element={<Login />} />

          {/* Protected Routes Wrapper */}
          <Route path="/" element={<AppLayout />}>
            <Route index element={<Dashboard />} />
            <Route path="employees" element={<Employees />} />
            <Route path="cargos" element={<Cargos />} />
            <Route path="orgaos" element={<Orgaos />} />
            <Route path="centro-custos" element={<CentroCustos />} />
            <Route path="vinculos" element={<Vinculos />} />
            <Route path="perfis" element={<Perfis />} />
            <Route path="users" element={<Users />} />
            <Route path="audit" element={<AuditLogs />} />
            {/* Missing route matching "settings" for now */}
            <Route path="settings" element={<div className="p-4">Configurações...</div>} />
          </Route>

          <Route path="*" element={<Navigate to="/" replace />} />
        </Routes>
      </BrowserRouter>
    </AuthProvider>
  );
}

export default App;
