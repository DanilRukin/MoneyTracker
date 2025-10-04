import type { IDevelopmentService } from "../services/IDevelopmentService";
import { MockDevelopmentService } from "../services/MockDevelopmentService";

/**
 * Хук-фабрика, создающая экзмепляр IDevelopmentService
 */
export const useDevelopmentService = (): IDevelopmentService => {
  // Подмени логику на свою
  return new MockDevelopmentService();
};
