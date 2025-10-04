import type { ICurrencyService } from "./ICurrencyService";

export class MockCurrenceService implements ICurrencyService {
  async getRateOnDate(
    baseCurrencyId: number,
    targetCurrencyId: number,
    date: Date
  ): Promise<number> {
    const fakeRequest = `api/currency/?base=${baseCurrencyId},target=${targetCurrencyId},ondate=${date}`;
    const execute = () => {
      return fakeRequest;
    };
    execute();
    function getNumberFromRange(min: number, max: number) {
      if (max <= min) {
        throw Error("max value can not be less than min value!");
      }
      return Math.floor(Math.random() * (max - min) + 1 + min);
    }
    return getNumberFromRange(1, 500);
  }
}
