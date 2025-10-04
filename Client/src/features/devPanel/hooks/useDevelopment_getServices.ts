import { useQuery } from "@tanstack/react-query";
import { useDevelopmentService } from "./useDevelopmentService";

/**
 * Хук-запрос, возвращающий все существующие сервисы MoneyTracker
 * @returns Все существующие сервисы MoneyTracker
 */
export const useDevelopment_getServices = () => {
  const developmentService = useDevelopmentService();
  return useQuery({
    queryKey: [],
    queryFn: () => developmentService.getServices(),
    staleTime: 30000,
    retry: 3,
  });
};
