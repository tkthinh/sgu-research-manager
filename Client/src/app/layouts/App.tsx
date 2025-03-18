import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { Session } from "@toolpad/core";
import { ReactRouterAppProvider } from "@toolpad/core/react-router";
import { useEffect, useMemo, useState } from "react";
import { Outlet, useLocation } from "react-router-dom";
import { toast, ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import { AcademicTitle } from "../../lib/types/enums/AcademicTitle";
import { OfficerRank } from "../../lib/types/enums/OfficerRank";
import { User } from "../../lib/types/models/User";
import { NAVIGATION } from "./Navigation";

interface CustomSession extends Session {
  user: User;
}

const BRANDING = {
  logo: <img src="/logo.png" alt="SGU" />,
  title: "SGU - NCKH",
};

const demoSession: CustomSession = {
  user: {
    id: "1",
    username: "123456",
    email: "test@sgu.edu.vn",
    fullname: "Nguyễn Văn A",
    academicTitle: AcademicTitle.CN,
    officerRank: OfficerRank.ChuyenVien,
    departmentId: "KHOA GIÁO DỤC QUỐC PHÒNG - AN NINH VÀ GIÁO DỤC THỂ CHẤT",
    fieldId: "Điện - Điện tử - Tự động hóa",
    createdAt: new Date().toISOString(),
    identityId: "identity01",
    role: "admin",
  },
};

export default function App() {
  const [customSession, setCustomSession] = useState<CustomSession | null>(
    demoSession,
  );

  const queryClient = new QueryClient();
  const location = useLocation();

  useEffect(() => {
    toast.dismiss();
  }, [location]);

  const authentication = useMemo(() => {
    return {
      signIn: () => {
        setCustomSession(demoSession);
      },
      signOut: () => {
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
