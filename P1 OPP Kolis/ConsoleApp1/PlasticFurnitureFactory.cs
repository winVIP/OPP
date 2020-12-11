using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    class PlasticFurnitureFactory : FurnitureFactory
    {
        public override Furniture getChair()
        {
            return new PlasticChair();
        }

        public override Furniture getTable()
        {
            return new PlasticTable();
        }
    }
}
