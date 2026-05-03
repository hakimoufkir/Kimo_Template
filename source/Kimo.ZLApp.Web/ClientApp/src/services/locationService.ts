import axios from "axios";
import { Result } from "@/services/common/Result";
import { ErrorMapper } from "@/utils/mappers/errorMapper";
import type { Location } from "@/models/locations/location";

class LocationService {
  private static readonly BASE_ROUTE = "v1/locations";

  static async getAll(): Promise<Result<Location[]>> {
    try {
      const response = await axios.get(this.BASE_ROUTE);
      return Result.ok(response.data);
    } catch (e: unknown) {
      console.error(e);
      return Result.error(ErrorMapper.mapToError(e));
    }
  }
}

export default LocationService;
