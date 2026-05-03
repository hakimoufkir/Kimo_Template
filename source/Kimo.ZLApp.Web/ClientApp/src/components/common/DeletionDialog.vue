<script lang="ts" setup>
import {Button, Dialog} from "primevue";
import {useI18n} from "vue-i18n";

const {t} = useI18n();

defineProps<{ visible: boolean; content: string }>();
const emit = defineEmits(["update:visible", "delete"]);

function changeVisibility(value: boolean): void {
  emit("update:visible", value);
}

function accept(): void {
  emit("delete");
  changeVisibility(false);
}
</script>

<template>
  <Dialog :visible="visible" modal @update:visible="changeVisibility($event)">
    <template #container>
      <div
        class="flex flex-col items-center p-8 bg-surface-0 dark:bg-surface-900 rounded-border">
        <div
          class="rounded-full bg-primary text-primary-contrast inline-flex justify-center items-center h-16 w-16 -mt-16">
          <i :class="`pi pi-question text-xl`"></i>
        </div>

        <span class="font-bold text-2xl block mb-2 mt-6">
          {{ t("common.confirm") }}
        </span>

        <div class="flex flex-wrap">
          {{ t("common.deleteConfirm", {content: content}) }}
        </div>
        <div class="flex items-center gap-2 mt-6">
          <Button
            :label="t('common.yes')"
            autofocus
            icon="pi pi-check"
            @click="accept"></Button>

          <Button
            :label="t('common.no')"
            icon="pi pi-times"
            severity="secondary"
            @click="changeVisibility(false)"></Button>
        </div>
      </div>
    </template>
  </Dialog>
</template>

<style scoped></style>
