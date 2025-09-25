import type { ApiMethodType } from "./ApiMethod";

export interface ApiEndpoint {
  method: ApiMethodType;
  description: string;
  url: string;
  parameters: string;
}
