import React from "react";
import { Navigate } from "react-router-dom";
import { isTokenValid, clearAuth } from "@/utils/auth";
import { useAuthContext } from "@/context/authContext";
import { useUserContext } from "@/context/userContext";

interface PrivateRouteProps {
  children: React.ReactNode;
}

const NormalUserRoute: React.FC<PrivateRouteProps> = ({ children }) => {
  const { isAuthenticated, setIsAuthenticated } = useAuthContext();
  const { isSuperUser, isLoading } = useUserContext();

  if (isLoading) {
    return <div>Loading...</div>;
  }

  if (!isTokenValid() || !isAuthenticated) {
    clearAuth();
    setIsAuthenticated(false);
    return <Navigate to="/login" replace />;
  }

  if (isSuperUser) {
    return <Navigate to="/superuser" replace />;
  }

  return <>{children}</>;
};

export default NormalUserRoute;
