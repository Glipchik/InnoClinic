import { defineConfig } from "cypress";

export default defineConfig({
  e2e: {
    setupNodeEvents(on, config) {
      config.env.BASE_URL = process.env.VITE_BASE_URL;
      config.env.AUTH_SERVER_BASE_URL = process.env.VITE_AUTH_SERVER_BASE_URL;
      return config;
    },
    baseUrl: "http://localhost:3000",
  },
});
