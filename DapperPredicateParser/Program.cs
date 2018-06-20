using DapperExtensions;
using DapperPredicateParser.Model;
using DapperPredicateParser.ParserComponents;
using System.Collections.Generic;
using System.Linq;

namespace DapperPredicateParser
{
    class Program
    {
        static void Main(string[] args)
        {
            var predicates = new List<IPredicate>
            {
                Predicates.Field<Car>(f => f.Type, Operator.Eq, "Sedan"),
                Predicates.Field<Car>(f => f.Type, Operator.Eq, "Hatchback"),
            };

            var testdata = Car.PrepareTestData();
            var expression = PredicateParser<Car>.ParseOr(predicates).Compile();
            var result = testdata.Where(expression);
        }
    }
}