import { defineConfig } from "cypress";

export default defineConfig({
  e2e: {
    setupNodeEvents(on, config) {
      config.env.BASE_URL = "http://localhost:3000";
      config.env.AUTH_SERVER_BASE_URL = "https://localhost:5001";
      return config;
    },
    baseUrl: "http://localhost:3000",
  },
});
