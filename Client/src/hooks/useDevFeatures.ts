import type React from "react";
import { useDevMode } from "../contexts";

export const useDevFeatures = () => {
  const { isDevMode } = useDevMode();

  const withDevMode = (
    component: React.ReactNode,
    fallback: React.ReactNode = null
  ) => {
    return isDevMode ? component : fallback;
  };

  const devOnly = <T>(value: T, fallback: T): T => {
    return isDevMode ? value : fallback;
  };

  return {
    isDevMode,
    withDevMode,
    devOnly,
  };
};
