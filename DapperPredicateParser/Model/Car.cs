using System.Collections.Generic;

namespace DapperPredicateParser.Model
{
    public class Car
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Model { get; set; }
        public string Type { get; set; }
        public float Cost { get; set; }

        public static List<Car> PrepareTestData()
        {
            return new List<Car>()
            {
                new Car(){ Id = 1, Name = "Fiat", Model = "500", Type = "Kompakt", Cost = 20000 },
                new Car(){ Id = 2, Name = "Fiat", Model = "Bravo", Type = "Kompakt", Cost = 30000 },
                new Car(){ Id = 3, Name = "Opel", Model = "Astra", Type = "Sedan", Cost = 20000 },
                new Car(){ Id = 4, Name = "Honda", Model = "Civic", Type = "Hatchback", Cost = 15000 },
                new Car(){ Id = 5, Name = "Audi", Model = "A4", Type = "Sedan", Cost = 40000 },
            };
        }
    }

}