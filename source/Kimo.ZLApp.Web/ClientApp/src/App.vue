<script lang="ts" setup>
import Toast from "primevue/toast";
import { computed, onMounted, ref } from "vue";
import { useI18n } from "vue-i18n";
import type { MenuItem } from "primevue/menuitem";

const { t } = useI18n();
const isAuthEnabled = "False".toLowerCase() !== "false";
const items = ref<MenuItem[]>([]);
let isAuthenticated = computed(() => false);

onMounted(async () => {
  await refreshMenu();
});


async function refreshMenu(): Promise<void> {

  items.value = [
    {
      label: t("navigation.start"),
      route: "/"
    },
    {
      label: t("navigation.weatherForecasts"),
      route: "/weather-forecasts",
    },
    {
      label: t("navigation.locations"),
      route: "/locations",
    }
  ];
}

async function login(): Promise<void> {
}

async function logout(): Promise<void> {
}
</script>

<template>
  <Toast />

  <div class="app-shell">
    <header class="app-header">
      <div class="brand">
        <div class="brand-mark">{{ t("application.abbreviation") }}</div>
        <div>
          <div class="brand-title">{{ t("application.name") }}</div>
          <div class="brand-subtitle">Template Test App</div>
        </div>
      </div>

      <nav class="app-nav">
        <router-link
          v-for="item in items.filter((entry) => entry.visible !== false)"
          :key="String(item.route)"
          :to="String(item.route)"
          class="nav-link">
          {{ item.label }}
        </router-link>
      </nav>

      <div v-if="isAuthEnabled" class="actions">
        <button
          v-if="!isAuthenticated"
          class="action-button"
          type="button"
          @click="login">
          Sign in
        </button>
        <button
          v-else
          class="action-button secondary"
          type="button"
          @click="logout">
          Sign out
        </button>
      </div>
    </header>

    <main class="app-content">
      <router-view />
    </main>
  </div>
</template>

<style>
:root {
  color-scheme: light;
  font-family: Inter, system-ui, sans-serif;
  background: #f4f7fb;
  color: #162033;
}

body {
  margin: 0;
  background:
    radial-gradient(circle at top left, rgba(83, 144, 217, 0.16), transparent 30%),
    linear-gradient(180deg, #eef4fb 0%, #f8fafc 100%);
}

#app {
  min-height: 100vh;
}

body.p-overflow-hidden {
  padding-inline-end: 0 !important;
  padding-right: 0 !important;
}

.app-shell {
  min-height: 100vh;
}

.app-header {
  display: grid;
  grid-template-columns: auto 1fr auto;
  gap: 1rem;
  align-items: center;
  padding: 1rem 1.5rem;
  background: rgba(255, 255, 255, 0.88);
  backdrop-filter: blur(14px);
  border-bottom: 1px solid rgba(22, 32, 51, 0.08);
  position: sticky;
  top: 0;
  z-index: 10;
}

.brand {
  display: flex;
  align-items: center;
  gap: 0.75rem;
}

.brand-mark {
  width: 2.5rem;
  height: 2.5rem;
  display: grid;
  place-items: center;
  border-radius: 0.85rem;
  background: linear-gradient(135deg, #285ea8, #4a90e2);
  color: #fff;
  font-weight: 700;
}

.brand-title {
  font-weight: 700;
}

.brand-subtitle {
  font-size: 0.85rem;
  color: #5b667a;
}

.app-nav {
  display: flex;
  flex-wrap: wrap;
  justify-content: center;
  gap: 0.75rem;
}

.nav-link {
  text-decoration: none;
  color: #20304a;
  padding: 0.55rem 0.9rem;
  border-radius: 999px;
  transition: background-color 0.2s ease, color 0.2s ease;
}

.nav-link.router-link-active {
  background: #1d4f91;
  color: #fff;
}

.nav-link:hover {
  background: rgba(29, 79, 145, 0.1);
}

.actions {
  display: flex;
  justify-content: flex-end;
}

.action-button {
  border: 0;
  border-radius: 999px;
  padding: 0.65rem 1rem;
  cursor: pointer;
  background: #1d4f91;
  color: #fff;
  font-weight: 600;
}

.action-button.secondary {
  background: #e6ecf5;
  color: #20304a;
}

.app-content {
  width: min(1100px, calc(100% - 2rem));
  margin: 1.5rem auto;
}

@media (max-width: 900px) {
  .app-header {
    grid-template-columns: 1fr;
    justify-items: start;
  }

  .app-nav {
    justify-content: flex-start;
  }
}
</style>
