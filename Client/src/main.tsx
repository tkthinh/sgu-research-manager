import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';
import theme from './theme';

import './app/layouts/styles.css';
import '@fontsource/roboto/300.css';
import '@fontsource/roboto/400.css';
import '@fontsource/roboto/500.css';
import '@fontsource/roboto/700.css';

import { ThemeProvider } from '@mui/material';
import { RouterProvider } from 'react-router-dom';
import { router } from './app/routes/Router';

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <ThemeProvider theme={theme}>
      <RouterProvider router={router} />
    </ThemeProvider>
  </StrictMode>
);
