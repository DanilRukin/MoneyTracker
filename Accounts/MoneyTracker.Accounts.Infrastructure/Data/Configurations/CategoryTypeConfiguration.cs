using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MoneyTracker.Accounts.Domain.Categories;
using MoneyTracker.Accounts.Infrastructure.Data;

internal class CategoryEntityTypeConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable(TableNames.Categories, SchemaName.Name);

        builder.Property<int>("Id")
            .ValueGeneratedOnAdd();

        builder.HasKey("Id");

        builder.Property(c => c.Name)
            .HasMaxLength(100)
            .IsRequired();

        // Создаем теневое свойство для дискриминатора
        builder.Property<string>("CategoryType")
            .HasMaxLength(20)
            .IsRequired();

        // TPH настройка использует теневое свойство
        builder.HasDiscriminator<string>("CategoryType")
            .HasValue<IncomeCategory>("Income")
            .HasValue<ExpenseCategory>("Expense")
            .HasValue<TransferIncomeCategory>("TransferIncome")
            .HasValue<TransferExpenseCategory>("TransferExpense");

        // используем теневое свойство в индексе
        builder.HasIndex("Name", "CategoryType")
            .IsUnique();

        //builder.Ignore("Id");

        // Seed данные
        builder.HasData(
            new { Name = "Перевод между счетами", CategoryType = "TransferIncome", Id = 1 },
            new { Name = "Перевод между счетами", CategoryType = "TransferExpense", Id = 2 },
            new { Name = "Зарплата", CategoryType = "Income", Id = 3 },
            new { Name = "Инвестиции", CategoryType = "Income", Id = 4 },
            new { Name = "Продукты", CategoryType = "Expense", Id = 5 },
            new { Name = "Транспорт", CategoryType = "Expense", Id = 6 },
            new { Name = "Развлечения", CategoryType = "Expense", Id = 7 }
        );
    }
}