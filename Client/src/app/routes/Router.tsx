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

export const router = createBrowserRouter([
  {
    Component: App,
    children: [
      {
        path: "/",
        Component: Layout,
        children: [
          // ============== CHÍNH ==============
          {
            path: "/",
            element: <Dashboard />,
          },
          {
            path: "/cong-trinh",
            element: <></>,
          },
          {
            path: "/cham-diem",
            element: <></>,
          },
          {
            path: "/phan-cong",
            element: <></>,
          },

          // ============== BÁO CÁO ==============
          {
            path: "/bao-cao",
            element: <></>,
          },

          // ============== HỆ THỐNG ==============
          {
            path: "/he-thong",
            element: <Setting />,
          },
          // Quản lý tài khoản
          {
            path: "/he-thong/quan-ly-tai-khoan",
            element: <></>,
          },
          // Quản lý thời gian
          {
            path: "/he-thong/quan-ly-thoi-gian",
            element: <></>,
          },
          // Lịch sử upload
          {
            path: "/he-thong/lich-su-upload",
            element: <></>,
          },
          // Loại công trình
          {
            path: "/he-thong/loai-cong-trinh",
            element: <WorkTypePage />,
          },
          // Cấp công trình
          {
            path: "/he-thong/don-vi",
            element: <DepartmentPage />,
          },
          // Ngành
          {
            path: "/he-thong/nganh",
            element: <FieldPage />,
          },
          // Đơn vị công tác
          {
            path: "/he-thong/don-vi",
            element: <DepartmentPage />,
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
