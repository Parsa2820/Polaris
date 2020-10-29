using Database.Filtering.Criteria.SQLCriteria;
using System.Collections.Generic;
using System.Text;

namespace Database.Filtering.Filter
{
    public class SqlServerFilter : Filter<string>
    {
        private readonly string tableName;
        private readonly string columns;

        public SqlServerFilter(string[] filterQueries, Dictionary<string, string> mapping, string tableName, string columns) : base(filterQueries, mapping)
        {
            this.tableName = tableName;
            this.columns = columns;
        }

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
            return $"select {columns} from {tableName} {queries}";
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