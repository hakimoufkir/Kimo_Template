import type { Temperature } from "@/models/forecasts/temperature";
import type { Precipitation } from "@/models/forecasts/precipitation";
import type { Location } from "@/models/locations/location";

export interface Forecast {
  id: number;
  date: string;
  location: Location;
  temperatureData: Temperature[];
  precipitationData: Precipitation[];
}
