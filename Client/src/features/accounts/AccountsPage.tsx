/* eslint-disable @typescript-eslint/no-explicit-any */
// pages/accounts/AccountsPage.tsx
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
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  TextField,
  FormControl,
  InputLabel,
  Select,
  MenuItem,
  IconButton,
  Menu,
  List,
  ListItem,
  ListItemText,
  ListItemIcon,
  Divider,
  Switch,
  FormControlLabel,
  useTheme,
  alpha,
} from "@mui/material";
import {
  Add as AddIcon,
  MoreHoriz as MoreHorizIcon,
  Edit as EditIcon,
  Delete as DeleteIcon,
  Visibility as VisibilityIcon,
  VisibilityOff as VisibilityOffIcon,
  CompareArrows as TransferIcon,
  CreditCard as CreditCardIcon,
  Wallet as WalletIcon,
  Business as BusinessIcon,
  Savings as SavingsIcon,
} from "@mui/icons-material";
import { useDevTools } from "../../contexts/DevToolsContext";
import { useDevMode } from "../../contexts";
import { useShiftF12 } from "../../hooks/useShiftF12";
import { WithDevTools } from "../../components/WithDevTools";

// Mock data
const accounts = [
  {
    id: "1",
    name: "Основная карта",
    type: "debit",
    balance: 125000,
    currency: "RUB",
    bank: "Сбербанк",
    lastFour: "1234",
    isDefault: true,
    transactions: [
      {
        date: "2025-01-08",
        description: "Покупки в супермаркете",
        amount: -2500,
      },
      {
        date: "2025-01-07",
        description: "Перевод с зарплатной карты",
        amount: 50000,
      },
      { date: "2025-01-06", description: "Заправка", amount: -1200 },
    ],
  },
  {
    id: "2",
    name: "Зарплатная карта",
    type: "salary",
    balance: 85000,
    currency: "RUB",
    bank: "ВТБ",
    lastFour: "5678",
    isDefault: false,
    transactions: [
      { date: "2025-01-07", description: "Зарплата за январь", amount: 95000 },
      {
        date: "2025-01-07",
        description: "Перевод на основную карту",
        amount: -50000,
      },
      { date: "2025-01-05", description: "Налог на доходы", amount: -12350 },
    ],
  },
  {
    id: "3",
    name: "Кредитная карта",
    type: "credit",
    balance: -15000,
    creditLimit: 200000,
    currency: "RUB",
    bank: "Тинькофф",
    lastFour: "9012",
    isDefault: false,
    transactions: [
      { date: "2025-01-05", description: "Кино с семьей", amount: -850 },
      { date: "2025-01-01", description: "Новогодний ужин", amount: -4500 },
      { date: "2024-12-30", description: "Покупка подарков", amount: -9650 },
    ],
  },
  {
    id: "4",
    name: "Накопительный счет",
    type: "savings",
    balance: 350000,
    currency: "RUB",
    bank: "Альфа-Банк",
    interestRate: 8.5,
    isDefault: false,
    transactions: [
      { date: "2025-01-01", description: "Начисление процентов", amount: 2458 },
      { date: "2024-12-15", description: "Пополнение счета", amount: 50000 },
      { date: "2024-12-01", description: "Начисление процентов", amount: 2398 },
    ],
  },
  {
    id: "5",
    name: "Наличные",
    type: "cash",
    balance: 12000,
    currency: "RUB",
    isDefault: false,
    transactions: [
      { date: "2025-01-02", description: "Подарок от мамы", amount: 5000 },
      { date: "2024-12-31", description: "Такси", amount: -500 },
      { date: "2024-12-30", description: "Снятие с банкомата", amount: 7500 },
    ],
  },
];

const accountTypeIcons = {
  debit: CreditCardIcon,
  credit: CreditCardIcon,
  salary: BusinessIcon,
  savings: SavingsIcon,
  cash: WalletIcon,
};

const accountTypeLabels = {
  debit: "Дебетовая карта",
  credit: "Кредитная карта",
  salary: "Зарплатная карта",
  savings: "Накопительный счет",
  cash: "Наличные",
};

const accountTypeColors = {
  debit: "success",
  credit: "error",
  salary: "primary",
  savings: "secondary",
  cash: "warning",
};

interface AccountCardProps {
  account: any;
  hideBalances: boolean;
  onMenuClick: (event: React.MouseEvent<HTMLElement>, account: any) => void;
}

const AccountCard: React.FC<AccountCardProps> = ({
  account,
  hideBalances,
  onMenuClick,
}) => {
  const theme = useTheme();
  const IconComponent =
    accountTypeIcons[account.type as keyof typeof accountTypeIcons];
  const [expanded, setExpanded] = useState(false);

  const formatCurrency = (amount: number) => {
    if (hideBalances) return "••••••";
    return new Intl.NumberFormat("ru-RU", {
      style: "currency",
      currency: "RUB",
      minimumFractionDigits: 0,
      maximumFractionDigits: 0,
    }).format(amount);
  };

  const formatDate = (dateString: string) => {
    return new Date(dateString).toLocaleDateString("ru-RU", {
      day: "numeric",
      month: "short",
    });
  };

  return (
    <Card>
      <CardHeader
        sx={{
          "&:hover": {
            backgroundColor: alpha(theme.palette.primary.main, 0.02),
          },
          cursor: "pointer",
        }}
        onClick={() => setExpanded(!expanded)}
        title={
          <Box sx={{ display: "flex", alignItems: "center", gap: 2 }}>
            <Box
              sx={{
                p: 1,
                borderRadius: 2,
                backgroundColor: `${
                  accountTypeColors[
                    account.type as keyof typeof accountTypeColors
                  ]
                }.main`,
                color: "white",
                display: "flex",
                alignItems: "center",
                justifyContent: "center",
              }}
            >
              <IconComponent sx={{ fontSize: 24 }} />
            </Box>

            <Box sx={{ flex: 1 }}>
              <Box
                sx={{ display: "flex", alignItems: "center", gap: 1, mb: 0.5 }}
              >
                <Typography variant="h6">{account.name}</Typography>
                {account.isDefault && (
                  <Chip label="Основной" size="small" color="primary" />
                )}
              </Box>

              <Typography variant="body2" color="text.secondary">
                {
                  accountTypeLabels[
                    account.type as keyof typeof accountTypeLabels
                  ]
                }
                {account.bank && ` • ${account.bank}`}
                {account.lastFour && ` •••• ${account.lastFour}`}
              </Typography>

              {account.type === "credit" && account.creditLimit && (
                <Typography variant="caption" color="text.secondary">
                  Лимит: {formatCurrency(account.creditLimit)}
                </Typography>
              )}

              {account.type === "savings" && account.interestRate && (
                <Typography variant="caption" color="text.secondary">
                  Ставка: {account.interestRate}% годовых
                </Typography>
              )}
            </Box>
          </Box>
        }
        action={
          <Box sx={{ display: "flex", alignItems: "center", gap: 1 }}>
            <Box sx={{ textAlign: "right", mr: 2 }}>
              <Typography
                variant="h6"
                color={account.balance >= 0 ? "success.main" : "error.main"}
                fontFamily="monospace"
              >
                {formatCurrency(account.balance)}
              </Typography>
              <Typography variant="caption" color="text.secondary">
                {account.currency}
              </Typography>
            </Box>

            <IconButton
              onClick={(e) => {
                e.stopPropagation();
                onMenuClick(e, account);
              }}
              size="small"
            >
              <MoreHorizIcon />
            </IconButton>
          </Box>
        }
      />

      {expanded && account.transactions && (
        <CardContent sx={{ pt: 0 }}>
          <Divider sx={{ mb: 2 }} />
          <Typography variant="subtitle2" gutterBottom>
            Последние операции
          </Typography>
          <List dense>
            {account.transactions.map((transaction: any, index: number) => (
              <ListItem key={index} sx={{ px: 0 }}>
                <ListItemIcon sx={{ minWidth: 32 }}>
                  <Box
                    sx={{
                      width: 8,
                      height: 8,
                      borderRadius: "50%",
                      backgroundColor:
                        transaction.amount >= 0 ? "success.main" : "error.main",
                    }}
                  />
                </ListItemIcon>
                <ListItemText
                  primary={transaction.description}
                  secondary={formatDate(transaction.date)}
                />
                <Typography
                  variant="body2"
                  color={
                    transaction.amount >= 0 ? "success.main" : "error.main"
                  }
                  fontFamily="monospace"
                >
                  {formatCurrency(transaction.amount)}
                </Typography>
              </ListItem>
            ))}
          </List>
        </CardContent>
      )}
    </Card>
  );
};

export const AccountsPage: React.FC = () => {
  const theme = useTheme();
  const { openDebugForm } = useDevTools();
  const { isDevMode } = useDevMode();

  const [hideBalances, setHideBalances] = useState(false);
  const [isAddDialogOpen, setIsAddDialogOpen] = useState(false);
  const [isTransferDialogOpen, setIsTransferDialogOpen] = useState(false);
  const [accountMenuAnchor, setAccountMenuAnchor] =
    useState<null | HTMLElement>(null);
  const [, setSelectedAccount] = useState<any>(null);

  // Интеграция с окном разработчика
  useShiftF12(() => {
    if (isDevMode) {
      openDebugForm();
    }
  }, [isDevMode, openDebugForm]);

  const formatCurrency = (amount: number) => {
    if (hideBalances) return "••••••";
    return new Intl.NumberFormat("ru-RU", {
      style: "currency",
      currency: "RUB",
      minimumFractionDigits: 0,
      maximumFractionDigits: 0,
    }).format(amount);
  };

  const handleAccountMenuClick = (
    event: React.MouseEvent<HTMLElement>,
    account: any
  ) => {
    setAccountMenuAnchor(event.currentTarget);
    setSelectedAccount(account);
  };

  const handleAccountMenuClose = () => {
    setAccountMenuAnchor(null);
    setSelectedAccount(null);
  };

  const totalBalance = accounts.reduce(
    (sum, account) => sum + account.balance,
    0
  );
  const totalAssets = accounts
    .filter((acc) => acc.balance > 0)
    .reduce((sum, acc) => sum + acc.balance, 0);
  const totalLiabilities = Math.abs(
    accounts
      .filter((acc) => acc.balance < 0)
      .reduce((sum, acc) => sum + acc.balance, 0)
  );

  // Вспомогательная функция для безопасного получения цвета
  const getSafeColor = (theme: any, colorName: string): string => {
    // Предопределенная карта для известных цветов
    const safeColorMap: { [key: string]: string } = {
      primary: theme.palette.primary.main,
      secondary: theme.palette.secondary.main,
      error: theme.palette.error.main,
      warning: theme.palette.warning.main,
      info: theme.palette.info.main,
      success: theme.palette.success.main,
    };

    // Если цвет есть в безопасной карте, используем его
    if (colorName in safeColorMap) {
      return safeColorMap[colorName];
    }

    // Иначе пытаемся получить из palette
    const paletteColor = theme.palette[colorName as keyof typeof theme.palette];

    if (
      typeof paletteColor === "object" &&
      paletteColor !== null &&
      "main" in paletteColor
    ) {
      return (paletteColor as any).main;
    }

    // Fallback
    return theme.palette.primary.main;
  };

  // Data for charts
  const distributionData = accounts.map((account) => {
    const colorKey =
      accountTypeColors[account.type as keyof typeof accountTypeColors];

    return {
      name: account.name,
      value: Math.abs(account.balance),
      color: getSafeColor(theme, colorKey),
    };
  });

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
    <Card sx={{ height: "100%" }}>
      <CardContent>
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
    <WithDevTools componentName="Accounts">
      <Box sx={{ p: 3 }}>
        {/* Header */}
        <Box
          sx={{
            display: "flex",
            alignItems: "center",
            justifyContent: "space-between",
            mb: 4,
          }}
        >
          <Box>
            <Typography variant="h4" component="h1" gutterBottom>
              Счета
            </Typography>
            <Typography variant="body1" color="text.secondary">
              Управление всеми счетами и балансами
            </Typography>
          </Box>

          <Box sx={{ display: "flex", gap: 1 }}>
            <FormControlLabel
              control={
                <Switch
                  checked={hideBalances}
                  onChange={(e) => setHideBalances(e.target.checked)}
                />
              }
              label={
                <Box sx={{ display: "flex", alignItems: "center", gap: 1 }}>
                  {hideBalances ? <VisibilityIcon /> : <VisibilityOffIcon />}
                  {hideBalances ? "Показать" : "Скрыть"} балансы
                </Box>
              }
            />

            <Button
              variant="outlined"
              onClick={() => setIsTransferDialogOpen(true)}
              startIcon={<TransferIcon />}
            >
              Перевод
            </Button>

            <Button
              variant="contained"
              onClick={() => setIsAddDialogOpen(true)}
              startIcon={<AddIcon />}
              sx={{
                backgroundColor: "success.main",
                "&:hover": {
                  backgroundColor: "success.dark",
                },
              }}
            >
              Добавить счет
            </Button>
          </Box>
        </Box>

        {/* Summary Cards */}
        <Grid container spacing={3} sx={{ mb: 4 }}>
          <Grid size={{ xs: 12, md: 4 }}>
            <StatCard
              title="Общий баланс"
              value={formatCurrency(totalBalance)}
              subtitle="По всем счетам"
              color={totalBalance >= 0 ? "success" : "error"}
            />
          </Grid>
          <Grid size={{ xs: 12, md: 4 }}>
            <StatCard
              title="Активы"
              value={formatCurrency(totalAssets)}
              subtitle="Положительный баланс"
              color="success"
            />
          </Grid>
          <Grid size={{ xs: 12, md: 4 }}>
            <StatCard
              title="Обязательства"
              value={formatCurrency(totalLiabilities)}
              subtitle="Кредитные задолженности"
              color="error"
            />
          </Grid>
        </Grid>

        <Grid container spacing={3}>
          {/* Account List */}
          <Grid size={{ xs: 12, lg: 8 }}>
            <Typography variant="h5" gutterBottom>
              Список счетов
            </Typography>

            <Box sx={{ display: "flex", flexDirection: "column", gap: 2 }}>
              {accounts.map((account) => (
                <AccountCard
                  key={account.id}
                  account={account}
                  hideBalances={hideBalances}
                  onMenuClick={handleAccountMenuClick}
                />
              ))}
            </Box>
          </Grid>

          {/* Analytics Sidebar */}
          <Grid size={{ xs: 12, lg: 4 }}>
            <Box sx={{ display: "flex", flexDirection: "column", gap: 3 }}>
              {/* Distribution Chart */}
              <Card>
                <CardHeader title="Распределение средств" />
                <CardContent>
                  <Box
                    sx={{ display: "flex", flexDirection: "column", gap: 2 }}
                  >
                    {distributionData.map((account) => {
                      const total = distributionData.reduce(
                        (sum, acc) => sum + acc.value,
                        0
                      );
                      const percentage = (
                        (account.value / total) *
                        100
                      ).toFixed(1);

                      return (
                        <Box
                          key={account.name}
                          sx={{
                            display: "flex",
                            flexDirection: "column",
                            gap: 1,
                          }}
                        >
                          <Box
                            sx={{
                              display: "flex",
                              justifyContent: "space-between",
                            }}
                          >
                            <Typography variant="body2">
                              {account.name}
                            </Typography>
                            <Typography variant="body2">
                              {percentage}%
                            </Typography>
                          </Box>
                          <Box
                            sx={{
                              width: "100%",
                              bgcolor: "grey.200",
                              borderRadius: 1,
                              height: 8,
                            }}
                          >
                            <Box
                              sx={{
                                height: 8,
                                borderRadius: 1,
                                backgroundColor: account.color,
                                width: `${percentage}%`,
                                transition: "width 0.3s ease",
                              }}
                            />
                          </Box>
                        </Box>
                      );
                    })}
                  </Box>
                </CardContent>
              </Card>

              {/* Quick Stats */}
              <Card>
                <CardHeader title="Статистика" />
                <CardContent>
                  <Box
                    sx={{ display: "flex", flexDirection: "column", gap: 2 }}
                  >
                    <Box
                      sx={{ display: "flex", justifyContent: "space-between" }}
                    >
                      <Typography variant="body2" color="text.secondary">
                        Количество счетов
                      </Typography>
                      <Typography variant="body2" fontWeight="bold">
                        {accounts.length}
                      </Typography>
                    </Box>
                    <Box
                      sx={{ display: "flex", justifyContent: "space-between" }}
                    >
                      <Typography variant="body2" color="text.secondary">
                        Основная валюта
                      </Typography>
                      <Typography variant="body2" fontWeight="bold">
                        RUB
                      </Typography>
                    </Box>
                    <Box
                      sx={{ display: "flex", justifyContent: "space-between" }}
                    >
                      <Typography variant="body2" color="text.secondary">
                        Средний баланс
                      </Typography>
                      <Typography variant="body2" fontWeight="bold">
                        {formatCurrency(totalBalance / accounts.length)}
                      </Typography>
                    </Box>
                    <Box
                      sx={{ display: "flex", justifyContent: "space-between" }}
                    >
                      <Typography variant="body2" color="text.secondary">
                        Кредитная нагрузка
                      </Typography>
                      <Typography variant="body2" fontWeight="bold">
                        {((totalLiabilities / totalAssets) * 100).toFixed(1)}%
                      </Typography>
                    </Box>
                  </Box>
                </CardContent>
              </Card>
            </Box>
          </Grid>
        </Grid>

        {/* Add Account Dialog */}
        <Dialog
          open={isAddDialogOpen}
          onClose={() => setIsAddDialogOpen(false)}
          maxWidth="sm"
          fullWidth
        >
          <DialogTitle>Новый счет</DialogTitle>
          <DialogContent>
            <Box
              sx={{ display: "flex", flexDirection: "column", gap: 3, mt: 1 }}
            >
              <TextField
                label="Название счета"
                placeholder="Например: Основная карта"
                fullWidth
              />

              <Box sx={{ display: "flex", gap: 2 }}>
                <FormControl fullWidth>
                  <InputLabel>Тип счета</InputLabel>
                  <Select label="Тип счета">
                    <MenuItem value="debit">Дебетовая карта</MenuItem>
                    <MenuItem value="credit">Кредитная карта</MenuItem>
                    <MenuItem value="savings">Накопительный счет</MenuItem>
                    <MenuItem value="cash">Наличные</MenuItem>
                  </Select>
                </FormControl>

                <TextField
                  label="Начальный баланс"
                  type="number"
                  placeholder="0"
                  fullWidth
                />
              </Box>

              <Box sx={{ display: "flex", gap: 2 }}>
                <TextField
                  label="Банк"
                  placeholder="Название банка"
                  fullWidth
                />

                <FormControl fullWidth>
                  <InputLabel>Валюта</InputLabel>
                  <Select defaultValue="RUB" label="Валюта">
                    <MenuItem value="RUB">₽ Рубль</MenuItem>
                    <MenuItem value="USD">$ Доллар</MenuItem>
                    <MenuItem value="EUR">€ Евро</MenuItem>
                  </Select>
                </FormControl>
              </Box>
            </Box>
          </DialogContent>
          <DialogActions>
            <Button onClick={() => setIsAddDialogOpen(false)}>Отмена</Button>
            <Button
              variant="contained"
              sx={{
                backgroundColor: "success.main",
                "&:hover": {
                  backgroundColor: "success.dark",
                },
              }}
            >
              Добавить счет
            </Button>
          </DialogActions>
        </Dialog>

        {/* Transfer Dialog */}
        <Dialog
          open={isTransferDialogOpen}
          onClose={() => setIsTransferDialogOpen(false)}
          maxWidth="sm"
          fullWidth
        >
          <DialogTitle>Перевод между счетами</DialogTitle>
          <DialogContent>
            <Box
              sx={{ display: "flex", flexDirection: "column", gap: 3, mt: 1 }}
            >
              <Box sx={{ display: "flex", gap: 2 }}>
                <FormControl fullWidth>
                  <InputLabel>Со счета</InputLabel>
                  <Select label="Со счета">
                    {accounts.map((account) => (
                      <MenuItem key={account.id} value={account.id}>
                        {account.name} - {formatCurrency(account.balance)}
                      </MenuItem>
                    ))}
                  </Select>
                </FormControl>

                <FormControl fullWidth>
                  <InputLabel>На счет</InputLabel>
                  <Select label="На счет">
                    {accounts.map((account) => (
                      <MenuItem key={account.id} value={account.id}>
                        {account.name} - {formatCurrency(account.balance)}
                      </MenuItem>
                    ))}
                  </Select>
                </FormControl>
              </Box>

              <TextField
                label="Сумма перевода"
                type="number"
                placeholder="0"
                fullWidth
              />

              <TextField
                label="Комментарий"
                placeholder="Описание перевода (необязательно)"
                fullWidth
              />
            </Box>
          </DialogContent>
          <DialogActions>
            <Button onClick={() => setIsTransferDialogOpen(false)}>
              Отмена
            </Button>
            <Button
              variant="contained"
              sx={{
                backgroundColor: "success.main",
                "&:hover": {
                  backgroundColor: "success.dark",
                },
              }}
            >
              Выполнить перевод
            </Button>
          </DialogActions>
        </Dialog>

        {/* Account Menu */}
        <Menu
          anchorEl={accountMenuAnchor}
          open={Boolean(accountMenuAnchor)}
          onClose={handleAccountMenuClose}
        >
          <MenuItem onClick={handleAccountMenuClose}>
            <ListItemIcon>
              <EditIcon fontSize="small" />
            </ListItemIcon>
            <ListItemText>Редактировать</ListItemText>
          </MenuItem>
          <MenuItem
            onClick={handleAccountMenuClose}
            sx={{ color: "error.main" }}
          >
            <ListItemIcon>
              <DeleteIcon fontSize="small" color="error" />
            </ListItemIcon>
            <ListItemText>Удалить</ListItemText>
          </MenuItem>
        </Menu>
      </Box>
    </WithDevTools>
  );
};
