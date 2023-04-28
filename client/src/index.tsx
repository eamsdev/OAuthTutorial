import App from './App';
import './assets/scss/site.scss';
import theme from './themes/theme';
import { Container, ThemeProvider } from '@mui/material';
import CssBaseline from '@mui/material/CssBaseline';
import React from 'react';
import { createRoot } from 'react-dom/client';
import { BrowserRouter } from 'react-router-dom';

createRoot(document.getElementById('root')).render(
  <React.StrictMode>
    <ThemeProvider theme={theme}>
      <CssBaseline />
      <BrowserRouter>
        <App />
      </BrowserRouter>
    </ThemeProvider>
  </React.StrictMode>,
);
