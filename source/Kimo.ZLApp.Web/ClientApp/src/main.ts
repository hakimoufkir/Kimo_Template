import { createApp } from "vue";
import App from "@/App.vue";
import axios from "axios";
import ToastService from "primevue/toastservice";
import PrimeVue from "primevue/config";
import Aura from "@primeuix/themes/aura";
import "primeicons/primeicons.css";
import router from "@/router";
import { createPinia } from "pinia";
import { createI18n } from "vue-i18n";
import * as en from "@/translations/en.json";
import ConfirmationService from "primevue/confirmationservice";

const app = createApp(App);
const pinia = createPinia();
const i18n = createI18n({
  locale: "en",
  fallbackLocale: "en",
  messages: {
    en
  }
});

axios.defaults.baseURL = import.meta.env.VITE_APP_URL + "/api/";


app
  .use(pinia)
  .use(i18n)
  .use(router)
  .use(PrimeVue, {
    theme: {
      preset: Aura
    }
  })
  .use(ToastService)
  .use(ConfirmationService);

app.provide("axios", axios);
app.provide("router", router);

app.mount("#app");
