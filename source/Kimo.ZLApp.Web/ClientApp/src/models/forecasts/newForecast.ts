export interface NewForecast {
  date: string;
  locationId: number;
  hour: number;
  temperature: number | null;
  precipitation: number | null;
}
