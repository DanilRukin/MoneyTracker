using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyTracker.Accounts.Domain.Categories;

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

            builder.HasDiscriminator<string>("Type")
                .HasValue<IncomeCategory>("Income")
                .HasValue<ExpenseCategory>("Expense")
                .HasValue<TransferIncomeCategory>("TransferIncome")
                .HasValue<TransferExpenseCategory>("TransferExpense");

            builder.HasIndex(c => new { c.Name, c.Type })
                .IsUnique();

            builder.Ignore("Id");

            builder.HasData(new { Name = "Пополение переводом с другого счета", Type = "TransferIncome" });
            builder.HasData(new { Name = "Снятия для перевода на другой счет", Type = "TransferExpense" });
        }
    }
}
