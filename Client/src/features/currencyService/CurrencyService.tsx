// pages/currency-service/CurrencyServicePage.tsx
import React, { useState } from "react";
import {
  Box,
  Typography,
  Grid,
  Card,
  CardContent,
  CardHeader,
  Button,
  Chip,
  Paper,
  FormControl,
  InputLabel,
  Select,
  MenuItem,
  TextField,
  Switch,
  FormControlLabel,
  Divider,
  CircularProgress,
  Collapse,
  IconButton,
  LinearProgress,
} from "@mui/material";
import {
  Public as GlobeIcon,
  CheckCircle as CheckCircleIcon,
  Warning as WarningIcon,
  Error as ErrorIcon,
  Refresh as RefreshIcon,
  PlayArrow as PlayIcon,
  Delete as DeleteIcon,
  Description as FileTextIcon,
  AccessTime as ClockIcon,
  ExpandMore as ExpandMoreIcon,
  Storage as DatabaseIcon,
  MonitorHeart as ActivityIcon,
} from "@mui/icons-material";
import { useDevLogger } from "../../hooks";
import { WithDevTools } from "../../components/WithDevTools";
import type { ServiceHealth } from "./Entities/ServiceHealth";
import type { CurrencyPair } from "./Entities/CurrencyPair";
import type { ApiEndpoint } from "../../shared/api/ApiEndpoint";
import { MicroservicesDescriptions } from "../../shared/constants/Microservices/MicroservicesDescriptions";
import { MicroservicesNames } from "../../shared/constants/Microservices";

// Mock data
const mockCurrencies = ["USD", "EUR", "RUB", "CNY", "JPY", "GBP", "CAD", "AUD"];
const mockProviders = ["ЦБ РФ", "OpenExchangeRates", "CoinGecko", "Mock Data"];

const mockHealthData: ServiceHealth = {
  status: "online",
  httpStatus: 200,
  responseTime: 145,
  message: "Service is healthy",
  uptime: "5d 12h 34m",
  memoryUsage: { used: 145, total: 512 },
  cacheCount: 47,
  rps: 23.5,
  errorRate: 0.02,
  avgResponseTime: 178,
  healthScore: 96,
};

const mockApiEndpoints: ApiEndpoint[] = [
  {
    method: "GET",
    url: "/api/rates/current",
    description: "Получить актуальные курсы валют",
    parameters: "base, target, amount (optional)",
  },
  {
    method: "GET",
    url: "/api/rates/history",
    description: "Получить исторические данные курсов",
    parameters: "base, target, from, to, period",
  },
  {
    method: "GET",
    url: "/api/health",
    description: "Проверка состояния сервиса",
    parameters: "none",
  },
  {
    method: "POST",
    url: "/api/cache/warm",
    description: "Предварительная загрузка кэша",
    parameters: "currencies[] (optional)",
  },
];

// const mockChartData = [
//   { date: "2025-01-15", rate: 1.0823 },
//   { date: "2025-01-16", rate: 1.0845 },
//   { date: "2025-01-17", rate: 1.0891 },
//   { date: "2025-01-18", rate: 1.0876 },
//   { date: "2025-01-19", rate: 1.0912 },
//   { date: "2025-01-20", rate: 1.0934 },
// ];

export const CurrencyServicePage: React.FC = () => {
  const { devLog } = useDevLogger();

  const [health, setHealth] = useState<ServiceHealth>(mockHealthData);
  const [selectedPair, setSelectedPair] = useState<CurrencyPair>({
    from: "USD",
    to: "EUR",
    rate: 0.9234,
    lastUpdated: "2025-01-20 14:23:15",
  });
  const [provider, setProvider] = useState("ЦБ РФ");
  const [apiKey, setApiKey] = useState("");
  const [updateInterval, setUpdateInterval] = useState(60);
  const [intervalUnit, setIntervalUnit] = useState("minutes");
  const [manualOverride, setManualOverride] = useState(false);
  const [overrideRate, setOverrideRate] = useState("");
  const [chaosMode, setChaosMode] = useState("none");
  const [showLogs, setShowLogs] = useState(false);
  const [isChecking, setIsChecking] = useState(false);
  const [openEndpoints, setOpenEndpoints] = useState<{
    [key: number]: boolean;
  }>({});

  const checkHealth = async () => {
    setIsChecking(true);
    devLog("Checking service health");

    setTimeout(() => {
      setHealth((prev) => ({
        ...prev,
        responseTime: Math.floor(Math.random() * 300) + 100,
        rps: Math.random() * 50 + 10,
        errorRate: Math.random() * 0.05,
      }));
      setIsChecking(false);
      devLog("Health check completed");
    }, 1000);
  };

  const warmCache = () => {
    devLog("Warming cache");
    // Mock implementation
  };

  const clearCache = () => {
    devLog("Clearing cache");
    // Mock implementation
  };

  const forceUpdate = () => {
    devLog("Forcing currency update");
    setSelectedPair((prev) => ({
      ...prev,
      rate: prev.rate + (Math.random() - 0.5) * 0.01,
      lastUpdated: new Date().toLocaleString("ru-RU"),
    }));
  };

  const renderStatusIndicator = () => {
    const { status } = health;
    const statusConfig = {
      online: {
        color: "success" as const,
        icon: CheckCircleIcon,
        label: "Online",
      },
      offline: { color: "error" as const, icon: ErrorIcon, label: "Offline" },
      degraded: {
        color: "warning" as const,
        icon: WarningIcon,
        label: "Degraded",
      },
    };

    const config = statusConfig[status];
    const IconComponent = config.icon;

    return (
      <Box sx={{ display: "flex", alignItems: "center", gap: 1 }}>
        <IconComponent sx={{ color: `${config.color}.main`, fontSize: 20 }} />
        <Typography variant="body2" fontWeight="medium">
          {config.label}
        </Typography>
      </Box>
    );
  };

  const renderHealthScore = () => {
    const score = health.healthScore;
    const color = score >= 90 ? "success" : score >= 70 ? "warning" : "error";

    return (
      <Box sx={{ position: "relative", width: 80, height: 80 }}>
        <CircularProgress
          variant="determinate"
          value={score}
          size={80}
          thickness={4}
          color={color}
        />
        <Box
          sx={{
            position: "absolute",
            top: 0,
            left: 0,
            bottom: 0,
            right: 0,
            display: "flex",
            alignItems: "center",
            justifyContent: "center",
          }}
        >
          <Typography variant="h6" color={`${color}.main`} fontWeight="bold">
            {score}%
          </Typography>
        </Box>
      </Box>
    );
  };

  const toggleEndpoint = (index: number) => {
    setOpenEndpoints((prev) => ({
      ...prev,
      [index]: !prev[index],
    }));
  };

  const InfoCard = ({
    title,
    value,
    description,
  }: {
    title: string;
    value: string;
    description?: string;
  }) => (
    <Card sx={{ height: "100%" }}>
      <CardContent sx={{ p: 2 }}>
        <Typography variant="body2" color="text.secondary" gutterBottom>
          {title}
        </Typography>
        <Typography variant="h6" fontWeight="bold" gutterBottom>
          {value}
        </Typography>
        {description && (
          <Typography variant="caption" color="text.secondary">
            {description}
          </Typography>
        )}
      </CardContent>
    </Card>
  );

  const StatCard = ({
    title,
    value,
    subtitle,
    color = "primary",
  }: {
    title: string;
    value: string;
    subtitle?: string;
    color?: "primary" | "secondary" | "error" | "warning" | "info" | "success";
  }) => (
    <Card>
      <CardContent sx={{ p: 2, textAlign: "center" }}>
        <Typography variant="body2" color="text.secondary" gutterBottom>
          {title}
        </Typography>
        <Typography
          variant="h4"
          color={`${color}.main`}
          fontWeight="bold"
          gutterBottom
        >
          {value}
        </Typography>
        {subtitle && (
          <Typography variant="caption" color="text.secondary">
            {subtitle}
          </Typography>
        )}
      </CardContent>
    </Card>
  );

  return (
    <WithDevTools componentName="CurrencyService">
      <Box sx={{ p: 3 }}>
        {/* Hero Section */}
        <Box sx={{ mb: 4 }}>
          <Box sx={{ display: "flex", alignItems: "center", gap: 2, mb: 3 }}>
            <GlobeIcon sx={{ fontSize: 32, color: "primary.main" }} />
            <Box>
              <Typography variant="h4" component="h1" fontWeight="bold">
                {MicroservicesNames.CURRENCY_SERVICE}
              </Typography>
              <Typography variant="body1" color="text.secondary">
                {MicroservicesDescriptions.CURRENCY_SERVICE}
              </Typography>
            </Box>
          </Box>

          <Grid container spacing={2}>
            <Grid size={{ xs: 12, sm: 6, md: 3 }}>
              <InfoCard
                title="Хост"
                value="https://currency-api.internal:7042"
              />
            </Grid>
            <Grid size={{ xs: 12, sm: 6, md: 3 }}>
              <InfoCard title="Версия API" value="v1.2" />
            </Grid>
            <Grid size={{ xs: 12, sm: 6, md: 3 }}>
              <Card sx={{ height: "100%" }}>
                <CardContent sx={{ p: 2 }}>
                  <Typography
                    variant="body2"
                    color="text.secondary"
                    gutterBottom
                  >
                    Статус
                  </Typography>
                  {renderStatusIndicator()}
                </CardContent>
              </Card>
            </Grid>
            <Grid size={{ xs: 12, sm: 6, md: 3 }}>
              <InfoCard
                title="Описание"
                value={MicroservicesDescriptions.CURRENCY_SERVICE}
              />
            </Grid>
          </Grid>
        </Box>

        {/* Health & Stats Dashboard */}
        <Card sx={{ mb: 3 }}>
          <CardHeader
            title={
              <Box sx={{ display: "flex", alignItems: "center", gap: 1 }}>
                <ActivityIcon />
                <Typography variant="h6">Дашборд состояния</Typography>
              </Box>
            }
          />
          <CardContent>
            <Box sx={{ display: "flex", alignItems: "center", gap: 2, mb: 3 }}>
              <Button
                variant="contained"
                onClick={checkHealth}
                disabled={isChecking}
                startIcon={
                  <RefreshIcon
                    sx={{
                      animation: isChecking
                        ? "spin 1s linear infinite"
                        : "none",
                    }}
                  />
                }
              >
                Проверить здоровье
              </Button>
              <Box sx={{ display: "flex", gap: 2 }}>
                <Chip label={`HTTP: ${health.httpStatus}`} variant="outlined" />
                <Chip
                  label={`Время ответа: ${health.responseTime}ms`}
                  variant="outlined"
                />
                <Typography variant="body2" color="text.secondary">
                  {health.message}
                </Typography>
              </Box>
            </Box>

            <Grid container spacing={2} sx={{ mb: 3 }}>
              <Grid size={{ xs: 12, sm: 6, md: 3 }}>
                <StatCard
                  title="Аптайм"
                  value={health.uptime}
                  color="success"
                />
              </Grid>
              <Grid size={{ xs: 12, sm: 6, md: 3 }}>
                <Card>
                  <CardContent sx={{ p: 2 }}>
                    <Typography
                      variant="body2"
                      color="text.secondary"
                      gutterBottom
                    >
                      Память
                    </Typography>
                    <Typography variant="h6" gutterBottom>
                      {health.memoryUsage.used} MB / {health.memoryUsage.total}{" "}
                      MB
                    </Typography>
                    <LinearProgress
                      variant="determinate"
                      value={
                        (health.memoryUsage.used / health.memoryUsage.total) *
                        100
                      }
                      sx={{ mt: 1 }}
                    />
                  </CardContent>
                </Card>
              </Grid>
              <Grid size={{ xs: 12, sm: 6, md: 3 }}>
                <StatCard
                  title="Валют в кэше"
                  value={health.cacheCount.toString()}
                  color="info"
                />
              </Grid>
              <Grid size={{ xs: 12, sm: 6, md: 3 }}>
                <StatCard
                  title="RPS"
                  value={health.rps.toFixed(1)}
                  color="primary"
                />
              </Grid>
            </Grid>

            <Box
              sx={{
                display: "flex",
                justifyContent: "space-between",
                alignItems: "center",
              }}
            >
              <Box>
                <Typography variant="body2" gutterBottom>
                  Процент ошибок:{" "}
                  <strong>{(health.errorRate * 100).toFixed(2)}%</strong>
                </Typography>
                <Typography variant="body2">
                  Среднее время ответа:{" "}
                  <strong>{health.avgResponseTime}ms</strong>
                </Typography>
              </Box>
              <Box sx={{ textAlign: "center" }}>
                <Typography variant="body2" color="text.secondary" gutterBottom>
                  Health Score
                </Typography>
                {renderHealthScore()}
              </Box>
            </Box>
          </CardContent>
        </Card>

        {/* Configuration and Data Management */}
        <Grid container spacing={3} sx={{ mb: 3 }}>
          <Grid size={{ xs: 12, lg: 6 }}>
            <Card>
              <CardHeader title="Управление курсом" />
              <CardContent>
                <Grid container spacing={2} sx={{ mb: 2 }}>
                  <Grid size={{ xs: 6 }}>
                    <FormControl fullWidth>
                      <InputLabel>Базовая валюта</InputLabel>
                      <Select
                        value={selectedPair.from}
                        label="Базовая валюта"
                        onChange={(e) =>
                          setSelectedPair((prev) => ({
                            ...prev,
                            from: e.target.value,
                          }))
                        }
                      >
                        {mockCurrencies.map((currency) => (
                          <MenuItem key={currency} value={currency}>
                            {currency}
                          </MenuItem>
                        ))}
                      </Select>
                    </FormControl>
                  </Grid>
                  <Grid size={{ xs: 6 }}>
                    <FormControl fullWidth>
                      <InputLabel>Целевая валюта</InputLabel>
                      <Select
                        value={selectedPair.to}
                        label="Целевая валюта"
                        onChange={(e) =>
                          setSelectedPair((prev) => ({
                            ...prev,
                            to: e.target.value,
                          }))
                        }
                      >
                        {mockCurrencies.map((currency) => (
                          <MenuItem key={currency} value={currency}>
                            {currency}
                          </MenuItem>
                        ))}
                      </Select>
                    </FormControl>
                  </Grid>
                </Grid>

                <Paper sx={{ p: 2, bgcolor: "grey.100", mb: 2 }}>
                  <Typography variant="body2" color="text.secondary">
                    Текущий курс
                  </Typography>
                  <Typography variant="h4" fontWeight="bold">
                    {selectedPair.rate.toFixed(4)}
                  </Typography>
                  <Typography variant="caption" color="text.secondary">
                    Обновлено: {selectedPair.lastUpdated}
                  </Typography>
                </Paper>

                <Button variant="contained" onClick={forceUpdate} fullWidth>
                  Принудительно обновить
                </Button>
              </CardContent>
            </Card>
          </Grid>

          <Grid size={{ xs: 12, lg: 6 }}>
            <Card>
              <CardHeader title="Настройки провайдера" />
              <CardContent>
                <FormControl fullWidth sx={{ mb: 2 }}>
                  <InputLabel>Основной провайдер</InputLabel>
                  <Select
                    value={provider}
                    label="Основной провайдер"
                    onChange={(e) => setProvider(e.target.value)}
                  >
                    {mockProviders.map((prov) => (
                      <MenuItem key={prov} value={prov}>
                        {prov}
                      </MenuItem>
                    ))}
                  </Select>
                </FormControl>

                {provider === "OpenExchangeRates" && (
                  <TextField
                    fullWidth
                    type="password"
                    label="API ключ"
                    value={apiKey}
                    onChange={(e) => setApiKey(e.target.value)}
                    sx={{ mb: 2 }}
                  />
                )}

                <Box sx={{ display: "flex", gap: 1 }}>
                  <TextField
                    type="number"
                    label="Интервал обновления"
                    value={updateInterval}
                    onChange={(e) => setUpdateInterval(Number(e.target.value))}
                    sx={{ flex: 1 }}
                  />
                  <FormControl sx={{ minWidth: 120 }}>
                    <InputLabel>Единица</InputLabel>
                    <Select
                      value={intervalUnit}
                      label="Единица"
                      onChange={(e) => setIntervalUnit(e.target.value)}
                    >
                      <MenuItem value="minutes">минут</MenuItem>
                      <MenuItem value="hours">часов</MenuItem>
                    </Select>
                  </FormControl>
                </Box>
              </CardContent>
            </Card>
          </Grid>
        </Grid>

        {/* Cache Management */}
        <Card sx={{ mb: 3 }}>
          <CardHeader
            title={
              <Box sx={{ display: "flex", alignItems: "center", gap: 1 }}>
                <DatabaseIcon />
                <Typography variant="h6">Управление кэшем</Typography>
              </Box>
            }
          />
          <CardContent>
            <Grid container spacing={2} sx={{ mb: 3 }}>
              <Grid size={{ xs: 12, md: 4 }}>
                <StatCard title="Размер кэша" value="12.4 MB" />
              </Grid>
              <Grid size={{ xs: 12, md: 4 }}>
                <StatCard
                  title="Записей"
                  value={health.cacheCount.toString()}
                />
              </Grid>
              <Grid size={{ xs: 12, md: 4 }}>
                <StatCard title="Hit Rate" value="94.2%" />
              </Grid>
            </Grid>

            <Box sx={{ display: "flex", gap: 1 }}>
              <Button
                variant="outlined"
                onClick={warmCache}
                startIcon={<PlayIcon />}
              >
                Warm Cache
              </Button>
              <Button
                variant="outlined"
                onClick={clearCache}
                startIcon={<DeleteIcon />}
              >
                Очистить кэш
              </Button>
            </Box>
          </CardContent>
        </Card>

        {/* Testing & Sandbox */}
        <Card sx={{ mb: 3 }}>
          <CardHeader title="Тестирование и эмуляция" />
          <CardContent>
            <FormControlLabel
              control={
                <Switch
                  checked={manualOverride}
                  onChange={(e) => setManualOverride(e.target.checked)}
                />
              }
              label="Подменить курс вручную"
            />
            <Typography
              variant="caption"
              color="text.secondary"
              sx={{ display: "block", mb: 2 }}
            >
              Все запросы будут возвращать фиксированное значение
            </Typography>

            {manualOverride && (
              <TextField
                fullWidth
                type="number"
                label="Значение курса"
                value={overrideRate}
                onChange={(e) => setOverrideRate(e.target.value)}
                inputProps={{ step: "0.0001" }}
                sx={{ mb: 2 }}
              />
            )}

            <Divider sx={{ my: 2 }} />

            <FormControl fullWidth>
              <InputLabel>Режим эмуляции</InputLabel>
              <Select
                value={chaosMode}
                label="Режим эмуляции"
                onChange={(e) => setChaosMode(e.target.value)}
              >
                <MenuItem value="none">Нет</MenuItem>
                <MenuItem value="timeout">Таймаут</MenuItem>
                <MenuItem value="error500">Ошибка 500</MenuItem>
                <MenuItem value="random">Случайные ошибки</MenuItem>
              </Select>
            </FormControl>
          </CardContent>
        </Card>

        {/* API Documentation */}
        <Card sx={{ mb: 3 }}>
          <CardHeader
            title={
              <Box sx={{ display: "flex", alignItems: "center", gap: 1 }}>
                <FileTextIcon />
                <Typography variant="h6">Документация API</Typography>
              </Box>
            }
          />
          <CardContent>
            <Box sx={{ display: "flex", flexDirection: "column", gap: 1 }}>
              {mockApiEndpoints.map((endpoint, index) => {
                const isOpen = openEndpoints[index] || false;

                return (
                  <Paper key={index} variant="outlined">
                    <Box
                      sx={{
                        p: 2,
                        display: "flex",
                        alignItems: "center",
                        gap: 2,
                        cursor: "pointer",
                        "&:hover": { bgcolor: "action.hover" },
                      }}
                      onClick={() => toggleEndpoint(index)}
                    >
                      <Chip
                        label={endpoint.method}
                        color={
                          endpoint.method === "GET" ? "primary" : "secondary"
                        }
                        size="small"
                      />
                      <Typography
                        variant="body2"
                        fontFamily="monospace"
                        sx={{ flex: 1 }}
                      >
                        {endpoint.url}
                      </Typography>
                      <Typography
                        variant="body2"
                        color="text.secondary"
                        sx={{ flex: 2 }}
                      >
                        {endpoint.description}
                      </Typography>
                      <IconButton size="small">
                        <ExpandMoreIcon
                          sx={{ transform: isOpen ? "rotate(180deg)" : "none" }}
                        />
                      </IconButton>
                    </Box>
                    <Collapse in={isOpen}>
                      <Divider />
                      <Box sx={{ p: 2, bgcolor: "grey.50" }}>
                        <Typography variant="body2">
                          <strong>Параметры:</strong> {endpoint.parameters}
                        </Typography>
                      </Box>
                    </Collapse>
                  </Paper>
                );
              })}
            </Box>
          </CardContent>
        </Card>

        {/* Logs & Audit */}
        <Card>
          <CardHeader
            title={
              <Box sx={{ display: "flex", alignItems: "center", gap: 1 }}>
                <ClockIcon />
                <Typography variant="h6">Логи и аудит</Typography>
              </Box>
            }
          />
          <CardContent>
            <Box sx={{ display: "flex", alignItems: "center", gap: 2, mb: 2 }}>
              <Button
                variant="outlined"
                onClick={() => setShowLogs(!showLogs)}
                startIcon={<FileTextIcon />}
              >
                {showLogs ? "Скрыть логи" : "Показать логи"}
              </Button>
              <Typography variant="body2" color="text.secondary">
                Последнее обновление: {new Date().toLocaleString("ru-RU")}
              </Typography>
            </Box>

            <Collapse in={showLogs}>
              <Paper variant="outlined" sx={{ p: 2, bgcolor: "grey.50" }}>
                <Box
                  sx={{
                    fontFamily: "monospace",
                    fontSize: "0.875rem",
                    lineHeight: 1.5,
                  }}
                >
                  <Box sx={{ color: "success.main" }}>
                    [2025-01-20 14:23:15] INFO: Currency rates updated
                    successfully
                  </Box>
                  <Box sx={{ color: "info.main" }}>
                    [2025-01-20 14:22:58] DEBUG: Cache hit for USD/EUR pair
                  </Box>
                  <Box sx={{ color: "warning.main" }}>
                    [2025-01-20 14:20:12] WARN: High memory usage detected (85%)
                  </Box>
                  <Box sx={{ color: "text.secondary" }}>
                    [2025-01-20 14:18:45] INFO: Provider failover from
                    OpenExchangeRates to ЦБ РФ
                  </Box>
                  <Box sx={{ color: "success.main" }}>
                    [2025-01-20 14:15:30] INFO: Cache warmed with 47 currency
                    pairs
                  </Box>
                </Box>
              </Paper>
            </Collapse>
          </CardContent>
        </Card>
      </Box>
    </WithDevTools>
  );
};
