using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Database.Exceptions.Elastic;
using Database.Filtering.Attributes;

namespace Database.Filtering.Criteria.SQLCriteria
{
    using OperatorToFunctionDict = Dictionary<string, Func<NumericSqlCriteria, string, string, string>>;

    public class NumericSqlCriteria : Criteria<string>
    {
        private static OperatorToFunctionDict registry = GetRegistry<NumericSqlCriteria>();
        protected static readonly Regex ValuePattern = new Regex(
            @"^[+-]?([1-9][0-9]*(\.[0-9]+)?)|(0\.[0-9]+)$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase
        );

        public NumericSqlCriteria(string field, string @operator, string value) : base(field, @operator, value)
        {
            value = value.Trim();
            if (ValuePattern.Match(value) is null)
                throw new InvalidNestFilterException($"\"{value}\" is invalid for NumericCriteria");
        }

        [FilterOperator("gte")]
        public static string GreaterThanOrEqual(string field, string value)
        {
            throw new NotImplementedException();
        }

        [FilterOperator("gt")]
        public static string GreaterThan(string field, string value)
        {
            throw new NotImplementedException();
        }

        [FilterOperator("lte")]
        public static string LessThanOrEqual(string field, string value)
        {
            throw new NotImplementedException();
        }

        [FilterOperator("lt")]
        public static string LessThan(string field, string value)
        {
            throw new NotImplementedException();
        }

        [FilterOperator("eq")]
        public static string Equal(string field, string value)
        {
            throw new NotImplementedException();
        }

        [FilterOperator("nq")]
        public static string NotEqual(string field, string value)
        {
            throw new NotImplementedException();
        }

        public override string Interpret()
        {
            if (!registry.ContainsKey(Operator))
                throw new InvalidNestFilterException($"Operator: \"{Operator}\" is not registered in NumericCriteria");
            return registry[Operator].Invoke(null, Field, Value);
        }
    }
}