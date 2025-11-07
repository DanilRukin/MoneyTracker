using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyTracker.Accounts.Domain.Categories;
using MoneyTracker.Accounts.Domain.Transactions;

namespace MoneyTracker.Accounts.Infrastructure.Data.Configurations
{
    internal class CategoryTypeConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable(TableNames.Categories, SchemaName.Name);

            builder.Property<int>("Id")
                .ValueGeneratedOnAdd();

            builder.HasKey("Id");

            builder.Property(c => c.Name)
                .HasMaxLength(64)
                .IsRequired();

            // Дискриминатор с конвертером enum -> string
            builder.HasDiscriminator(e => e.Type)
                .HasValue<IncomeCategory>(TransactionType.Income)
                .HasValue<ExpenseCategory>(TransactionType.Expense)
                .HasValue<TransferIncomeCategory>(TransactionType.Income) // Тот же тип что у родителя
                .HasValue<TransferExpenseCategory>(TransactionType.Expense);

            // Конвертер для enum в БД
            builder.Property(e => e.Type)
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.HasIndex(c => new { c.Name, c.Type })
                .IsUnique();            
        }
    }
}
