using System;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            FurnitureFactory plasticFactory = new PlasticFurnitureFactory();
            FurnitureFactory woodenFactory = new WoodenFurnitureFactory();

            Console.WriteLine(plasticFactory.getChair().GetType());
            Console.WriteLine(plasticFactory.getTable().GetType());
            Console.WriteLine(woodenFactory.getChair().GetType());
            Console.WriteLine(woodenFactory.getTable().GetType());
        }
    }
}
