import { useDevMode } from "../contexts";

export const useDevLogger = () => {
  const { isDevMode } = useDevMode();
  const devLog = (message: string, data?: unknown) => {
    if (isDevMode) console.log(`[DEV] ${message}`, data || "");
  };

  const devWarn = (message: string, data?: unknown) => {
    if (isDevMode) console.log(`[DEV WARN] ${message}`, data || "");
  };

  return { devLog, devWarn };
};
