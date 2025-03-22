import { createBrowserRouter } from "react-router-dom";
import App from "../layouts/App";
import Layout from "../layouts/Layout";

import SignIn from "../../features/auth/SignIn";
import SignUp from "../../features/auth/SignUp";
import WorkPage from "../../features/settings/works/WorkPage";
import FieldPage from "../../features/settings/fields/FieldPage";
import Dashboard from "../../features/dashboard/Dashboard";
import DepartmentPage from "../../features/settings/departments/DepartmentPage";
import PurposePage from "../../features/settings/purposes/PurposePage";
import ScimagoFieldPage from "../../features/settings/scimagoFields/ScimagoFieldPage";
import FactorPage from "../../features/settings/factors/FactorPage";
import AuthorRolePage from "../../features/settings/authorRoles/AuthorRolePage";
import WorkLevelPage from "../../features/settings/workLevels/WorkLevelPage";
import WorkTypePage from "../../features/settings/workTypes/WorkTypePage";
import MarkedWorksPage from "../../features/settings/works/MarkedWorksPage";
import ProtectedRoute from "../shared/components/ProtectedRoute";
import UserPage from "../../features/settings/users/UserPage";
import SystemConfigPage from "../../features/settings/systemConfig/SystemConfigPage";
import Setting from "../../features/settings/Setting";

export const router = createBrowserRouter([
  {
    element: <App />,
    children: [
      {
        element: <ProtectedRoute />,
        children: [
          {
            path: "/",
            Component: Layout,
            children: [
              // ============== CHÍNH ==============
              { path: "/", element: <Dashboard /> },
              { path: "/cong-trinh", element: <WorkPage /> },
              { path: "dang-ky-quy-doi", element: <MarkedWorksPage /> },
              { path: "/cham-diem", element: <></> },
              { path: "/phan-cong", element: <></> },
              // ============== BÁO CÁO ==============
              { path: "/bao-cao", element: <></> },
              // ============== HỆ THỐNG ==============
              { path: "/quan-ly-tai-khoan", element: <UserPage /> },
              { path: "/cau-hinh-he-thong", element: <SystemConfigPage /> },
              { path: "/he-thong", element: <Setting /> },
              { path: "/he-thong/quan-ly-thoi-gian", element: <></> },
              { path: "/he-thong/loai-cong-trinh", element: <WorkTypePage /> },
              { path: "/he-thong/cap-cong-trinh", element: <WorkLevelPage /> },
              { path: "/he-thong/vai-tro-tac-gia", element: <AuthorRolePage /> },
              { path: "/he-thong/muc-dich", element: <PurposePage /> },
              { path: "/he-thong/he-so-quy-doi", element: <FactorPage /> },
              { path: "/he-thong/nganh-scimago", element: <ScimagoFieldPage /> },
              { path: "/he-thong/don-vi", element: <DepartmentPage /> },
              { path: "/he-thong/nganh", element: <FieldPage /> },
            ],
          },
        ],
      },
    ],
  },
  // ============== ĐĂNG NHẬP/ ĐĂNG KÝ ==============
  {
    path: "/dang-ky",
    element: <SignUp />,
  },
  {
    path: "/dang-nhap",
    element: <SignIn />,
  },
]);