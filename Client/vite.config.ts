import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react-swc'

// https://vite.dev/config/
export default defineConfig({
  plugins: [react()],
  server: {
    proxy: {
      "/api": {
        target: "localhost:7251/api",
        changeOrigin: true,
        secure: true,
      },
    },
  },
})
