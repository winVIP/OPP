using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace OPP.Interfaces
{
    interface IWalkBehavior
    {
        void Walk(int spd, bool left, bool right, bool up, bool down, PictureBox playerPictureBox);
    }
}
