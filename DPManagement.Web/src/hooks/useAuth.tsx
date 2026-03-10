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
  tokenExp: number | null;
  login: (token: string, userData: any) => void;
  logout: () => void;
  hasPermission: (modulo: string, acao: string) => boolean;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export function AuthProvider({ children }: { children: ReactNode }) {
  const [user, setUser] = useState<User | null>(null);
  const [token, setToken] = useState<string | null>(localStorage.getItem('token'));
  const [tokenExp, setTokenExp] = useState<number | null>(null);

  useEffect(() => {
    if (token) {
      try {
        const decoded: any = jwtDecode(token);
        const permissions = Array.isArray(decoded.Permission)
          ? decoded.Permission
          : decoded.Permission ? [decoded.Permission] : [];

        if (decoded.exp) {
          setTokenExp(decoded.exp * 1000);
        }

        // Simulação básica de dados do usuário vindos do token ou storage
        const storedUser = localStorage.getItem('user');
        if (storedUser) {
          const parsed = JSON.parse(storedUser);
          setUser({ ...parsed, permissions });
        }
      } catch (error) {
        logout();
      }
    } else {
      setTokenExp(null);
    }
  }, [token]);

  useEffect(() => {
    if (!tokenExp) return;

    const checkExpiration = () => {
      // Usamos uma margem de segurança caso precise (ex: - 5000) mas aqui faremos logout exato
      if (Date.now() >= tokenExp) {
        logout();
      }
    };

    const interval = setInterval(checkExpiration, 1000);
    checkExpiration();

    return () => clearInterval(interval);
  }, [tokenExp]);

  const login = (newToken: string, userData: any) => {
    localStorage.setItem('token', newToken);
    localStorage.setItem('user', JSON.stringify(userData));
    setToken(newToken);

    const decoded: any = jwtDecode(newToken);
    const permissions = Array.isArray(decoded.Permission)
      ? decoded.Permission
      : decoded.Permission ? [decoded.Permission] : [];

    if (decoded.exp) {
      setTokenExp(decoded.exp * 1000);
    }

    setUser({ ...userData, permissions });
  };

  const logout = () => {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    setToken(null);
    setTokenExp(null);
    setUser(null);
  };

  const hasPermission = (modulo: string, acao: string) => {
    if (user?.perfil === 'Admin') return true;
    return user?.permissions.includes(`${modulo}:${acao}`) ?? false;
  };

  return (
    <AuthContext.Provider value={{ user, token, tokenExp, login, logout, hasPermission }}>
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
