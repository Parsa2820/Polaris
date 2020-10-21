using Database.Communication.MicrosoftSqlServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Database.Test.Communication.MicrosoftSqlServer
{
    public class MSqlEntityHandlerTest
    {
        [Fact]
        public void DeleteEntityTest()
        {
            MSqlClientFactory.GetInstance().CreateInitialClient("Data Source=(local);Initial Catalog=Star;Integrated Security=True;");
            var handler = new MSqlHandler<DummyBankAccount>();
            var accounts = handler.FetchAll("BANK_ACCOUNT");
            var entityHandler = new MSqlEntityHandler<DummyBankAccount, int>();
            entityHandler.DeleteEntity(accounts.First().Id, "BANK_ACCOUNT");
            var newAccounts = handler.FetchAll("BANK_ACCOUNT");
            Assert.True(accounts.Count() - 1 == newAccounts.Count());
        }
    }
}
