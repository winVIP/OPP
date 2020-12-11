using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    class WoodenFurnitureFactory : FurnitureFactory
    {
        public override Furniture getChair()
        {
            return new WoodenChair();
        }

        public override Furniture getTable()
        {
            return new WoodenTable();
        }
    }
}
