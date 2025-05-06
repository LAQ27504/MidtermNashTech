import React, { useEffect } from "react";
import { Navigate } from "react-router-dom";
import { isTokenValid, clearAuth } from "@/utils/auth";
import { useAuthContext } from "@/context/authContext";
import { useUserContext } from "@/context/userContext";

interface PrivateRouteProps {
  children: React.ReactNode;
}

const SuperUserRoute: React.FC<PrivateRouteProps> = ({ children }) => {
  const { setIsAuthenticated } = useAuthContext();
  const { isSuperUser, isLoading } = useUserContext();

  useEffect(() => {
    if (!isLoading && !isSuperUser) {
      // If loading is done and the user is not a SuperUser, clear auth and redirect
      clearAuth();
      setIsAuthenticated(false);
    }
  }, [isLoading, isSuperUser]);

  if (!isLoading && !isSuperUser) {
    return <Navigate to="/access-denied" replace />;
  }

  if (isLoading) {
    return <div>Loading...</div>; // Optionally show loading state
  }

  if (!isTokenValid()) {
    clearAuth();
    setIsAuthenticated(false);
    return <Navigate to="/login" replace />;
  }

  if (!isSuperUser) {
    return <Navigate to="/access-denied" replace />;
  }

  return <>{children}</>;
};

export default SuperUserRoute;
