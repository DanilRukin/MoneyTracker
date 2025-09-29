import type { ReactElement } from "react";
import {
  MicroservicesNames,
  MicroservicesRoutes,
} from "../../../shared/constants/Microservices";
import LanguageIcon from "@mui/icons-material/Language";

export const getServiceIcon = (serviceName: string): ReactElement => {
  const iconMap = {
    [MicroservicesNames.CURRENCY_SERVICE]: <LanguageIcon />,
  };

  return iconMap[serviceName as keyof typeof iconMap] || <LanguageIcon />;
};

export const getServiceRoute = (serviceName: string): string => {
  const routeMap = {
    [MicroservicesNames.CURRENCY_SERVICE]: MicroservicesRoutes.CURRENCY_SERVICE,
  };

  return (
    routeMap[serviceName as keyof typeof routeMap] || MicroservicesNames.DEFAULT
  );
};
