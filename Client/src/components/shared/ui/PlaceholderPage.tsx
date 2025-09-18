import { Card, CardContent, Box, Typography } from "@mui/material";
import React from "react";

interface PlaceholderProps {
  title: string;
  description: string;
}

export const PlaceholderPage: React.FC<PlaceholderProps> = ({
  title,
  description,
}) => {
  return (
    <Box
      sx={{
        width: "100%", // Занимает всю ширину
        height: "100%", // Занимает всю высоту
        p: 3, // Используем MUI систему вместо inline styles
        boxSizing: "border-box", // Важно для правильного расчета размеров
      }}
    >
      <Box sx={{ mb: 3 }}>
        <Typography variant="h4" gutterBottom>
          {title}
        </Typography>
        <Typography variant="body1" color="textSecondary">
          {description}
        </Typography>
      </Box>

      <Card
        sx={{
          width: "100%", // Карточка на всю ширину
          maxWidth: "none", // Убираем максимальную ширину
        }}
      >
        <CardContent
          sx={{
            p: 6, // Используем MUI систему
            textAlign: "center",
          }}
        >
          <Box
            sx={{
              width: 64,
              height: 64,
              margin: "0 auto 16px auto",
              backgroundColor: "grey.100",
              borderRadius: "50%",
              display: "flex",
              alignItems: "center",
              justifyContent: "center",
            }}
          >
            <Box
              sx={{
                width: 32,
                height: 32,
                backgroundColor: "grey.300",
                borderRadius: "50%",
              }}
            />
          </Box>
          <Typography variant="h6" gutterBottom>
            Раздел в разработке
          </Typography>
          <Typography variant="body2" color="textSecondary">
            Этот раздел будет реализован в следующих версиях приложения
          </Typography>
        </CardContent>
      </Card>
    </Box>
  );
};
