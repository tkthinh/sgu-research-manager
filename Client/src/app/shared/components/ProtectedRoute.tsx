import { Navigate, Outlet } from "react-router-dom";
import { useAuth } from "../contexts/AuthContext";

interface ProtectedRouteProps {
  allowedRoles?: string[];
  children?: React.ReactNode;
}

const isAuthenticated = (): boolean => {
  const token = localStorage.getItem("token");
  const expiration = localStorage.getItem("expiration");
  if (!token || !expiration) return false;
  return new Date(expiration).getTime() > Date.now();
};

export default function ProtectedRoute({
  allowedRoles,
  children,
}: ProtectedRouteProps) {
  const { user, loading } = useAuth();

  if (loading) {
    return;
  }

  // Redirect if not authenticated
  if (!isAuthenticated() || !user) {
    return <Navigate to="/dang-nhap" replace />;
  }

  // If allowedRoles is provided, check if the user's role is allowed
  if (allowedRoles) {
    if (!allowedRoles.includes(user?.role!)) {
      return <Navigate to="/unauthorized" replace />;
    }
  }

  // Render the child routes if all checks pass
  return children ? <>{children}</> : <Outlet />;
}
