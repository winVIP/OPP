using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    abstract class FurnitureFactory
    {
        public abstract Furniture getChair();
        public abstract Furniture getTable();

    }
}
