import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { Session } from "@toolpad/core";
import { ReactRouterAppProvider } from "@toolpad/core/react-router";
import { useEffect, useMemo, useState } from "react";
import { Outlet, useLocation, useNavigate } from "react-router-dom";
import { toast, ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import { User } from "../../lib/types/models/User";
import { isUserProfileIncomplete } from "../../lib/utils/checkUserProfile";
import ProfileIncompletedDialog from "../shared/components/dialogs/ProfileIncompletedDialog";
import SessionExpiredDialog from "../shared/components/dialogs/SessionExpiredDialog";
import { useAuth } from "../shared/contexts/AuthContext";
import {
  NAVIGATION_ADMIN,
  NAVIGATION_MANAGER,
  NAVIGATION_USER,
} from "./Navigation";

interface CustomSession extends Session {
  user: User;
}

const BRANDING = {
  logo: <img src="/logo.png" alt="SGU" />,
  title: "SGU - NCKH",
};

export default function App() {
  const [customSession, setCustomSession] = useState<CustomSession | null>(null);
  const [showTimeoutDialog, setShowTimeoutDialog] = useState(false);
  const [showProfileUpdateDialog, setShowProfileUpdateDialog] = useState(false);

  const { user, loading } = useAuth();

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

  // Update session when user changes
  useEffect(() => {
    if (user) {
      if (isUserProfileIncomplete(user)) {
        setShowProfileUpdateDialog(true);
      }
      
      setCustomSession({ user });
    } else if (!loading) {
      setCustomSession(null);
    }
  }, [user, loading]);

  // Automatically sign out user when token expires
  useEffect(() => {
    const expiration = localStorage.getItem("expiration");
    if (!expiration) return;

    const timeout = new Date(expiration).getTime() - Date.now();

    if (timeout > 0) {
      const timer = setTimeout(() => {
        setShowTimeoutDialog(true);
        authentication.signOut();
      }, timeout);
      return () => clearTimeout(timer);
    } else {
      setShowTimeoutDialog(true);
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
          open={showTimeoutDialog}
          onClose={() => navigate("/dang-nhap")}
        />
        <ProfileIncompletedDialog
          open={showProfileUpdateDialog}
          onClose={() => navigate("/cap-nhat-thong-tin")}
        />
      </QueryClientProvider>
    </ReactRouterAppProvider>
  );
}
