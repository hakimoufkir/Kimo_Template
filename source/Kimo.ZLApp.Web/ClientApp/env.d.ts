/// <reference types="vite/client" />

interface ImportMetaEnv {
  readonly VITE_APP_URL: string;
  readonly VITE_API_URL: string;
  readonly VITE_APP_CHROME_URL: string;
  readonly VITE_AUTHORITY_URL: string;
  readonly VITE_CLIENT_ID: string;
  readonly VITE_AUTHORITY_API_URL: string;
  readonly VITE_APP_CLIENT_CLAIM: string;
}

interface ImportMeta {
  readonly env: ImportMetaEnv;
}
