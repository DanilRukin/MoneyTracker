import type { MicroserviceStatusNameType } from "../../../shared/constants/Microservices";

export interface ServiceHealth {
  status: MicroserviceStatusNameType;
  httpStatus: number;
  responseTime: number;
  message: string;
  uptime: string;
  memoryUsage: { used: number; total: number };
  cacheCount: number;
  rps: number;
  errorRate: number;
  avgResponseTime: number;
  healthScore: number;
}
