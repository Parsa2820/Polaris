using Database.Communication.MicrosoftSqlServer;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Database.Test.Communication.MicrosoftSqlServer
{
    public class MSqlHandlerTest
    {
        [Fact]
        public void FetchAllTest()
        {
            MSqlClientFactory.GetInstance().CreateInitialClient("Data Source=(local);Initial Catalog=Star;Integrated Security=True;");
            var handler = new MSqlHandler<DummyBankAccount>();
            var accounts = handler.FetchAll("BANK_ACCOUNT");
            System.IO.File.WriteAllText("test.txt", string.Join("\n", accounts.Select(a => a.ToString())));
            Assert.NotEmpty(accounts);
        }

        [Fact]
        public void InsertTest()
        {
            MSqlClientFactory.GetInstance().CreateInitialClient("Data Source=(local);Initial Catalog=Star;Integrated Security=True;");
            var handler = new MSqlHandler<DummyBankAccount>();
            var accounts = handler.FetchAll("BANK_ACCOUNT");
            handler.Insert(new DummyBankAccount() { Id = 111111, OwnerName = "Masoud" }, "BANK_ACCOUNT");
            var newAccounts = handler.FetchAll("BANK_ACCOUNT");
            Assert.True(accounts.Count() + 1 == newAccounts.Count());
        }

        [Fact]
        public void BulkInsertTest()
        {
            MSqlClientFactory.GetInstance().CreateInitialClient("Data Source=(local);Initial Catalog=Star;Integrated Security=True;");
            var handler = new MSqlHandler<DummyBankAccount>();
            var list = new List<DummyBankAccount>();
            list.Add(new DummyBankAccount() { Id = 2222, OwnerName = "Shahin" });
            handler.BulkInsert(list, "BANK_ACCOUNT");
        }
    }

    class DummyBankAccount
    {
        public int Id { get; set; }
        public string OwnerName { get; set; }

        public override string ToString()
        {
            return Id + "-" + OwnerName;
        }
    }
}
