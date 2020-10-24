using Database.Filtering.Attributes;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Database.Filtering.Criteria
{
    public abstract class Criteria<TQueryContainer> : IDbInterpretable<TQueryContainer>
    {
        protected Criteria(string field, string @operator, string value)
        {
            Field = field;
            Operator = @operator;
            Value = value;
        }

        protected string Field { get; set; }
        protected string Operator { get; set; }
        protected string Value { get; set; }

        public abstract TQueryContainer Interpret();

        public static Dictionary<string, Func<TCriteria, string, string, TQueryContainer>> GetRegistry<TCriteria>()
        where TCriteria : Criteria<TQueryContainer>
        {
            var registry = new Dictionary<string, Func<TCriteria, string, string, TQueryContainer>>();
            var methods = typeof(TCriteria)
                .GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Where(m => m.GetCustomAttributes(typeof(FilterOperator), false).Length > 0);
            foreach (var method in methods)
            {
                var pair = BuildMethodDelegate<TCriteria>(method);
                registry[pair.Key] = pair.Value;
            }

            return registry;
        }

        private static KeyValuePair<string, Func<TCriteria, string, string, TQueryContainer>> BuildMethodDelegate<TCriteria>(MethodInfo method)
        where TCriteria : Criteria<TQueryContainer>
        {
            var objectInput = Expression.Parameter(typeof(TCriteria), "criteria");
            var fieldInput = Expression.Parameter(typeof(string), "field");
            var valueInput = Expression.Parameter(typeof(string), "value");
            var lambdaExpression = Expression.Lambda<Func<TCriteria, string, string, TQueryContainer>>(
                Expression.Call(null, method, fieldInput, valueInput), objectInput, fieldInput, valueInput)
                .Compile();

            return new KeyValuePair<string, Func<TCriteria, string, string, TQueryContainer>>(
                ((FilterOperator)method.GetCustomAttribute(typeof(FilterOperator), false)).Abbrv,
                lambdaExpression
            );
        }
    }
}