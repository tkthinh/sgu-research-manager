import { createBrowserRouter } from "react-router-dom";
import App from "../layouts/App";
import Layout from "../layouts/Layout";

import Dashboard from "../../features/dashboard/Dashboard";
import FieldPage from "../../features/settings/fields/FieldPage";

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
          // Cấu hình hệ thống
          {
            path: "/he-thong/nganh",
            element: <FieldPage />,
          },
        ],
      },
    ],
  },
]);
