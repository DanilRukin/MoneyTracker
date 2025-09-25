export const MicroserviceStatusName = {
  ONLINE: "online",
  OFFLINE: "offline",
  DEGRADED: "degraded",
} as const;

export type MicroserviceStatusNameType =
  (typeof MicroserviceStatusName)[keyof typeof MicroserviceStatusName];
