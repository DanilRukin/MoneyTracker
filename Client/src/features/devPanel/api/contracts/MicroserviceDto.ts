import type { MicroserviceStatusNameType } from "../../../../shared/constants/Microservices";

export type MicroserviceDto = {
  name: string;
  apiVersion: string;
  description: string;
  status: MicroserviceStatusNameType;
};
