/**
 * Сервис по работе с CurrencyService
 */
export interface ICurrencyService {
  getRateOnDate(
    baseCurrencyId: number,
    targetCurrencyId: number,
    date: Date
  ): Promise<number>;
}
