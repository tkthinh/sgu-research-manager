import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { Session } from "@toolpad/core";
import { ReactRouterAppProvider } from "@toolpad/core/react-router";
import { useEffect, useMemo, useState } from "react";
import { Outlet, useLocation, useNavigate } from "react-router-dom";
import { toast, ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import { User } from "../../lib/types/models/User";
import SessionExpiredDialog from "../shared/components/dialogs/SessionExpiredDialog";
import { useAuth } from "../shared/contexts/AuthContext";
import { NAVIGATION_ADMIN, NAVIGATION_MANAGER, NAVIGATION_USER } from "./Navigation";

interface CustomSession extends Session {
  user: User;
}

const BRANDING = {
  logo: <img src="/logo.png" alt="SGU" />,
  title: "SGU - NCKH",
};

export default function App() {
  const [customSession, setCustomSession] = useState<CustomSession | null>(
    null,
  );
  const [showDialog, setShowDialog] = useState(false);

  const { user } = useAuth();
  const userRole = user?.role;

  const navigation = useMemo(() => {
    switch (userRole) {
      case "Admin":
        return NAVIGATION_ADMIN;
      case "Manager":
        return NAVIGATION_MANAGER;
      case "User":
        return NAVIGATION_USER;
      default:
        return [];
    }
  }, [userRole]);

  const queryClient = new QueryClient();
  const location = useLocation();
  const navigate = useNavigate();

  useEffect(() => {
    toast.dismiss();
  }, [location]);

  // Load user session from local storage
  useEffect(() => {
    const token = localStorage.getItem("token");
    if (token) {
      const user = JSON.parse(localStorage.getItem("user") || "{}");
      setCustomSession({ user });
    }
  }, []);

  const authentication = useMemo(() => {
    return {
      signIn: () => {
        navigate("/dang-nhap");
      },
      signOut: () => {
        localStorage.removeItem("token");
        localStorage.removeItem("user");
        setCustomSession(null);
        navigate("/dang-nhap");
      },
    };
  }, []);

  // Automatically sign out user when token expires
  useEffect(() => {
    const expiration = localStorage.getItem("expiration");
    if (!expiration) return;

    const timeout = new Date(expiration).getTime() - Date.now();

    if (timeout > 0) {
      const timer = setTimeout(() => {
        setShowDialog(true);
        authentication.signOut();
      }, timeout);
      return () => clearTimeout(timer);
    } else {
      setShowDialog(true);
      authentication.signOut();
    }
  }, []);

  return (
    <ReactRouterAppProvider
      navigation={navigation}
      branding={BRANDING}
      session={customSession}
      authentication={authentication}
    >
      <QueryClientProvider client={queryClient}>
        <ToastContainer position="top-right" autoClose={2000} />
        <Outlet />
        <SessionExpiredDialog
          open={showDialog}
          onClose={() => navigate("/dang-nhap")}
        />
      </QueryClientProvider>
    </ReactRouterAppProvider>
  );
}
