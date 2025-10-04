import type { MicroserviceStatusNameType } from "../../../shared/constants/Microservices";

export interface MicroserviceStatus {
  name: string;
  status: MicroserviceStatusNameType;
  version: string;
  description: string;
  icon: React.ReactElement;
  route: string;
}
