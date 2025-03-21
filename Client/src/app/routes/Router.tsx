import { createBrowserRouter } from "react-router-dom";
import App from "../layouts/App";
import Layout from "../layouts/Layout";

import SignIn from "../../features/auth/SignIn";
import SignUp from "../../features/auth/SignUp";
import Dashboard from "../../features/dashboard/Dashboard";
import DepartmentPage from "../../features/settings/departments/DepartmentPage";
import FieldPage from "../../features/settings/fields/FieldPage";
import Setting from "../../features/settings/Setting";
import WorkTypePage from "../../features/settings/workTypes/WorkTypePage";
import WorksPage from "../../features/settings/works/WorksPage";
import ProtectedRoute from "../shared/components/ProtectedRoute";

export const router = createBrowserRouter([
  {
    Component: App,
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
              { path: "/cong-trinh", element: <WorksPage /> },
              { path: "/cham-diem", element: <></> },
              { path: "/phan-cong", element: <></> },
              // ============== BÁO CÁO ==============
              { path: "/bao-cao", element: <></> },
              // ============== HỆ THỐNG ==============
              { path: "/he-thong", element: <Setting /> },
              { path: "/he-thong/quan-ly-tai-khoan", element: <></> },
              { path: "/he-thong/quan-ly-thoi-gian", element: <></> },
              { path: "/he-thong/loai-cong-trinh", element: <WorkTypePage /> },
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