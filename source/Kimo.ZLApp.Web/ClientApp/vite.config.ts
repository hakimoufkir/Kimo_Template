import { defineConfig } from "vite";
import vue from "@vitejs/plugin-vue";
import { readFileSync } from "fs";
import { fileURLToPath, URL } from "node:url";
import tailwindcss from "@tailwindcss/vite";
import { certFilePath, keyFilePath } from "./aspnetcore-https.js";

export default defineConfig(({ command }) => {
  const plugins = [vue(), tailwindcss()];

  if (command === "build") {
    return {
      plugins,
      resolve: {
        alias: {
          "@": fileURLToPath(new URL("./src", import.meta.url))
        }
      }
    };
  }

  return {
    plugins,
    resolve: {
      alias: {
        "@": fileURLToPath(new URL("./src", import.meta.url))
      }
    },
    server: {
      https: {
        key: readFileSync(keyFilePath),
        cert: readFileSync(certFilePath)
      },
      port: 5002,
      proxy: {
        "/api": {
          target: "https://localhost:5001/",
          changeOrigin: true,
          secure: false
        }
      }
    }
  };
});
