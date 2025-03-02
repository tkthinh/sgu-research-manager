import { createBrowserRouter } from 'react-router-dom';
import App from '../layouts/App';
import Layout from '../layouts/layout';
import Dashboard from '../../features/dashboard/Dashboard';

export const router = createBrowserRouter([
  {
    Component: App,
    children: [
      {
        path: '/',
        Component: Layout,
        children: [
          {
            path: '/',
            Component: Dashboard,
          }
        ]
      },
    ],
  },
]);
