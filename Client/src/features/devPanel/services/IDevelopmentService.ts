import type { MicroserviceDto } from "../api/contracts/MicroserviceDto";

/**
 * Сервис, предоставляющий функции разработчика
 */
export interface IDevelopmentService {
  /**
   * Получает все существующие сервисы
   */
  getServices(): Promise<MicroserviceDto[]>;
}
