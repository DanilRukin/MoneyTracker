import type { MicroserviceDto } from "../api/contracts/MicroserviceDto";
import type { MicroserviceStatus } from "../models/MicroserviceStatus";
import { getServiceIcon, getServiceRoute } from "./MapperHelpers";

export class MicroserviceMapper {
  public static ToMicroserviceStatus(dto: MicroserviceDto): MicroserviceStatus {
    if (dto === null) {
      throw new Error("Dto can not be null!");
    }
    return {
      name: dto.name,
      description: dto.description,
      version: dto.apiVersion,
      status: dto.status,
      icon: getServiceIcon(dto.name),
      route: getServiceRoute(dto.name),
    };
  }
}
