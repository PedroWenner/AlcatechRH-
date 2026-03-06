import type { ReactNode } from 'react';
import { NavLink, Outlet } from 'react-router-dom';
import { LayoutDashboard, Users, LogOut, Settings } from 'lucide-react';

export function AppLayout() {
  return (
    <div className="flex h-screen bg-gray-50 text-slate-800">
      {/* Sidebar */}
      <aside className="w-64 bg-indigo-900 text-white flex flex-col">
        <div className="p-6 border-b border-indigo-800">
          <h1 className="text-2xl font-bold tracking-tight">DP/Contábil</h1>
          <p className="text-indigo-300 text-sm mt-1">Gestão Integrada</p>
        </div>
        
        <nav className="flex-1 px-4 py-6 space-y-2">
          <NavItem to="/" icon={<LayoutDashboard size={20} />} label="Dashboard" />
          <NavItem to="/employees" icon={<Users size={20} />} label="Colaboradores" />
          <NavItem to="/settings" icon={<Settings size={20} />} label="Configurações" />
        </nav>

        <div className="p-4 border-t border-indigo-800">
          <button className="flex items-center w-full px-4 py-2 text-sm text-indigo-200 hover:text-white hover:bg-indigo-800 rounded-md transition-colors">
            <LogOut size={20} className="mr-3" />
            Sair do sistema
          </button>
        </div>
      </aside>

      {/* Main Content */}
      <main className="flex-1 flex flex-col overflow-hidden">
        <header className="bg-white shadow-sm border-b border-gray-200 h-16 flex items-center px-8 justify-between">
          <h2 className="text-lg font-medium text-gray-700">Bem-vindo, Administrador</h2>
          <div className="h-8 w-8 bg-indigo-100 text-indigo-800 rounded-full flex items-center justify-center font-bold">
            AD
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
        `flex items-center px-4 py-3 rounded-md transition-colors ${
          isActive
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
