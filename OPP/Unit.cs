using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace OPP
{
    public class Unit
    {
        Point position;
        // 0 - player, 1 - food, 2 - confusionFood
        int type;
        Color playerColor;
        Size playerSize;
        public int index;

        public Unit(Point position, Color color, Size size)
        {
            this.position = position;
            this.type = 0;
            this.playerColor = color;
            this.playerSize = size;
        }

        public Unit(Point position, int type, Color col)
        {
            this.position = position;
            this.type = type;
            this.playerColor = col;
        }

        public Point getPosition()
        {
            return this.position;
        }
        public int getType()
        {
            return this.type;
        }
        public Color getColor()
        {
            return this.playerColor;
        }
        public Size getSize()
        {
            return this.playerSize;
        }

        public void setPosition(Point position)
        {
            this.position = position;
        }
    }
}
