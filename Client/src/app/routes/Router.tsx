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
import TestPage from "../../features/test/test";

import PurposePage from "../../features/settings/purposes/PurposePage";
import ScimagoFieldPage from "../../features/settings/scimagoFields/ScimagoFieldPage";

import WorkLevelPage from "../../features/settings/workLevels/WorkLevelPage";
import WorkTypePage from "../../features/settings/workTypes/WorkTypePage";

import AssignmentPage from "../../features/assignments/AssignmentPage";
import AcademicYearPage from "../../features/settings/academicYears/AcademicYearPage";
import SystemConfigPage from "../../features/settings/systemConfigs/SystemConfigPage";
import UserPage from "../../features/settings/users/UserPage";
import ProtectedRoute from "../shared/components/ProtectedRoute";
import NotFound from "../shared/pages/NotFound";
import Unauthorized from "../shared/pages/Unauthorized";

import WorkRegisterPage from "../../features/work-register/WorkRegisterPage";
import WorkScoreDetailPage from "../../features/work-scores/WorkScoreDetailPage";
import UpdateInfoPage from "../../features/update-info/UpdateInfoPage";
import WorkScorePage from "../../features/work-scores/WorkScorePage";
import CachePage from "../../features/settings/caches/CachePage";
import ReportPage from "../../features/report/ReportPage";
import StatisticsPage from '../../features/statistics/StatisticsPage';

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
                  <ProtectedRoute allowedRoles={["User", "Manager"]}>
                    <WorkPage />
                  </ProtectedRoute>
                ),
              },
              {
                path: "dang-ky-quy-doi",
                element: (
                  <ProtectedRoute allowedRoles={["User"]}>
                    <WorkRegisterPage />
                  </ProtectedRoute>
                ),
              },
              {
                path: "/cham-diem",
                element: (
                  <ProtectedRoute allowedRoles={["Manager", "Admin"]}>
                    <WorkScorePage /> 
                  </ProtectedRoute>
                ),
              },
              {
                path: "/cham-diem/user/:userId",
                element: (
                  <ProtectedRoute allowedRoles={["Manager", "Admin"]}>
                    <WorkScoreDetailPage /> 
                  </ProtectedRoute>
                ),
              },
              {
                path: "/phan-cong",
                element: (
                  <ProtectedRoute allowedRoles={["Admin"]}>
                    <AssignmentPage />
                  </ProtectedRoute>
                ),
              },
              {
                path: "/cap-nhat-thong-tin",
                element: (
                  <ProtectedRoute>
                    <UpdateInfoPage />
                  </ProtectedRoute>
                ),
              },
              // ============== TEST ==============
              {
                path: "/test",
                element: (
                  <ProtectedRoute allowedRoles={["User", "Manager", "Admin"]}>
                    <TestPage />
                  </ProtectedRoute>
                ),
              },
              // ============== BÁO CÁO ==============
              {
                path: "/bao-cao",
                element: (
                  <ProtectedRoute allowedRoles={["User", "Manager"]}>
                    <ReportPage />
                  </ProtectedRoute>
                ),
              },
              {
                path: "/thong-ke",
                element: (
                  <ProtectedRoute allowedRoles={["Admin", "Manager"]}>
                    <StatisticsPage />
                  </ProtectedRoute>
                ),
              },
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
              {
                path: "/cai-dat/nam-hoc",
                element: (
                  <ProtectedRoute allowedRoles={["Admin"]}>
                    <AcademicYearPage />
                  </ProtectedRoute>
                ),
              },
              {
                path: "/cai-dat/quan-ly-cache",
                element: (
                  <ProtectedRoute allowedRoles={["Admin"]}>
                    <CachePage />
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

  // ============== TRANG KHÁC ==============
  { path: "/unauthorized", element: <Unauthorized /> },
  { path: "*", element: <NotFound /> },
]);
