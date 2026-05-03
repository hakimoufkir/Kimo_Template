import * as yup from "yup";
import { useForm } from "vee-validate";
import { useI18n } from "vue-i18n";
import type { NewForecast } from "@/models/forecasts/newForecast";
import { normalizeDateOnly } from "@/utils/format/dateFormatter";

export function useInsertForecastForm() {
  const { t } = useI18n();

  const schema = yup.object({
    precipitation: yup
      .number()
      .nullable()
      .moreThan(-1, t("validation.greaterThanOrEqual", { value: 0 }))
      .lessThan(101, t("validation.lesserThanOrEqual", { value: 100 })),
    temperature: yup
      .number()
      .nullable()
      .moreThan(-101, t("validation.greaterThanOrEqual", { value: -100 }))
      .lessThan(101, t("validation.lesserThanOrEqual", { value: 100 })),
    timestamp: yup.date().required(t("validation.required")),
    location: yup.number().required(t("validation.required"))
  });

  const { defineField, errors, validate, meta } = useForm({
    validationSchema: schema
  });

  const [precipitation] = defineField("precipitation");
  const [temperature] = defineField("temperature");
  const [timestamp] = defineField("timestamp");
  const [location] = defineField("location");

  function resetForm(): void {
    location.value = null;
    timestamp.value = null;
    temperature.value = null;
    precipitation.value = null;
  }

  async function createForecast(): Promise<NewForecast | null> {
    const { valid } = await validate();
    if (!valid) return null;

    return {
      locationId: location.value,
      date: normalizeDateOnly(timestamp.value),
      hour: timestamp.value.getHours(),
      temperature: temperature.value,
      precipitation: precipitation.value
    };
  }

  return {
    precipitation,
    temperature,
    timestamp,
    location,
    errors,
    meta,
    resetForm,
    createForecast
  };
}
