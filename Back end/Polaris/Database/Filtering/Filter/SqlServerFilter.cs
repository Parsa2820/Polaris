using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Database.Filtering.Criteria.SQLCriteria;

namespace Database.Filtering.Filter
{
    public class SqlServerFilter : Filter<string>
    {
        public SqlServerFilter(string[] filterQueries, Dictionary<string, string> mapping) : base(filterQueries, mapping) { }
        private string table = "";
        public override string Interpret()
        {
            var queries = new StringBuilder();
            var andString = "AND";
            var whereString = "WHERE";
            if (criterias.Count != 0)
            {
                queries.Append(whereString);
                queries.Append(criterias[0]);
                this.criterias.GetRange(1, criterias.Count - 1).ForEach(str => { queries.Append(andString); queries.Append(str); });
            }
            return $"select * from {table} {queries.ToString()}";
        }

        protected override void BuildCriterias(string[] filterQueries)
        {
            foreach (var query in filterQueries)
                criterias.Add(
                    BuildCriteria(query,
                (q, o, v) => new TextSqlCriteria(q, o, v),
                (q, o, v) => new NumericSqlCriteria(q, o, v)));
        }
    }
}