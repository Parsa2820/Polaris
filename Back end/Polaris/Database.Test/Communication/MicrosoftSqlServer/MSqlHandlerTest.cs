using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Database.Communication.MicrosoftSqlServer;
using Elasticsearch.Net;
using Xunit;
using Xunit.Abstractions;

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
