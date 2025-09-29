import { useQuery } from "@tanstack/react-query";
import { useCurrencyService } from "./useCurrencyService";

/**
 * Хук-запрос, возвращающий курс валют на указанную дату
 * @param baseCurrencyId Id базовой валюты
 * @param targetCurrencyId Id целевой валюты
 * @param onDate Дата, на которую запрашивается курс валют
 * @returns Курс указанных валют на указанную дату
 */
export const useCurrencyService_getRateOnDate = (
  baseCurrencyId: number,
  targetCurrencyId: number,
  onDate: Date
) => {
  const currencyService = useCurrencyService();
  return useQuery({
    queryKey: ["currency", "pair"],
    queryFn: () =>
      currencyService.getRateOnDate(baseCurrencyId, targetCurrencyId, onDate),
    staleTime: 30000,
    retry: 3,
  });
};
