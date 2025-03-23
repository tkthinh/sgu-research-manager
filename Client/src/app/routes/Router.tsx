import { createBrowserRouter } from "react-router-dom";
import App from "../layouts/App";
import Layout from "../layouts/Layout";

import SignIn from "../../features/auth/SignIn";
import SignUp from "../../features/auth/SignUp";
import WorkPage from "../../features/works/WorkPage";
import FieldPage from "../../features/settings/fields/FieldPage";
import Dashboard from "../../features/dashboard/Dashboard";
import DepartmentPage from "../../features/settings/departments/DepartmentPage";
import PurposePage from "../../features/settings/purposes/PurposePage";
import ScimagoFieldPage from "../../features/settings/scimagoFields/ScimagoFieldPage";
import FactorPage from "../../features/settings/factors/FactorPage";
import AuthorRolePage from "../../features/settings/authorRoles/AuthorRolePage";
import WorkLevelPage from "../../features/settings/workLevels/WorkLevelPage";
import WorkTypePage from "../../features/settings/workTypes/WorkTypePage";
import ProtectedRoute from "../shared/components/ProtectedRoute";
import UserPage from "../../features/settings/users/UserPage";
import MarkedWorksPage from "../../features/works/MarkedWorksPage";
import SystemConfigPage from "../../features/settings/systemConfig/SystemConfigPage";

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
              // { path: "/cai-dat", element: <Setting /> },
              { path: "/cai-dat/quan-ly-thoi-gian", element: <></> },
              { path: "/cai-dat/loai-cong-trinh", element: <WorkTypePage /> },
              { path: "/cai-dat/cap-cong-trinh", element: <WorkLevelPage /> },
              { path: "/cai-dat/vai-tro-tac-gia", element: <AuthorRolePage /> },
              { path: "/cai-dat/muc-dich-quy-doi", element: <PurposePage /> },
              { path: "/cai-dat/he-so-quy-doi", element: <FactorPage /> },
              { path: "/cai-dat/nganh-scimago", element: <ScimagoFieldPage /> },
              { path: "/cai-dat/don-vi", element: <DepartmentPage /> },
              { path: "/cai-dat/nganh", element: <FieldPage /> },
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