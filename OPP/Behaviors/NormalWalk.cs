using OPP.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace OPP.Behaviors
{

    class NormalWalk : IWalkBehavior
    {
        public void Walk(int spd, bool left, bool right, bool up, bool down, PictureBox playerPictureBox)
        {
            int speed = spd;
            if (left == true && playerPictureBox.Location.X > 1)
            {
                for (int i = 0; i < speed; i++)
                {
                    playerPictureBox.Location = new Point(playerPictureBox.Location.X - 1, playerPictureBox.Location.Y);
                }
            }
            if (right == true && playerPictureBox.Location.X < 1899)
            {
                for (int i = 0; i < speed; i++)
                {
                    playerPictureBox.Location = new Point(playerPictureBox.Location.X + 1, playerPictureBox.Location.Y);
                }
            }
            if (up == true && playerPictureBox.Location.Y > 1)
            {
                for (int i = 0; i < speed; i++)
                {
                    playerPictureBox.Location = new Point(playerPictureBox.Location.X, playerPictureBox.Location.Y - 1);
                }
            }
            if (down == true && playerPictureBox.Location.Y < 999)
            {
                for (int i = 0; i < speed; i++)
                {
                    playerPictureBox.Location = new Point(playerPictureBox.Location.X, playerPictureBox.Location.Y + 1);
                }
            }
        }
    }
}
