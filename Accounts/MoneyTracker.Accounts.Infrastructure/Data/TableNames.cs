using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyTracker.Accounts.Infrastructure.Data
{
    internal static class TableNames
    {
        public const string Accounts = "Accounts";
        public const string Transactions = "Transactions";
        public const string Categories = "Categories";
        public const string Currencies = "Currencies";

        public const string TransactionSources = "TransactionSources";
        public const string Transfers = "Transfers";
    }
}
