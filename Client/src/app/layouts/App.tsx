import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { Session } from "@toolpad/core";
import { ReactRouterAppProvider } from "@toolpad/core/react-router";
import { useEffect, useMemo, useState } from "react";
import { Outlet, useLocation } from "react-router-dom";
import { toast, ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import { NAVIGATION } from "./Navigation";

const BRANDING = {
  logo: <img src="/logo.png" alt="SGU" />,
  title: "SGU - NCKH",
};

export default function App() {
  const [session, setSession] = useState<Session | null>({
    user: {
      name: "Nguyễn Văn A",
      email: "test@sgu.edu.vn",
    },
  });

  const queryClient = new QueryClient();
  const location = useLocation();

  useEffect(() => {
    toast.dismiss();
  }, [location]);

  const authentication = useMemo(() => {
    return {
      signIn: () => {
        setSession({
          user: {
            name: "Nguyễn Văn A",
            email: "test@sgu.edu.vn",
          },
        });
      },
      signOut: () => {
        setSession(null);
      },
    };
  }, []);

  return (
    <ReactRouterAppProvider
      navigation={NAVIGATION}
      branding={BRANDING}
      session={session}
      authentication={authentication}
    >
      <QueryClientProvider client={queryClient}>
        <ToastContainer position="top-right" autoClose={2000} />
        <Outlet />
      </QueryClientProvider>
    </ReactRouterAppProvider>
  );
}
