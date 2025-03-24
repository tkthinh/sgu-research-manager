import { createBrowserRouter } from "react-router-dom";
import App from "../layouts/App";
import Layout from "../layouts/Layout";

import SignIn from "../../features/auth/SignIn";
import SignUp from "../../features/auth/SignUp";
import Dashboard from "../../features/dashboard/Dashboard";
import Setting from "../../features/settings/Setting";
import AuthorRolePage from "../../features/settings/authorRoles/AuthorRolePage";
import DepartmentPage from "../../features/settings/departments/DepartmentPage";
import FactorPage from "../../features/settings/factors/FactorPage";
import FieldPage from "../../features/settings/fields/FieldPage";
import WorkPage from "../../features/works/WorkPage";

import PurposePage from "../../features/settings/purposes/PurposePage";
import ScimagoFieldPage from "../../features/settings/scimagoFields/ScimagoFieldPage";

import WorkLevelPage from "../../features/settings/workLevels/WorkLevelPage";
import WorkTypePage from "../../features/settings/workTypes/WorkTypePage";

import AssignmentPage from "../../features/assignments/AssignmentPage";
import SystemConfigPage from "../../features/settings/systemConfig/SystemConfigPage";
import UserPage from "../../features/settings/users/UserPage";
import UserListPage from "../../features/users/UserListPage";
import MarkedWorksPage from "../../features/works/MarkedWorksPage";
import ProtectedRoute from "../shared/components/ProtectedRoute";
import SystemConfigPage from "../../features/settings/systemConfig/SystemConfigPage";
import ScoreWorksPage from "../../features/users/UserListPage";
import UserWorkDetailPage from "../../features/users/UserWorkDetailPage";

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
              {
                path: "/cong-trinh",
                element: (
                  <ProtectedRoute allowedRoles={["User", "Manager", "Admin"]}>
                    <WorkPage />
                  </ProtectedRoute>
                ),
              },
              {
                path: "dang-ky-quy-doi",
                element: (
                  <ProtectedRoute allowedRoles={["User"]}>
                    <MarkedWorksPage />
                  </ProtectedRoute>
                ),
              },
              { path: "/cham-diem", element: <ScoreWorksPage /> },
              { path: "/cham-diem/user/:userId", element: <UserWorkDetailPage /> },
              {
                path: "/phan-cong",
                element: (
                  <ProtectedRoute allowedRoles={["Admin"]}>
                    <AssignmentPage />
                  </ProtectedRoute>
                ),
              },
              {
                path: "/danh-sach-nguoi-dung",
                element: (
                  <ProtectedRoute allowedRoles={["Manager"]}>
                    <UserListPage />
                  </ProtectedRoute>
                ),
              },
              // ============== BÁO CÁO ==============
              { path: "/bao-cao", element: <></> },
              // ============== HỆ THỐNG ==============
              {
                path: "/quan-ly-tai-khoan",
                element: (
                  <ProtectedRoute allowedRoles={["Admin"]}>
                    <UserPage />
                  </ProtectedRoute>
                ),
              },
              {
                path: "/cau-hinh-he-thong",
                element: (
                  <ProtectedRoute allowedRoles={["Admin"]}>
                    <SystemConfigPage />
                  </ProtectedRoute>
                ),
              },
              {
                path: "/cai-dat",
                element: (
                  <ProtectedRoute allowedRoles={["Admin"]}>
                    <Setting />
                  </ProtectedRoute>
                ),
              },
              {
                path: "/cai-dat/loai-cong-trinh",
                element: (
                  <ProtectedRoute allowedRoles={["Admin"]}>
                    <WorkTypePage />
                  </ProtectedRoute>
                ),
              },
              {
                path: "/cai-dat/cap-cong-trinh",
                element: (
                  <ProtectedRoute allowedRoles={["Admin"]}>
                    <WorkLevelPage />
                  </ProtectedRoute>
                ),
              },
              {
                path: "/cai-dat/vai-tro-tac-gia",
                element: (
                  <ProtectedRoute allowedRoles={["Admin"]}>
                    <AuthorRolePage />
                  </ProtectedRoute>
                ),
              },
              {
                path: "/cai-dat/muc-dich-quy-doi",
                element: (
                  <ProtectedRoute allowedRoles={["Admin"]}>
                    <PurposePage />
                  </ProtectedRoute>
                ),
              },
              {
                path: "/cai-dat/he-so-quy-doi",
                element: (
                  <ProtectedRoute allowedRoles={["Admin"]}>
                    <FactorPage />
                  </ProtectedRoute>
                ),
              },
              {
                path: "/cai-dat/nganh-scimago",
                element: (
                  <ProtectedRoute allowedRoles={["Admin"]}>
                    <ScimagoFieldPage />
                  </ProtectedRoute>
                ),
              },
              {
                path: "/cai-dat/don-vi",
                element: (
                  <ProtectedRoute allowedRoles={["Admin"]}>
                    <DepartmentPage />
                  </ProtectedRoute>
                ),
              },
              {
                path: "/cai-dat/nganh",
                element: (
                  <ProtectedRoute allowedRoles={["Admin"]}>
                    <FieldPage />
                  </ProtectedRoute>
                ),
              },
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
