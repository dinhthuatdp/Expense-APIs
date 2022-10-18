using System;
using System.Data.Common;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;

namespace CommonLib.Extensions
{
    public enum ExpressionMethods
    {
        Contains
    }

    public enum ExpressionCondition
    {
        And,
        Or
    }

    public static class ExpressionExtension
    {
        public static Expression<Func<TItem, bool>> BuildFilter<TItem>(
            ExpressionMethods expressionMethod,
            Dictionary<string, string>? properties,
            ExpressionCondition expressionCondition)
        {
            if (properties is null ||
                properties.Count == 0)
            {
                return u => true;
            }
            var method = typeof(string).GetMethod(expressionMethod.ToString(), new[] { typeof(string) });

            ParameterExpression parameter = Expression.Parameter(typeof(TItem));
            MemberExpression[] members = new MemberExpression[properties.Count];
            for (int i = 0; i < properties.Count; i++)
            {
                members[i] = Expression.Property(parameter, properties.Keys.ElementAt(i));
            }
            Expression? predicate = null;
            ConstantExpression constant;
            foreach (var item in members)
            {
                constant = Expression.Constant(properties[item.Member.Name.ToLower()]);
                MethodCallExpression callExp = Expression.Call(item, method, constant);
                switch (expressionCondition)
                {
                    case ExpressionCondition.And:
                        predicate = predicate == null
                            ? (Expression)callExp
                            : Expression.And(predicate, callExp);
                        break;
                    case ExpressionCondition.Or:
                        predicate = predicate == null
                            ? (Expression)callExp
                            : Expression.OrElse(predicate, callExp);
                        break;
                }
            }
            if (predicate is null)
            {
                throw new Exception($"BuildFilter Predicate is null");
            }
            return Expression.Lambda<Func<TItem, bool>>(predicate, parameter);
        }
    }
}

