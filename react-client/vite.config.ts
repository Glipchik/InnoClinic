import react from "@vitejs/plugin-react";
import tailwindcss from "@tailwindcss/vite";
import path from 'path';
import { defineConfig } from 'vitest/config';

export default defineConfig({
  server: {
    port: 3000,
    open: true,
  },
  resolve: {
    alias: {
      "@features": path.resolve(__dirname, "./src/features"),
      "@pages": path.resolve(__dirname, "./src/pages"),
      "@shared": path.resolve(__dirname, "./src/shared"),
      "@entities": path.resolve(__dirname, "./src/entities"),
      "@app": path.resolve(__dirname, "./src/app"),
      "@models": path.resolve(__dirname, "./src/models"),
      "@widgets": path.resolve(__dirname, "./src/widgets")
    }
  },
  plugins: [react(), tailwindcss()],
  test: {
    environment: 'jsdom',
    globals: true,
    setupFiles: "./src/tests/setup.ts"
  }
});
