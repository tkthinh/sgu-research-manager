import react from "@vitejs/plugin-react-swc";
import dotenv from "dotenv";
import legacy from '@vitejs/plugin-legacy';
import { visualizer } from 'rollup-plugin-visualizer';
import { defineConfig } from "vite";

// Load environment variables from .env files
dotenv.config();

export default defineConfig({
  plugins: [
    react(),
    visualizer({ filename: 'dist/stats.html', open: false }),
    legacy({ targets: ['defaults', 'not IE 11'] }),
  ],
  server: {
    proxy: {
      "/api": {
        target: process.env.VITE_API_URL,
        changeOrigin: true,
        secure: true,
      },
    },
  },
  build: {
    sourcemap: false,
    emptyOutDir: true,
    chunkSizeWarningLimit: 1500,
    rollupOptions: {
      output: {
        manualChunks(id) {
          if (id.includes('node_modules')) return 'vendor';
        },
      },
    },
  }
});
