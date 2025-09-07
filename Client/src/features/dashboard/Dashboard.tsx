// pages/dashboard/DashboardPage.tsx
import React from "react";
import { Typography, Paper, Grid, Box } from "@mui/material";

export const DashboardPage: React.FC = () => {
  return (
    <Box
      sx={{
        width: "100%",
        height: "100%",
        p: 3, // Добавляем отступы внутри страницы
        boxSizing: "border-box",
      }}
    >
      <Typography variant="h4" gutterBottom>
        Дашборд
      </Typography>

      <Grid container spacing={3}>
        <Grid size={{ xs: 12, md: 6, lg: 4 }}>
          <Paper sx={{ p: 3, height: "100%" }}>
            <Typography variant="h6">Общий баланс</Typography>
            <Typography variant="h4">100 000 ₽</Typography>
          </Paper>
        </Grid>

        <Grid size={{ xs: 12, md: 6, lg: 4 }}>
          <Paper sx={{ p: 3, height: "100%" }}>
            <Typography variant="h6">Доходы</Typography>
            <Typography variant="h4" color="success.main">
              50 000 ₽
            </Typography>
          </Paper>
        </Grid>

        <Grid size={{ xs: 12, md: 6, lg: 4 }}>
          <Paper sx={{ p: 3, height: "100%" }}>
            <Typography variant="h6">Расходы</Typography>
            <Typography variant="h4" color="error.main">
              30 000 ₽
            </Typography>
          </Paper>
        </Grid>

        {/* График на всю ширину */}
        <Grid size={12}>
          <Paper sx={{ p: 3, height: 400 }}>
            <Typography variant="h6" gutterBottom>
              Динамика доходов и расходов
            </Typography>
            {/* Здесь будет график */}
            <Box
              sx={{
                width: "100%",
                height: 300,
                backgroundColor: "grey.100",
                display: "flex",
                alignItems: "center",
                justifyContent: "center",
                borderRadius: 1,
              }}
            >
              <Typography color="text.secondary">График будет здесь</Typography>
            </Box>
          </Paper>
        </Grid>
      </Grid>
    </Box>
  );
};
