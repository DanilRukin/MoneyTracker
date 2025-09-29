/* eslint-disable @typescript-eslint/no-explicit-any */
import React from "react";

import { useApp } from "../../contexts";
import { useDevLogger } from "../../hooks";
import { WithDevTools } from "../../components/WithDevTools";
import { MicroserviceStatusName } from "../../shared/constants/Microservices";
import {
  Alert,
  alpha,
  Box,
  Button,
  Card,
  CardContent,
  CardHeader,
  Chip,
  CircularProgress,
  Grid,
  Paper,
  Typography,
  useTheme,
} from "@mui/material";
import {
  AccessTime,
  Dns,
  FlashOn,
  Monitor,
  Speed,
  Storage,
  TrendingUp,
} from "@mui/icons-material";
import { PagesDescriptions, PagesTitles } from "../../shared/constants";
import { useDevelopment_getServices } from "./hooks/useDevelopment_getServices";
import { MicroserviceMapper } from "./mappers/MicroserviceMapper";

export const DevPanelPage: React.FC = () => {
  const { setCurrentPage } = useApp();
  const { devLog } = useDevLogger();
  const theme = useTheme();
  const { data, isLoading, error, isError } = useDevelopment_getServices();

  if (isLoading) {
    return (
      <Box
        display="flex"
        justifyContent="center"
        alignItems="center"
        minHeight={200}
      >
        <CircularProgress />
      </Box>
    );
  }

  if (isError) {
    return (
      <Alert severity="error">
        Ошибка при загрузке сервисов: {error?.message}
      </Alert>
    );
  }

  const microservices = data
    ? data.map((service) => MicroserviceMapper.ToMicroserviceStatus(service))
    : [];

  const getStatusChip = (status: string) => {
    const statusConfig = {
      online: {
        color: "success" as const,
        label: MicroserviceStatusName.ONLINE,
      },
      offline: {
        color: "error" as const,
        label: MicroserviceStatusName.OFFLINE,
      },
      degraded: {
        color: "warning" as const,
        label: MicroserviceStatusName.DEGRADED,
      },
    };
    const config = statusConfig[status as keyof typeof statusConfig];
    return (
      <Chip
        label={config.label}
        color={config.color}
        size="small"
        variant="filled"
      />
    );
  };

  const handleServiceClick = (route: string, serviceName: string) => {
    setCurrentPage(route);
    devLog("Navigation to service", { route, serviceName });
  };

  const handleLogsClick = (serviceName: string) => {
    devLog("Requesting logs for service", { serviceName });
  };

  const StatusCard = ({
    icon: Icon,
    title,
    value,
    color,
  }: {
    icon: React.ComponentType<any>;
    title: string;
    value: string;
    color: "primary" | "secondary" | "error" | "warning" | "info" | "success";
  }) => (
    <Card sx={{ height: "100%" }}>
      <CardContent sx={{ p: 2 }}>
        <Box sx={{ display: "flex", alignItems: "center", gap: 2 }}>
          <Box
            sx={{
              p: 1,
              borderRadius: 2,
              backgroundColor: alpha(theme.palette[color].main, 0.1),
              display: "flex",
              alignItems: "center",
              justifyContent: "center",
            }}
          >
            <Icon
              sx={{
                color: `${color}.main`,
                fontSize: 24,
              }}
            />
          </Box>
          <Box>
            <Typography variant="body2" color="text.secondary">
              {title}
            </Typography>
            <Typography variant="h6" color={`${color}.main`} fontWeight="bold">
              {value}
            </Typography>
          </Box>
        </Box>
      </CardContent>
    </Card>
  );

  return (
    <WithDevTools componentName="DevPanel">
      <Box sx={{ p: 3 }}>
        {/* Заголовок */}
        <Box sx={{ mb: 4 }}>
          <Typography variant="h4" component="h1" gutterBottom>
            {PagesTitles.DEV_PANEL}
          </Typography>
          <Typography variant="body1" color="text.secondary">
            {PagesDescriptions.DEV_PANEL}
          </Typography>
        </Box>

        <Grid container spacing={2} sx={{ mb: 4 }}>
          <Grid size={{ xs: 12, sm: 6, md: 3 }}>
            <StatusCard
              icon={Storage}
              title="Активных сервисов"
              value="3"
              color="success"
            />
          </Grid>
          <Grid size={{ xs: 12, sm: 6, md: 3 }}>
            <StatusCard
              icon={Monitor}
              title="Деградированных"
              value="1"
              color="warning"
            />
          </Grid>
          <Grid size={{ xs: 12, sm: 6, md: 3 }}>
            <StatusCard
              icon={FlashOn}
              title="Недоступных"
              value="1"
              color="error"
            />
          </Grid>
          <Grid size={{ xs: 12, sm: 6, md: 3 }}>
            <StatusCard
              icon={Dns}
              title="Общий аптайм"
              value="99.3%"
              color="primary"
            />
          </Grid>
        </Grid>

        {/* Плитки сервисов */}
        <Grid container spacing={3} sx={{ mb: 4 }}>
          {microservices.map((service) => {
            return (
              <Grid size={{ xs: 12, sm: 6, lg: 4 }} key={service.name}>
                <Card
                  sx={{
                    height: "100%",
                    transition: "box-shadow 0.3s ease",
                    "&:hover": {
                      boxShadow: 6,
                    },
                  }}
                >
                  <CardHeader
                    sx={{ pb: 1 }}
                    title={
                      <Box
                        sx={{
                          display: "flex",
                          alignItems: "flex-start",
                          gap: 2,
                        }}
                      >
                        <Box
                          sx={{
                            p: 1,
                            borderRadius: 2,
                            backgroundColor: "primary.50",
                            display: "flex",
                            alignItems: "center",
                            justifyContent: "center",
                          }}
                        >
                          <Box sx={{ color: "primary.main", fontSize: 24 }}>
                            {service.icon}
                          </Box>
                        </Box>
                        <Box sx={{ flex: 1 }}>
                          <Typography variant="h6" component="h3">
                            {service.name}
                          </Typography>
                          <Typography variant="caption" color="text.secondary">
                            {service.version}
                          </Typography>
                        </Box>
                        {getStatusChip(service.status)}
                      </Box>
                    }
                  ></CardHeader>
                  <CardContent sx={{ pt: 0 }}>
                    <Typography
                      variant="body2"
                      color="text.secondary"
                      sx={{
                        mb: 3,
                        display: "-webkit-box",
                        WebkitLineClamp: 3,
                        WebkitBoxOrient: "vertical",
                        overflow: "hidden",
                      }}
                    >
                      {service.description}
                    </Typography>
                    <Box sx={{ display: "flex", gap: 1 }}>
                      <Button
                        variant="contained"
                        size="small"
                        onClick={() =>
                          handleServiceClick(service.route, service.name)
                        }
                        disabled={service.status === "offline"}
                        sx={{ flex: 1 }}
                      >
                        Перейти
                      </Button>
                      <Button
                        variant="outlined"
                        size="small"
                        onClick={() => handleLogsClick(service.name)}
                      >
                        Логи
                      </Button>
                    </Box>
                  </CardContent>
                </Card>
              </Grid>
            );
          })}
        </Grid>

        {/* Общее состояние системы */}
        <Card>
          <CardHeader
            title="Общее состояние системы"
            subheader="Сводная информация о производительности всех сервисов"
          />
          <CardContent>
            <Grid container spacing={2}>
              <Grid size={{ xs: 12, md: 4 }}>
                <Paper
                  sx={{
                    p: 3,
                    textAlign: "center",
                    backgroundColor: "success.50",
                    border: `1px solid ${theme.palette.success.main}20`,
                  }}
                >
                  <Typography
                    variant="h4"
                    color="success.main"
                    fontWeight="bold"
                    gutterBottom
                  >
                    98.7%
                  </Typography>
                  <Typography variant="body2" color="text.secondary">
                    <TrendingUp
                      sx={{
                        fontSize: 16,
                        verticalAlign: "text-bottom",
                        mr: 0.5,
                      }}
                    />
                    Средний аптайм
                  </Typography>
                </Paper>
              </Grid>
              <Grid size={{ xs: 12, md: 4 }}>
                <Paper
                  sx={{
                    p: 3,
                    textAlign: "center",
                    backgroundColor: "info.50",
                    border: `1px solid ${theme.palette.info.main}20`,
                  }}
                >
                  <Typography
                    variant="h4"
                    color="info.main"
                    fontWeight="bold"
                    gutterBottom
                  >
                    247ms
                  </Typography>
                  <Typography variant="body2" color="text.secondary">
                    <Speed
                      sx={{
                        fontSize: 16,
                        verticalAlign: "text-bottom",
                        mr: 0.5,
                      }}
                    />
                    Среднее время ответа
                  </Typography>
                </Paper>
              </Grid>
              <Grid size={{ xs: 12, md: 4 }}>
                <Paper
                  sx={{
                    p: 3,
                    textAlign: "center",
                    backgroundColor: "secondary.50",
                    border: `1px solid ${theme.palette.secondary.main}20`,
                  }}
                >
                  <Typography
                    variant="h4"
                    color="secondary.main"
                    fontWeight="bold"
                    gutterBottom
                  >
                    1,423
                  </Typography>
                  <Typography variant="body2" color="text.secondary">
                    <AccessTime
                      sx={{
                        fontSize: 16,
                        verticalAlign: "text-bottom",
                        mr: 0.5,
                      }}
                    />
                    Запросов в минуту
                  </Typography>
                </Paper>
              </Grid>
            </Grid>
          </CardContent>
        </Card>
      </Box>
    </WithDevTools>
  );
};
