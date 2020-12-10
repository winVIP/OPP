using OPP.Behaviors;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace OPP
{
    [Serializable]
    public class UnitData
    {
        public Point position { get; set; }
        // 0 - player, 1 - food
        public int type { get; set; }
        public Color playerColor { get; set; }
        public Size playerSize { get; set; }

        public bool confused { get; set; }

        public bool foodListChanged { get; set; }

        public int foodIndex { get; set; }
    }
}
