using DapperExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperPredicateParser
{

 
   
    class Program
    {
        static void Main(string[] args)
        {

           

            var groupPredicate = new PredicateGroup
            {
                Operator = GroupOperator.Or,
                Predicates = new List<IPredicate>()
            };

            //        var predicateGroup1 = new PredicateGroup
            //        {
            //            Operator = GroupOperator.Or,
            //            Predicates = new List<IPredicate>()
            //{
            //    Predicates.Field<TestCarModel>(f => f.Model, Operator.Eq, "500"),
            //    Predicates.Field<TestCarModel>(f => f.Model, Operator.Eq, "Bravo")
            //}
            // };

        }
    }
}
