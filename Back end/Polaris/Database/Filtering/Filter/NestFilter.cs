using Database.Filtering.Criteria;
using Nest;
using System.Collections.Generic;
using System.Linq;

namespace Database.Filtering.Filter
{
    public class NestFilter : Filter<QueryContainer>
    {

        public NestFilter(string[] filterQueries, Dictionary<string, string> mapping) : base(filterQueries, mapping)
        {
        }

        public override QueryContainer Interpret()
        {
            return (QueryContainer)new BoolQuery
            {
                Must = this.criterias.Select(criteria => criteria.Interpret()).ToList()
            };
        }

        protected override void BuildCriterias(string[] filterQueries)
        {
            foreach (var query in filterQueries)
                criterias.Add(
                    BuildCriteria(query,
                (q, o, v) => new TextNestCriteria(q, o, v),
                (q, o, v) => new NumericNestCriteria(q, o, v)));
        }


    }
}