import { Navigate, createBrowserRouter } from "react-router-dom";
import App from "../layouts/App";
import Layout from "../layouts/Layout";

import SignIn from "../../features/auth/SignIn";
import SignUp from "../../features/auth/SignUp";
import WorkPage from "../../features/settings/works/WorkPage";
import FieldPage from "../../features/settings/fields/FieldPage";
import DepartmentPage from "../../features/settings/departments/DepartmentPage";
import PurposePage from "../../features/settings/purposes/PurposePage";
import ScimagoFieldPage from "../../features/settings/scimagoFields/ScimagoFieldPage";
import FactorPage from "../../features/settings/factors/FactorPage";
import AuthorRolePage from "../../features/settings/authorRoles/AuthorRolePage";
import WorkLevelPage from "../../features/settings/workLevels/WorkLevelPage";
import WorkTypePage from "../../features/settings/workTypes/WorkTypePage";

export const router = createBrowserRouter([
  {
    element: <App />,
    children: [
      {
        path: "/",
        element: <Navigate to="/dashboard" replace />,
      },
      {
        path: "dashboard",
        element: <Layout />,
        children: [
          {
            index: true,
            element: <div>Dashboard</div>,
          },
        ],
      },
      {
        path: "cong-trinh",
        element: <Layout />,
        children: [
          {
            index: true,
            element: <WorkPage />,
          }
        ]
      },
      {
        path: "cai-dat/loai-cong-trinh",
        element: <Layout />,
        children: [
          {
            index: true,
            element: <WorkTypePage />,
          }
        ]
      },
      {
        path: "cai-dat/nganh",
        element: <Layout />,
        children: [
          {
            index: true,
            element: <FieldPage />,
          }
        ]
      },
      {
        path: "cai-dat/cap-cong-trinh",
        element: <Layout />,
        children: [
          {
            index: true,
            element: <WorkLevelPage />,
          }
        ]
      },
      {
        path: "cai-dat/vai-tro-tac-gia",
        element: <Layout />,
        children: [
          {
            index: true,
            element: <AuthorRolePage />,
          }
        ]
      },
      {
        path: "cai-dat/muc-dich",
        element: <Layout />,
        children: [
          {
            index: true,
            element: <PurposePage />,
          }
        ]
      },
      {
        path: "cai-dat/nganh-scimago",
        element: <Layout />,
        children: [
          {
            index: true,
            element: <ScimagoFieldPage />,
          }
        ]
      },
      {
        path: "cai-dat/don-vi",
        element: <Layout />,
        children: [
          {
            index: true,
            element: <DepartmentPage />,
          }
        ]
      },
      {
        path: "cai-dat/he-so-quy-doi",
        element: <Layout />,
        children: [
          {
            index: true,
            element: <FactorPage />,
          }
        ]
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