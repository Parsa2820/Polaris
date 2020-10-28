using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Database.Exceptions.Elastic;
using Database.Filtering.Attributes;

namespace Database.Filtering.Criteria.SQLCriteria
{
    using OperatorToFunctionDict = Dictionary<string, Func<TextSqlCriteria, string, string, string>>;

    public class TextSqlCriteria : Criteria<string>
    {
        private static OperatorToFunctionDict registry = GetRegistry<TextSqlCriteria>();
        protected static readonly Regex ValuePattern = new Regex(
            @"^\S+$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase
        );

        private static readonly string table = "";
        private static readonly string likeQuery = "LIKE";

        public TextSqlCriteria(string field, string @operator, string value) : base(field, @operator, value)
        {
            value = value.Trim();
            if (ValuePattern.Match(value) is null)
                throw new InvalidNestFilterException($"\"{value}\" is invalid for TextCriteria");
        }

        [FilterOperator("eq")]
        public static string Equal(string field, string value)
        {
            return BuildSqlQueryString(field, value, "=");

        }

        [FilterOperator("nq")]
        public static string NotEqual(string field, string value)
        {
            return BuildSqlQueryString(field, value, "<>");


        }

        [FilterOperator("sw")]
        public static string StartsWith(string field, string value)
        {
            return BuildSqlQueryString(field, value + "%", likeQuery);


        }

        [FilterOperator("ew")]
        public static string EndsWith(string field, string value)
        {
            return BuildSqlQueryString(field, "%" + value, likeQuery);
        }

        [FilterOperator("cnt")]
        public static string Contains(string field, string value)
        {
            return BuildSqlQueryString(field, "%" + value + "%", likeQuery);

        }

        [FilterOperator("gt")]
        public static string GreaterThan(string field, string value)
        {
            return BuildSqlQueryString(field, value, ">");
        }

        [FilterOperator("gte")]
        public static string GreaterThanOrEqual(string field, string value)
        {
            return BuildSqlQueryString(field, value, ">=");

        }

        [FilterOperator("lt")]
        public static string LessThan(string field, string value)
        {
            return BuildSqlQueryString(field, value, "<");

        }

        [FilterOperator("lte")]
        public static string LessThanOrEqual(string field, string value)
        {
            return BuildSqlQueryString(field, value, "<=");
        }

        //TODO: avoid using * in select query and try to use explicit column names;
        private static string BuildSqlQueryString(string field, string value, string operation)
        {
            return $"{field} {operation} {value}";
        }
        public override string Interpret()
        {
            if (!registry.ContainsKey(Operator))
                throw new InvalidNestFilterException($"Operator: \"{Operator}\" is not registered in TextCriteria");
            return registry[Operator].Invoke(null, Field, Value);
        }
    }
}