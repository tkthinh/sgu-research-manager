import { Navigate, createBrowserRouter } from "react-router-dom";
import App from "../layouts/App";
import Layout from "../layouts/Layout";

import WorksPage from "../../features/settings/works/WorksPage";
import FieldsPage from "../../features/settings/fields/FieldPage";
import AuthorRolePage from "../../features/settings/authorRoles/AuthorRolePage";
import PurposePage from "../../features/settings/purposes/PurposePage";
import ScimagoFieldPage from "../../features/settings/scimagoFields/ScimagoFieldPage";
import DepartmentPage from "../../features/settings/departments/DepartmentPage";
import FactorPage from "../../features/settings/factors/FactorPage";

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
            element: <WorksPage />,
          }
        ]
      },
      {
        path: "cai-dat/nganh",
        element: <Layout />,
        children: [
          {
            index: true,
            element: <FieldsPage />,
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
]);
