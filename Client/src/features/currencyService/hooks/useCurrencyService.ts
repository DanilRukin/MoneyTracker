import type { ICurrencyService } from "../services/ICurrencyService";
import { MockCurrenceService } from "../services/MockCurrencyService";

/**
 * Хук-фабрика, возращающая экзмемпляр ICurrencyService
 * @returns Экземпляр ICurrencyService
 */
export const useCurrencyService = (): ICurrencyService => {
  return new MockCurrenceService();
};
