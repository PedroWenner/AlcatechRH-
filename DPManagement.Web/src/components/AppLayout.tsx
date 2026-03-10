import { useEffect, useState, type ReactNode } from 'react';
import { NavLink, Outlet, useNavigate, useLocation } from 'react-router-dom';
import { LayoutDashboard, Users, LogOut, Settings, Calculator, Briefcase, History, Shield, Clock, UserCog, Building2, ChevronDown, ChevronRight, FolderTree, BadgeDollarSign } from 'lucide-react';
import { useAuth } from '../hooks/useAuth';

export function AppLayout() {
  const { user, token, tokenExp, logout, hasPermission } = useAuth();
  const navigate = useNavigate();
  const location = useLocation();
  const [timeLeft, setTimeLeft] = useState<number | null>(null);

  const hasAnyCadastroPermission = hasPermission('Usuarios', 'Visualizar') ||
    hasPermission('Colaboradores', 'Visualizar') ||
    hasPermission('Cargos', 'Visualizar') ||
    hasPermission('Estrutura', 'Visualizar') ||
    hasPermission('CentroCustos', 'Visualizar') ||
    hasPermission('Perfis', 'Visualizar');

  const isCadastrosActive = ['/users', '/employees', '/cargos', '/orgaos', '/centro-custos', '/perfis'].some(path => location.pathname.startsWith(path));

  const hasAnyFolhaPermission = hasPermission('Vinculos', 'Visualizar') || hasPermission('Vinculos', 'Criar') || hasPermission('Rubricas', 'Visualizar');

  const isFolhaActive = ['/vinculos', '/rubricas'].some(path => location.pathname.startsWith(path));

  useEffect(() => {
    if (!token) {
      navigate('/login');
    }
  }, [token, navigate]);

  useEffect(() => {
    if (!tokenExp) {
      setTimeLeft(null);
      return;
    }

    const updateTimer = () => {
      const remaining = Math.max(0, tokenExp - Date.now());
      setTimeLeft(remaining);
    };

    updateTimer();
    const interval = setInterval(updateTimer, 1000);

    return () => clearInterval(interval);
  }, [tokenExp]);

  const handleLogout = () => {
    logout();
    navigate('/login');
  };

  const formatTime = (ms: number) => {
    const totalSeconds = Math.floor(ms / 1000);
    const minutes = Math.floor(totalSeconds / 60);
    const seconds = totalSeconds % 60;
    return `${minutes.toString().padStart(2, '0')}:${seconds.toString().padStart(2, '0')}`;
  };

  if (!token) {
    return null;
  }

  return (
    <div className="flex h-screen bg-gray-50 text-slate-800">
      {/* Sidebar */}
      <aside className="w-64 bg-indigo-900 text-white flex flex-col">
        <div className="p-6 border-b border-indigo-800">
          <h1 className="text-2xl font-bold tracking-tight">DP/Contábil</h1>
          <p className="text-indigo-300 text-sm mt-1">Gestão Integrada</p>
        </div>

        <nav className="flex-1 px-4 py-6 space-y-2">
          {hasPermission('Dashboard', 'Visualizar') && (
            <NavItem to="/" icon={<LayoutDashboard size={20} />} label="Dashboard" />
          )}

          {hasAnyCadastroPermission && (
            <NavGroup icon={<FolderTree size={20} />} label="Cadastros" isActive={isCadastrosActive}>
              {hasPermission('Estrutura', 'Visualizar') && (
                <NavItem to="/orgaos" icon={<Building2 size={20} />} label="Estrutura Organizacional" />
              )}

              {hasPermission('CentroCustos', 'Visualizar') && (
                <NavItem to="/centro-custos" icon={<FolderTree size={20} />} label="Centros de Custos" />
              )}

              {hasPermission('Cargos', 'Visualizar') && (
                <NavItem to="/cargos" icon={<Briefcase size={20} />} label="Cargos" />
              )}

              {hasPermission('Colaboradores', 'Visualizar') && (
                <NavItem to="/employees" icon={<Users size={20} />} label="Colaboradores" />
              )}

              {hasPermission('Usuarios', 'Visualizar') && (
                <NavItem to="/users" icon={<UserCog size={20} />} label="Usuários" />
              )}

              {hasPermission('Perfis', 'Visualizar') && (
                <NavItem to="/perfis" icon={<Shield size={20} />} label="Perfis e Acessos" />
              )}
            </NavGroup>
          )}

          {hasAnyFolhaPermission && (
            <NavGroup icon={<BadgeDollarSign size={20} />} label="Folha" isActive={isFolhaActive}>
              {hasPermission('Vinculos', 'Visualizar') && (
                <NavItem to="/vinculos" icon={<Briefcase size={20} />} label="Vínculos Funcionais" />
              )}
              {hasPermission('Rubricas', 'Visualizar') && (
                <NavItem to="/rubricas" icon={<BadgeDollarSign size={20} />} label="Rubricas / Eventos" />
              )}
            </NavGroup>
          )}

          {hasPermission('Auditoria', 'Visualizar') && (
            <NavItem to="/audit" icon={<History size={20} />} label="Auditoria" />
          )}

          {hasPermission('Folha', 'Visualizar') && (
            <NavItem to="/payroll" icon={<Calculator size={20} />} label="Folha de Pagamento" />
          )}

          {hasPermission('Configuracoes', 'Visualizar') && (
            <NavItem to="/settings" icon={<Settings size={20} />} label="Configurações" />
          )}
        </nav>

        <div className="p-4 border-t border-indigo-800">
          <button
            onClick={handleLogout}
            className="flex items-center w-full px-4 py-2 text-sm text-indigo-200 hover:text-white hover:bg-indigo-800 rounded-md transition-colors"
          >
            <LogOut size={20} className="mr-3" />
            Sair do sistema
          </button>
        </div>
      </aside>

      {/* Main Content */}
      <main className="flex-1 flex flex-col overflow-hidden">
        <header className="bg-white shadow-sm border-b border-gray-200 h-16 flex items-center px-8 justify-between">
          <h2 className="text-lg font-medium text-gray-700">Bem-vindo, {user?.nome || 'Usuário'}</h2>
          <div className="flex items-center space-x-4">
            {timeLeft !== null && (
              <div className="flex items-center text-sm font-medium text-amber-600 bg-amber-50 px-3 py-1.5 rounded-full border border-amber-200">
                <Clock size={16} className="mr-1.5" />
                <span>{formatTime(timeLeft)}</span>
              </div>
            )}
            <div className="h-8 w-8 bg-indigo-100 text-indigo-800 rounded-full flex items-center justify-center font-bold">
              {user?.nome?.substring(0, 2).toUpperCase() || 'US'}
            </div>
          </div>
        </header>
        <div className="flex-1 overflow-auto p-8">
          <Outlet />
        </div>
      </main>
    </div>
  );
}

function NavItem({ to, icon, label }: { to: string; icon: ReactNode; label: string }) {
  return (
    <NavLink
      to={to}
      className={({ isActive }) =>
        `flex items-center px-4 py-3 rounded-md transition-colors ${isActive
          ? 'bg-indigo-700 text-white shadow-sm'
          : 'text-indigo-100 hover:bg-indigo-800 hover:text-white'
        }`
      }
    >
      <span className="mr-3">{icon}</span>
      <span className="font-medium">{label}</span>
    </NavLink>
  );
}

function NavGroup({ icon, label, children, isActive }: { icon: ReactNode; label: string; children: ReactNode; isActive: boolean }) {
  const [isOpen, setIsOpen] = useState(isActive);

  useEffect(() => {
    if (isActive) setIsOpen(true);
  }, [isActive]);

  return (
    <div className="space-y-1">
      <button
        onClick={() => setIsOpen(!isOpen)}
        className={`w-full flex items-center justify-between px-4 py-3 rounded-md transition-colors focus:outline-none ${isActive || isOpen ? 'bg-indigo-800 text-white' : 'text-indigo-100 hover:bg-indigo-800 hover:text-white'
          }`}
      >
        <div className="flex items-center">
          <span className="mr-3">{icon}</span>
          <span className="font-medium">{label}</span>
        </div>
        {isOpen ? <ChevronDown size={18} /> : <ChevronRight size={18} />}
      </button>
      {isOpen && (
        <div className="pl-4 space-y-1 mt-1 border-l border-indigo-700 ml-4 py-1">
          {children}
        </div>
      )}
    </div>
  );
}
