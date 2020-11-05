using Database.Exceptions.Elastic;
using Database.Filtering.Criteria;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Database.Filtering
{
    public abstract class Filter<TQueryContainer> : IDbInterpretable<TQueryContainer>
    {
        static readonly Regex FilterPattern = new Regex(
            @"^(?<field>\S+)\s(?<operator>\S+)\s(?<value>(\S+\s)+)$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase
        );

        protected List<Criteria<TQueryContainer>> criterias;
        protected Dictionary<string, string> mapping;

        public Filter(string[] filterQueries, Dictionary<string, string> mapping)
        {
            this.mapping = mapping;
            criterias = new List<Criteria<TQueryContainer>>();
            BuildCriterias(filterQueries);
        }

        public abstract TQueryContainer Interpret();

        protected abstract void BuildCriterias(string[] filterQueries);

        protected Criteria<TQueryContainer> BuildCriteria<TTextCriteria, TNumericCriteria>(
            string filterQuery,
            Func<string, string, string, TTextCriteria> newTextCriteria,
            Func<string, string, string, TNumericCriteria> newNumericCriteria)
        where TTextCriteria : Criteria<TQueryContainer>
        where TNumericCriteria : Criteria<TQueryContainer>
        {
            var match = FilterPattern.Match(filterQuery.Trim() + " ");
            var selectedField = match.Groups["field"].Value;
            string @operator;
            string value;

            switch (mapping[selectedField.ToLower()])
            {
                case "text":
                    @operator = match.Groups["operator"].Value;
                    value = match.Groups["value"].Value.Trim();
                    return newTextCriteria(selectedField, @operator, value);

                case "numeric":
                    @operator = match.Groups["operator"].Value;
                    value = match.Groups["value"].Value.Trim();
                    return newNumericCriteria(selectedField, @operator, value);

                default:
                    throw new InvalidNestFilterException($"Field: \"{selectedField}\" not in valid fields: [{mapping.Keys}]");
            }
        }
    }
}