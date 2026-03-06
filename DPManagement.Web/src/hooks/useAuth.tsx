import { createContext, useContext, useState, useEffect, type ReactNode } from 'react';
import { jwtDecode } from 'jwt-decode';

interface User {
  id: string;
  nome: string;
  email: string;
  perfil: string;
  permissions: string[];
}

interface AuthContextType {
  user: User | null;
  token: string | null;
  login: (token: string, userData: any) => void;
  logout: () => void;
  hasPermission: (modulo: string, acao: string) => boolean;
  isAuthorized: (modulo: string) => boolean;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export function AuthProvider({ children }: { children: ReactNode }) {
  const [user, setUser] = useState<User | null>(null);
  const [token, setToken] = useState<string | null>(localStorage.getItem('token'));

  useEffect(() => {
    if (token) {
      try {
        const decoded: any = jwtDecode(token);
        const permissions = Array.isArray(decoded.Permission) 
          ? decoded.Permission 
          : decoded.Permission ? [decoded.Permission] : [];
        
        // Simulação básica de dados do usuário vindos do token ou storage
        const storedUser = localStorage.getItem('user');
        if (storedUser) {
            const parsed = JSON.parse(storedUser);
            setUser({ ...parsed, permissions });
        }
      } catch (error) {
        logout();
      }
    }
  }, [token]);

  const login = (newToken: string, userData: any) => {
    localStorage.setItem('token', newToken);
    localStorage.setItem('user', JSON.stringify(userData));
    setToken(newToken);
    
    const decoded: any = jwtDecode(newToken);
    const permissions = Array.isArray(decoded.Permission) 
      ? decoded.Permission 
      : decoded.Permission ? [decoded.Permission] : [];
      
    setUser({ ...userData, permissions });
  };

  const logout = () => {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    setToken(null);
    setUser(null);
  };

  const hasPermission = (modulo: string, acao: string) => {
    if (user?.perfil === 'Admin') return true;
    return user?.permissions.includes(`${modulo}:${acao}`) ?? false;
  };

  const isAuthorized = (modulo: string) => {
    if (user?.perfil === 'Admin') return true;
    return user?.permissions.some(p => p.startsWith(`${modulo}:`)) ?? false;
  };

  return (
    <AuthContext.Provider value={{ user, token, login, logout, hasPermission, isAuthorized }}>
      {children}
    </AuthContext.Provider>
  );
}

export function useAuth() {
  const context = useContext(AuthContext);
  if (context === undefined) {
    throw new Error('useAuth must be used within an AuthProvider');
  }
  return context;
}
