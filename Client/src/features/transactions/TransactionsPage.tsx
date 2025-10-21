// pages/transactions/TransactionsPage.tsx
import React, { useState } from "react";
import {
  Box,
  Typography,
  Grid,
  Card,
  CardContent,
  Button,
  TextField,
  Chip,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Checkbox,
  FormControl,
  InputLabel,
  Select,
  MenuItem,
  IconButton,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  InputAdornment,
  TableSortLabel,
  Tooltip,
  Menu,
  Alert,
} from "@mui/material";
import {
  Add as AddIcon,
  Download as DownloadIcon,
  FilterList as FilterIcon,
  Search as SearchIcon,
  MoreVert as MoreVertIcon,
  Edit as EditIcon,
  Delete as DeleteIcon,
} from "@mui/icons-material";
import { useDevLogger } from "../../hooks";
import { WithDevTools } from "../../components/WithDevTools";
import { useShiftF12 } from "../../hooks/useShiftF12";
import { useDevTools } from "../../contexts/DevToolsContext";
import { useDevMode } from "../../contexts";

interface Transaction {
  id: string;
  date: string;
  amount: number;
  category: string;
  account: string;
  counterparty: string;
  description: string;
  tags: string[];
}

// Mock data
const transactions: Transaction[] = [
  {
    id: "1",
    date: "2025-01-08",
    amount: -2500,
    category: "Продукты",
    account: "Дебетовая карта",
    counterparty: "Перекресток",
    description: "Покупки в супермаркете",
    tags: ["еда", "семья"],
  },
  {
    id: "2",
    date: "2025-01-07",
    amount: 95000,
    category: "Зарплата",
    account: "Зарплатная карта",
    counterparty: "ООО Рога и Копыта",
    description: "Зарплата за январь",
    tags: ["работа", "доход"],
  },
  {
    id: "3",
    date: "2025-01-06",
    amount: -1200,
    category: "Транспорт",
    account: "Дебетовая карта",
    counterparty: "Лукойл",
    description: "Заправка автомобиля",
    tags: ["авто", "топливо"],
  },
  {
    id: "4",
    date: "2025-01-05",
    amount: -850,
    category: "Развлечения",
    account: "Кредитная карта",
    counterparty: "Кинотеатр ИMAX",
    description: "Кино с семьей",
    tags: ["семья", "развлечения"],
  },
  {
    id: "5",
    date: "2025-01-04",
    amount: -3200,
    category: "Коммунальные",
    account: "Дебетовая карта",
    counterparty: "Ростелеком",
    description: "Оплата интернета и ТВ",
    tags: ["дом", "регулярные"],
  },
  {
    id: "6",
    date: "2025-01-03",
    amount: -15000,
    category: "Здоровье",
    account: "Дебетовая карта",
    counterparty: "Поликлиника №1",
    description: "Медицинский осмотр",
    tags: ["здоровье", "медицина"],
  },
  {
    id: "7",
    date: "2025-01-02",
    amount: 5000,
    category: "Подарки",
    account: "Наличные",
    counterparty: "Мама",
    description: "Подарок на Новый год",
    tags: ["семья", "подарок"],
  },
  {
    id: "8",
    date: "2025-01-01",
    amount: -4500,
    category: "Развлечения",
    account: "Кредитная карта",
    counterparty: "Ресторан",
    description: "Новогодний ужин",
    tags: ["праздник", "ресторан"],
  },
];

const categories = [
  "Все",
  "Продукты",
  "Транспорт",
  "Развлечения",
  "Коммунальные",
  "Зарплата",
  "Здоровье",
  "Подарки",
];
const accounts = [
  "Все",
  "Дебетовая карта",
  "Кредитная карта",
  "Зарплатная карта",
  "Наличные",
];

export const TransactionsPage: React.FC = () => {
  const { devLog } = useDevLogger();
  const { openDebugForm } = useDevTools();
  const { isDevMode } = useDevMode();

  const [selectedTransactions, setSelectedTransactions] = useState<string[]>(
    []
  );
  const [searchQuery, setSearchQuery] = useState("");
  const [selectedCategory, setSelectedCategory] = useState("Все");
  const [selectedAccount, setSelectedAccount] = useState("Все");
  const [isAddDialogOpen, setIsAddDialogOpen] = useState(false);
  const [sortField, setSortField] = useState<"date" | "amount" | "category">(
    "date"
  );
  const [sortDirection, setSortDirection] = useState<"asc" | "desc">("desc");
  const [contextMenu, setContextMenu] = useState<{
    transaction: Transaction | null;
    anchorEl: HTMLElement | null;
  }>({ transaction: null, anchorEl: null });

  // Горячие клавиши для разработчика
  useShiftF12(() => {
    if (isDevMode) {
      openDebugForm();
      devLog("Debug form opened from Transactions");
    }
  }, [isDevMode, openDebugForm]);

  const formatCurrency = (amount: number) => {
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
      year: "numeric",
    });
  };

  const handleSelectAll = (event: React.ChangeEvent<HTMLInputElement>) => {
    if (event.target.checked) {
      setSelectedTransactions(filteredTransactions.map((t) => t.id));
      devLog("All transactions selected", {
        count: filteredTransactions.length,
      });
    } else {
      setSelectedTransactions([]);
      devLog("All transactions deselected");
    }
  };

  const handleSelectTransaction = (id: string, checked: boolean) => {
    if (checked) {
      setSelectedTransactions((prev) => [...prev, id]);
    } else {
      setSelectedTransactions((prev) => prev.filter((t) => t !== id));
    }
  };

  const handleSort = (field: "date" | "amount" | "category") => {
    if (sortField === field) {
      setSortDirection((prev) => (prev === "asc" ? "desc" : "asc"));
    } else {
      setSortField(field);
      setSortDirection("desc");
    }
    devLog("Transactions sorted", { field, direction: sortDirection });
  };

  const handleContextMenu = (
    event: React.MouseEvent,
    transaction: Transaction
  ) => {
    event.preventDefault();
    setContextMenu({
      transaction,
      anchorEl: event.currentTarget as HTMLElement,
    });
  };

  const handleCloseContextMenu = () => {
    setContextMenu({ transaction: null, anchorEl: null });
  };

  // Filter and sort transactions
  const filteredTransactions = transactions.filter((transaction) => {
    const matchesSearch =
      transaction.description
        .toLowerCase()
        .includes(searchQuery.toLowerCase()) ||
      transaction.counterparty
        .toLowerCase()
        .includes(searchQuery.toLowerCase());
    const matchesCategory =
      selectedCategory === "Все" || transaction.category === selectedCategory;
    const matchesAccount =
      selectedAccount === "Все" || transaction.account === selectedAccount;

    return matchesSearch && matchesCategory && matchesAccount;
  });

  // Sort transactions
  filteredTransactions.sort((a, b) => {
    let comparison = 0;

    switch (sortField) {
      case "date":
        comparison = new Date(a.date).getTime() - new Date(b.date).getTime();
        break;
      case "amount":
        comparison = a.amount - b.amount;
        break;
      case "category":
        comparison = a.category.localeCompare(b.category);
        break;
    }

    return sortDirection === "asc" ? comparison : -comparison;
  });

  const totalIncome = transactions
    .filter((t) => t.amount > 0)
    .reduce((sum, t) => sum + t.amount, 0);
  const totalExpense = transactions
    .filter((t) => t.amount < 0)
    .reduce((sum, t) => sum + Math.abs(t.amount), 0);
  const balance = totalIncome - totalExpense;

  const StatCard = ({
    title,
    value,
    color = "primary",
    isCurrency = true,
  }: {
    title: string;
    value: number;
    color?: "primary" | "secondary" | "error" | "warning" | "info" | "success";
    isCurrency?: boolean;
  }) => (
    <Card>
      <CardContent>
        <Typography variant="body2" color="text.secondary" gutterBottom>
          {title}
        </Typography>
        <Typography
          variant="h5"
          color={`${color}.main`}
          fontWeight="bold"
          fontFamily="monospace"
        >
          {isCurrency ? formatCurrency(value) : value}
        </Typography>
      </CardContent>
    </Card>
  );

  return (
    <WithDevTools componentName="TransactionsPage">
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
            <Typography
              variant="h4"
              component="h1"
              fontWeight="bold"
              gutterBottom
            >
              Транзакции
            </Typography>
            <Typography variant="body1" color="text.secondary">
              Управление всеми доходами и расходами
            </Typography>
          </Box>

          <Box sx={{ display: "flex", gap: 1 }}>
            <Button variant="outlined" startIcon={<DownloadIcon />}>
              Экспорт
            </Button>

            <Button
              variant="contained"
              startIcon={<AddIcon />}
              onClick={() => setIsAddDialogOpen(true)}
              sx={{
                backgroundColor: "success.main",
                "&:hover": { backgroundColor: "success.dark" },
              }}
            >
              Добавить транзакцию
            </Button>
          </Box>
        </Box>

        {/* Stats Cards */}
        <Grid container spacing={3} sx={{ mb: 4 }}>
          <Grid size={{ xs: 12, md: 4 }}>
            <StatCard title="Общий доход" value={totalIncome} color="success" />
          </Grid>
          <Grid size={{ xs: 12, md: 4 }}>
            <StatCard title="Общий расход" value={totalExpense} color="error" />
          </Grid>
          <Grid size={{ xs: 12, md: 4 }}>
            <StatCard
              title="Разность"
              value={balance}
              color={balance >= 0 ? "success" : "error"}
            />
          </Grid>
        </Grid>

        {/* Filters */}
        <Card sx={{ mb: 3 }}>
          <CardContent sx={{ p: 3 }}>
            <Grid container spacing={2} alignItems="center">
              <Grid size={{ xs: 12, md: 4 }}>
                <TextField
                  fullWidth
                  placeholder="Поиск по описанию или контрагенту..."
                  value={searchQuery}
                  onChange={(e) => setSearchQuery(e.target.value)}
                  InputProps={{
                    startAdornment: (
                      <InputAdornment position="start">
                        <SearchIcon color="action" />
                      </InputAdornment>
                    ),
                  }}
                />
              </Grid>

              <Grid size={{ xs: 12, md: 2 }}>
                <FormControl fullWidth>
                  <InputLabel>Категория</InputLabel>
                  <Select
                    value={selectedCategory}
                    label="Категория"
                    onChange={(e) => setSelectedCategory(e.target.value)}
                  >
                    {categories.map((cat) => (
                      <MenuItem key={cat} value={cat}>
                        {cat}
                      </MenuItem>
                    ))}
                  </Select>
                </FormControl>
              </Grid>

              <Grid size={{ xs: 12, md: 2 }}>
                <FormControl fullWidth>
                  <InputLabel>Счет</InputLabel>
                  <Select
                    value={selectedAccount}
                    label="Счет"
                    onChange={(e) => setSelectedAccount(e.target.value)}
                  >
                    {accounts.map((acc) => (
                      <MenuItem key={acc} value={acc}>
                        {acc}
                      </MenuItem>
                    ))}
                  </Select>
                </FormControl>
              </Grid>

              <Grid size={{ xs: 12, md: 2 }}>
                <Button variant="outlined" fullWidth startIcon={<FilterIcon />}>
                  Фильтры
                </Button>
              </Grid>

              {/* Dev Mode Indicator */}
              {isDevMode && (
                <Grid size={{ xs: 12, md: 2 }}>
                  <Alert severity="info" sx={{ py: 0.5 }}>
                    <Typography variant="caption">
                      Dev Mode: Shift+F12
                    </Typography>
                  </Alert>
                </Grid>
              )}
            </Grid>
          </CardContent>
        </Card>

        {/* Bulk Actions */}
        {selectedTransactions.length > 0 && (
          <Card sx={{ mb: 3 }}>
            <CardContent sx={{ p: 2 }}>
              <Box
                sx={{
                  display: "flex",
                  alignItems: "center",
                  justifyContent: "space-between",
                }}
              >
                <Typography variant="body2">
                  Выбрано транзакций:{" "}
                  <strong>{selectedTransactions.length}</strong>
                </Typography>
                <Box sx={{ display: "flex", gap: 1 }}>
                  <Button
                    variant="outlined"
                    size="small"
                    startIcon={<EditIcon />}
                  >
                    Редактировать
                  </Button>
                  <Button
                    variant="outlined"
                    size="small"
                    startIcon={<DeleteIcon />}
                    color="error"
                  >
                    Удалить
                  </Button>
                </Box>
              </Box>
            </CardContent>
          </Card>
        )}

        {/* Transactions Table */}
        <Card>
          <TableContainer>
            <Table>
              <TableHead>
                <TableRow>
                  <TableCell padding="checkbox">
                    <Checkbox
                      indeterminate={
                        selectedTransactions.length > 0 &&
                        selectedTransactions.length <
                          filteredTransactions.length
                      }
                      checked={
                        filteredTransactions.length > 0 &&
                        selectedTransactions.length ===
                          filteredTransactions.length
                      }
                      onChange={handleSelectAll}
                    />
                  </TableCell>
                  <TableCell>
                    <TableSortLabel
                      active={sortField === "date"}
                      direction={sortDirection}
                      onClick={() => handleSort("date")}
                    >
                      Дата
                    </TableSortLabel>
                  </TableCell>
                  <TableCell>
                    <TableSortLabel
                      active={sortField === "amount"}
                      direction={sortDirection}
                      onClick={() => handleSort("amount")}
                    >
                      Сумма
                    </TableSortLabel>
                  </TableCell>
                  <TableCell>
                    <TableSortLabel
                      active={sortField === "category"}
                      direction={sortDirection}
                      onClick={() => handleSort("category")}
                    >
                      Категория
                    </TableSortLabel>
                  </TableCell>
                  <TableCell>Счет</TableCell>
                  <TableCell>Контрагент</TableCell>
                  <TableCell>Описание</TableCell>
                  <TableCell align="center">Действия</TableCell>
                </TableRow>
              </TableHead>
              <TableBody>
                {filteredTransactions.map((transaction) => (
                  <TableRow
                    key={transaction.id}
                    hover
                    onContextMenu={(e) => handleContextMenu(e, transaction)}
                    sx={{
                      "&:last-child td, &:last-child th": { border: 0 },
                      cursor: "context-menu",
                    }}
                  >
                    <TableCell padding="checkbox">
                      <Checkbox
                        checked={selectedTransactions.includes(transaction.id)}
                        onChange={(e) =>
                          handleSelectTransaction(
                            transaction.id,
                            e.target.checked
                          )
                        }
                      />
                    </TableCell>
                    <TableCell>{formatDate(transaction.date)}</TableCell>
                    <TableCell>
                      <Typography
                        variant="body2"
                        color={
                          transaction.amount >= 0
                            ? "success.main"
                            : "error.main"
                        }
                        fontFamily="monospace"
                        fontWeight="medium"
                      >
                        {formatCurrency(transaction.amount)}
                      </Typography>
                    </TableCell>
                    <TableCell>
                      <Chip
                        label={transaction.category}
                        variant="outlined"
                        size="small"
                        color={
                          transaction.amount >= 0
                            ? "success"
                            : transaction.category === "Продукты"
                            ? "primary"
                            : transaction.category === "Транспорт"
                            ? "secondary"
                            : transaction.category === "Развлечения"
                            ? "warning"
                            : "default"
                        }
                      />
                    </TableCell>
                    <TableCell>
                      <Typography variant="body2" color="text.secondary">
                        {transaction.account}
                      </Typography>
                    </TableCell>
                    <TableCell>{transaction.counterparty}</TableCell>
                    <TableCell>
                      <Tooltip title={transaction.description}>
                        <Typography
                          variant="body2"
                          sx={{
                            maxWidth: 200,
                            overflow: "hidden",
                            textOverflow: "ellipsis",
                            whiteSpace: "nowrap",
                          }}
                        >
                          {transaction.description}
                        </Typography>
                      </Tooltip>
                    </TableCell>
                    <TableCell align="center">
                      <IconButton
                        size="small"
                        onClick={(e) => handleContextMenu(e, transaction)}
                      >
                        <MoreVertIcon />
                      </IconButton>
                    </TableCell>
                  </TableRow>
                ))}
              </TableBody>
            </Table>
          </TableContainer>

          {filteredTransactions.length === 0 && (
            <Box sx={{ p: 8, textAlign: "center" }}>
              <SearchIcon
                sx={{ fontSize: 64, color: "text.secondary", mb: 2 }}
              />
              <Typography variant="h6" gutterBottom>
                Транзакции не найдены
              </Typography>
              <Typography variant="body2" color="text.secondary" sx={{ mb: 3 }}>
                Попробуйте изменить фильтры или добавить новую транзакцию
              </Typography>
              <Button
                variant="contained"
                startIcon={<AddIcon />}
                onClick={() => setIsAddDialogOpen(true)}
                sx={{
                  backgroundColor: "success.main",
                  "&:hover": { backgroundColor: "success.dark" },
                }}
              >
                Добавить транзакцию
              </Button>
            </Box>
          )}
        </Card>

        {/* Context Menu */}
        <Menu
          open={Boolean(contextMenu.anchorEl)}
          anchorEl={contextMenu.anchorEl}
          onClose={handleCloseContextMenu}
        >
          <MenuItem onClick={handleCloseContextMenu}>
            <EditIcon sx={{ mr: 1 }} />
            Редактировать
          </MenuItem>
          <MenuItem
            onClick={handleCloseContextMenu}
            sx={{ color: "error.main" }}
          >
            <DeleteIcon sx={{ mr: 1 }} />
            Удалить
          </MenuItem>
        </Menu>

        {/* Add Transaction Dialog */}
        <Dialog
          open={isAddDialogOpen}
          onClose={() => setIsAddDialogOpen(false)}
          maxWidth="md"
          fullWidth
        >
          <DialogTitle>Новая транзакция</DialogTitle>
          <DialogContent>
            <Grid container spacing={2} sx={{ mt: 1 }}>
              <Grid size={{ xs: 6 }}>
                <TextField
                  fullWidth
                  label="Сумма"
                  type="number"
                  placeholder="0"
                />
              </Grid>
              <Grid size={{ xs: 6 }}>
                <FormControl fullWidth>
                  <InputLabel>Тип</InputLabel>
                  <Select label="Тип">
                    <MenuItem value="income">Доход</MenuItem>
                    <MenuItem value="expense">Расход</MenuItem>
                  </Select>
                </FormControl>
              </Grid>

              <Grid size={{ xs: 6 }}>
                <FormControl fullWidth>
                  <InputLabel>Категория</InputLabel>
                  <Select label="Категория">
                    {categories.slice(1).map((cat) => (
                      <MenuItem key={cat} value={cat}>
                        {cat}
                      </MenuItem>
                    ))}
                  </Select>
                </FormControl>
              </Grid>
              <Grid size={{ xs: 6 }}>
                <FormControl fullWidth>
                  <InputLabel>Счет</InputLabel>
                  <Select label="Счет">
                    {accounts.slice(1).map((acc) => (
                      <MenuItem key={acc} value={acc}>
                        {acc}
                      </MenuItem>
                    ))}
                  </Select>
                </FormControl>
              </Grid>

              <Grid size={{ xs: 12 }}>
                <TextField
                  fullWidth
                  label="Контрагент"
                  placeholder="Название организации или лица"
                />
              </Grid>

              <Grid size={{ xs: 12 }}>
                <TextField
                  fullWidth
                  label="Описание"
                  placeholder="Описание транзакции"
                  multiline
                  rows={3}
                />
              </Grid>
            </Grid>
          </DialogContent>
          <DialogActions>
            <Button onClick={() => setIsAddDialogOpen(false)}>Отмена</Button>
            <Button
              variant="contained"
              sx={{
                backgroundColor: "success.main",
                "&:hover": { backgroundColor: "success.dark" },
              }}
            >
              Сохранить
            </Button>
          </DialogActions>
        </Dialog>
      </Box>
    </WithDevTools>
  );
};
