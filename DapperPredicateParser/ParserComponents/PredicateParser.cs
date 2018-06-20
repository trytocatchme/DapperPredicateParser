using DapperExtensions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DapperPredicateParser.ParserComponents
{
    public static class PredicateParser<T>
    {
        public static Expression<Func<T, bool>> Parse(PredicateGroup predicateGroup)
        {
            var compositeIterator = new CompositeIterator<T>();
            var result = compositeIterator.Prepare(predicateGroup);

            return result;
        }

        public static Expression<Func<T, bool>> ParseAnd(IList<IPredicate> fieldPredicates)
        {
            Expression<Func<T, bool>> finalQuery = t => true;

            foreach (var predicate in fieldPredicates)
            {
                finalQuery = finalQuery.And(Parse(predicate));
            }

            return finalQuery;
        }

        public static Expression<Func<T, bool>> ParseOr(IList<IPredicate> fieldPredicates)
        {
            Expression<Func<T, bool>> finalQuery = t => false;

            foreach (var predicate in fieldPredicates)
            {
                finalQuery = finalQuery.Or(Parse(predicate));
            }

            return finalQuery;
        }

        public static Expression<Func<T, bool>> Parse(IPredicate predicate)
        {
            IFieldPredicate fieldPredicate = (IFieldPredicate)predicate;
            ParameterExpression parameterExpression = Expression.Parameter(typeof(T), "p");
            MemberExpression memberExpression = Expression.PropertyOrField(parameterExpression, fieldPredicate.PropertyName);
            UnaryExpression propertyValue = Expression.Convert(Expression.Constant(fieldPredicate.Value), memberExpression.Type);

            var operatorMatrix = new Dictionary<KeyValuePair<Operator, bool>, Func<Expression>>
            {
                { new KeyValuePair<Operator, bool>(Operator.Like, false),  () => LikeExpression.Like(memberExpression, propertyValue) },
                { new KeyValuePair<Operator, bool>(Operator.Eq, false),    () => Expression.Equal(memberExpression, propertyValue) },
                { new KeyValuePair<Operator, bool>(Operator.Gt, false),    () => Expression.GreaterThan(memberExpression, propertyValue) },
                { new KeyValuePair<Operator, bool>(Operator.Ge, false),    () => Expression.GreaterThanOrEqual(memberExpression, propertyValue) },
                { new KeyValuePair<Operator, bool>(Operator.Lt, false),    () => Expression.LessThan(memberExpression, propertyValue) },
                { new KeyValuePair<Operator, bool>(Operator.Le, false),    () => Expression.LessThanOrEqual(memberExpression, propertyValue) },
                { new KeyValuePair<Operator, bool>(Operator.Like, true),   () => LikeExpression.NotLike(memberExpression, propertyValue) },
                { new KeyValuePair<Operator, bool>(Operator.Eq, true),     () => Expression.NotEqual(memberExpression, propertyValue) },
                { new KeyValuePair<Operator, bool>(Operator.Gt, true),     () => Expression.LessThan(memberExpression, propertyValue) },
                { new KeyValuePair<Operator, bool>(Operator.Ge, true),     () => Expression.LessThanOrEqual(memberExpression, propertyValue) },
                { new KeyValuePair<Operator, bool>(Operator.Lt, true),     () => Expression.GreaterThan(memberExpression, propertyValue) },
                { new KeyValuePair<Operator, bool>(Operator.Le, true),     () => Expression.GreaterThanOrEqual(memberExpression, propertyValue) },
            };

            var body = operatorMatrix[new KeyValuePair<Operator, bool>(fieldPredicate.Operator, fieldPredicate.Not)].Invoke();

            return Expression.Lambda<Func<T, bool>>(body, parameterExpression);
        }
    }
}