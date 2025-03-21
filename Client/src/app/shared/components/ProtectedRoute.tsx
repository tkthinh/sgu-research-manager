import { Navigate, Outlet } from "react-router-dom";

const isAuthenticated = () => {
  const token = localStorage.getItem("token");
  const expiration = localStorage.getItem("expiration");
  if (!token || !expiration) return false;

  return new Date(expiration).getTime() > Date.now();
};

export default function ProtectedRoute() {
  return isAuthenticated() ? <Outlet /> : <Navigate to="/dang-nhap" replace />;
}
