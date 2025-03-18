import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { Session } from "@toolpad/core";
import { ReactRouterAppProvider } from "@toolpad/core/react-router";
import { useEffect, useMemo, useState } from "react";
import { Outlet, useLocation } from "react-router-dom";
import { toast, ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import { User } from "../../lib/types/models/User";
import { NAVIGATION } from "./Navigation";

interface CustomSession extends Session {
  user: User;
}

const BRANDING = {
  logo: <img src="/logo.png" alt="SGU" />,
  title: "SGU - NCKH",
};

export default function App() {
  const [customSession, setCustomSession] = useState<CustomSession | null>(null);

  const queryClient = new QueryClient();
  const location = useLocation();

  useEffect(() => {
    toast.dismiss();
  }, [location]);

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
        const storedUserData = JSON.parse(localStorage.getItem("user") || "{}");
        setCustomSession({ user: storedUserData });
      },
      signOut: () => {
        localStorage.removeItem("token");
        localStorage.removeItem("user");
        setCustomSession(null);
      },
    };
  }, []);

  return (
    <ReactRouterAppProvider
      navigation={NAVIGATION}
      branding={BRANDING}
      session={customSession}
      authentication={authentication}
    >
      <QueryClientProvider client={queryClient}>
        <ToastContainer position="top-right" autoClose={2000} />
        <Outlet />
      </QueryClientProvider>
    </ReactRouterAppProvider>
  );
}
