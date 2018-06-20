using System;
using System.Collections.Generic;
using System.Linq;
using DapperExtensions;
using DapperPredicateParser.Model;
using DapperPredicateParser.ParserComponents;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DapperPredicateParser.Tests
{
    [TestClass]
    public class PredicateParserTest
    {
        [TestMethod]
        public void ParseOr_CollectionOfCars_TwoTypeOfCars()
        {
            //Arrange
            var testdata = Car.PrepareTestData();

            var predicates = new List<IPredicate>
            {      
                Predicates.Field<Car>(f => f.Type, Operator.Eq, "Sedan"),
                Predicates.Field<Car>(f => f.Type, Operator.Eq, "Hatchback"),
            };

            //Act
            var expression = PredicateParser<Car>.ParseOr(predicates).Compile();
            var result = testdata.Where(expression);
            var typesNumber = result.GroupBy(b => b.Type).Count();

            //Assert
            Assert.AreEqual(2, typesNumber);
        }

        [TestMethod]
        public void Parse_CollectionOfCars_AllCars()
        {
            //Arrange
            var testdata = Car.PrepareTestData();

            var groupPredicate = new PredicateGroup
            {
                Operator = GroupOperator.Or,
                Predicates = new List<IPredicate>()
            };

            var predicateGroup1 = new PredicateGroup
            {
                Operator = GroupOperator.Or,
                Predicates = new List<IPredicate>()
                {
                    Predicates.Field<Car>(f => f.Model, Operator.Eq, "500"),
                    Predicates.Field<Car>(f => f.Model, Operator.Eq, "Bravo")
                }
            };

            var predicateGroup2 = new PredicateGroup
            {
                Operator = GroupOperator.Or,
                Predicates = new List<IPredicate>()
                {
                    Predicates.Field<Car>(f => f.Name, Operator.Eq, "Opel"),
                    Predicates.Field<Car>(f => f.Type, Operator.Eq, "Hatchback"),
                }
            };

            groupPredicate.Predicates.Add(Predicates.Field<Car>(f => f.Cost, Operator.Gt, 35000));
            groupPredicate.Predicates.Add(predicateGroup1);
            groupPredicate.Predicates.Add(predicateGroup2);

            //Act
            var expression = PredicateParser<Car>.Parse(groupPredicate).Compile();
            var result = testdata.Where(expression);

            //Assert
            Assert.AreEqual(result.Count(), testdata.Count());
        }

        [TestMethod]
        public void Parse_CollectionOfCars_ThreeTypeOfCars()
        {
            //Arrange
            var testdata = Car.PrepareTestData();

            var groupPredicate = new PredicateGroup
            {
                Operator = GroupOperator.Or,
                Predicates = new List<IPredicate>()
            };

            var predicateGroup1 = new PredicateGroup
            {
                Operator = GroupOperator.And,
                Predicates = new List<IPredicate>()
                {
                    Predicates.Field<Car>(f => f.Name, Operator.Eq, "Fiat"),
                    Predicates.Field<Car>(f => f.Model, Operator.Eq, "Bravo")
                }
            };

            var predicateGroup2 = new PredicateGroup
            {
                Operator = GroupOperator.And,
                Predicates = new List<IPredicate>()
                {
                    Predicates.Field<Car>(f => f.Cost, Operator.Gt, 20000),
                    Predicates.Field<Car>(f => f.Type, Operator.Eq, "Sedan"),
                }
            };

            groupPredicate.Predicates.Add(Predicates.Field<Car>(f => f.Type, Operator.Eq, "Hatchback"));
            groupPredicate.Predicates.Add(predicateGroup1);
            groupPredicate.Predicates.Add(predicateGroup2);

            //Act
            var expression = PredicateParser<Car>.Parse(groupPredicate).Compile();
            var result = testdata.Where(expression);

            //Assert
            Assert.AreEqual(3, result.Count());
        }
    }
}