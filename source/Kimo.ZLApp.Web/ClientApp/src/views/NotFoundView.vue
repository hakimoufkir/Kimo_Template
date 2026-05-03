<script lang="ts" setup>
import Button from "primevue/button";
import { useRouter } from "vue-router";

const router = useRouter();
</script>

<template>
  <section class="status-card">
    <p class="eyebrow">404</p>
    <h1>Page not found</h1>
    <p>The requested route does not exist or has been moved.</p>
    <div class="actions">
      <Button
        icon="pi pi-arrow-left"
        label="Back"
        severity="secondary"
        @click="router.go(-1)" />
      <router-link class="status-link" to="/">Back to home</router-link>
    </div>
  </section>
</template>

<style scoped>
.status-card {
  max-width: 42rem;
  margin: 3rem auto;
  padding: 2rem;
  border-radius: 1.25rem;
  background: #fff;
  border: 1px solid rgba(22, 32, 51, 0.08);
  box-shadow: 0 20px 40px rgba(21, 33, 54, 0.08);
}

.eyebrow {
  margin: 0 0 0.75rem;
  color: #1d4f91;
  font-weight: 700;
}

h1 {
  margin: 0 0 0.75rem;
}

p {
  margin: 0 0 1.5rem;
  color: #5b667a;
}

.actions {
  display: flex;
  gap: 0.75rem;
  align-items: center;
}

.status-link {
  color: #1d4f91;
  font-weight: 600;
  text-decoration: none;
}
</style>
