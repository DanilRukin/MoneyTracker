import {
  MicroservicesNames,
  MicroserviceStatusName,
  MicroservicesVersions,
} from "../../../shared/constants/Microservices";
import { MicroservicesDescriptions } from "../../../shared/constants/Microservices/MicroservicesDescriptions";
import type { MicroserviceDto } from "../api/contracts/MicroserviceDto";
import type { IDevelopmentService } from "./IDevelopmentService";

/**
 * Мок сервиса разрабочика
 */
export class MockDevelopmentService implements IDevelopmentService {
  private _microservices: MicroserviceDto[] = [
    {
      name: MicroservicesNames.CURRENCY_SERVICE,
      description: MicroservicesDescriptions.CURRENCY_SERVICE,
      apiVersion: MicroservicesVersions.CURRENCY_SERVICE,
      status: MicroserviceStatusName.ONLINE,
    },
  ];
  async getServices(): Promise<MicroserviceDto[]> {
    return this._microservices;
  }
}
