<script lang="ts" setup>
import HelloWorld from "@/components/HelloWorld.vue";
import {Card} from "primevue";
import {useI18n} from "vue-i18n";

const {t} = useI18n();
</script>

<template>
  <Card>
    <template #content>
      <div class="flex">
        <div class="flex-auto flex align-items-center justify-content-center">
          <a href="https://dotnet.microsoft.com/" target="_blank">
            <img alt=".NET logo" class="logo dotnet" src="/dotnet.png"/>
          </a>
          <a href="https://vitejs.dev" target="_blank">
            <img alt="Vite logo" class="logo" src="/vite.svg"/>
          </a>
          <a href="https://vuejs.org/" target="_blank">
            <img alt="Vue logo" class="logo vue" src="/vue.svg"/>
          </a>
          <a href="https://pinia.vuejs.org/" target="_blank">
            <img alt="pinia logo" class="logo pinia" src="/pinia.svg"/>
          </a>
        </div>
      </div>

      <HelloWorld :msg="t('title')"/>
    </template>
  </Card>
</template>

<style scoped>
.logo {
  height: 10em;
  padding: 1.5em;
  will-change: filter;
  transition: filter 300ms;
}

.logo:hover {
  filter: drop-shadow(0 0 2em #646cffaa);
}

.logo.vue:hover {
  filter: drop-shadow(0 0 2em #42b883aa);
}

.logo.pinia:hover {
  filter: drop-shadow(0 0 2em #ffd14d);
}

.logo.dotnet:hover {
  filter: drop-shadow(0 0 2em #512bd4aa);
}
</style>
