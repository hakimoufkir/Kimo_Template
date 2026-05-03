import { createRouter, createWebHistory } from "vue-router";
import { AppPermissions } from "@/appPermissions";

const useAuth = "False".toLowerCase() !== "false";

const index = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: "/",
      name: "Home",
      meta: {
        requiresAuth: false,
        permissions: []
      },
      component: () => import("@/views/HomeView.vue")
    },
    {
      path: "/weather-forecasts",
      name: "WeatherForecasts",
      meta: {
        requiresAuth: useAuth,
        permissions: useAuth ? [AppPermissions.Forecasts.Read] : []
      },
      component: () => import("@/views/ForecastsView.vue")
    },
    {
      path: "/locations",
      name: "Locations",
      meta: {
        requiresAuth: useAuth,
        permissions: useAuth ? [AppPermissions.Locations.Read] : []
      },
      component: () => import("@/views/LocationsView.vue")
    },
    {
      path: "/:pathMatch(.*)*",
      name: "NotFound",
      meta: {
        requiresAuth: false,
        permissions: []
      },
      component: () => import("@/views/NotFoundView.vue")
    }
  ]
});


export default index;
