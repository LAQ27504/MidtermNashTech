// src/hooks/useAuthContext.tsx
import { createContext, useContext, useState, useEffect } from "react";
import { isTokenValid, clearAuth } from "../utils/auth";

interface AuthContextType {
  isAuthenticated: boolean;
  setIsAuthenticated: (v: boolean) => void;
}

const AuthContext = createContext<AuthContextType>(null!);

export const AuthProvider: React.FC<{ children: React.ReactNode }> = ({
  children,
}) => {
  const [isAuthenticated, setIsAuthenticated] = useState<boolean>(
    isTokenValid()
  );

  // Optionally, re-check on mount in case token expired since last page-load:
  useEffect(() => {
    if (!isTokenValid() && isAuthenticated) {
      clearAuth();
      setIsAuthenticated(false);
    }
  }, []);

  return (
    <AuthContext.Provider value={{ isAuthenticated, setIsAuthenticated }}>
      {children}
    </AuthContext.Provider>
  );
};

export const useAuthContext = () => useContext(AuthContext);
