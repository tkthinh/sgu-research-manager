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
import MarkedWorksPage from "../../features/works/MarkedWorksPage";
import SystemConfigPage from "../../features/settings/systemConfig/SystemConfigPage";
// import EvaluateWorksPage from "../../features/users/EvaluateWorksPage";


export const router = createBrowserRouter([
  {
    element: <App />,
    children: [
      {
        path: "/",
        Component: Layout,
        children: [
          {
            path: "/",
            element: <Dashboard />
          },
          {
            path: "cong-trinh",
            element: <WorkPage />
          },
          {
            path: "dang-ky-quy-doi",
            element: <MarkedWorksPage />
          },
          // {
          //   path: "danh-gia",
          //   element: <EvaluateWorksPage />
          // },
          {
            path: "cau-hinh-he-thong",
            element: <SystemConfigPage />
          },
          {
            path: "cai-dat/loai-cong-trinh",
            element: <WorkTypePage />
          },
          {
            path: "cai-dat/cap-cong-trinh",
            element: <WorkLevelPage />
          },
          {
            path: "cai-dat/vai-tro-tac-gia",
            element: <AuthorRolePage />
          },
          {
            path: "cai-dat/he-so-quy-doi",
            element: <FactorPage />
          },
          {
            path: "cai-dat/muc-dich-quy-doi",
            element: <PurposePage />
          },
          // {
          //   path: "cai-dat/tinh-trang-cong-trinh",
          //   element: <PurposePage />
          // },
          {
            path: "cat-dat/nganh",
            element: <FieldPage />
          },
          {
            path: "cai-dat/nganh-scimago",
            element: <ScimagoFieldPage />
          },
          {
            path: "cai-dat/don-vi",
            element: <DepartmentPage />
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