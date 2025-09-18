import React, { type ReactNode } from "react";
import { useDevMode } from "../contexts";

interface DevModeProps {
  children: ReactNode;
  fallback?: ReactNode;
}

export const DevOnly: React.FC<DevModeProps> = ({
  children,
  fallback = null,
}) => {
  const { isDevMode } = useDevMode();
  return isDevMode ? <>{children}</> : <>{fallback}</>;
};
