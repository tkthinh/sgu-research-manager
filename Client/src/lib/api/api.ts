import axios from 'axios';

const apiClient = axios.create({
  baseURL: import.meta.env.VITE_API_URL || 'https://localhost:7251/api',
  headers: {
    'Content-Type': 'application/json',
  },
  timeout: 10000,
});

// Request interceptor to add the JWT token to the Authorization header
apiClient.interceptors.request.use(
  (config) => {
    // Get the JWT token from localStorage
    const token = localStorage.getItem('token');
    
    // If there's a token, set it in the Authorization header as Bearer token
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }

    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

export default apiClient;
