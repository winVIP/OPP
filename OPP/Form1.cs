using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Drawing.Drawing2D;
using System.Numerics;
using System.Threading;

namespace OPP
{
    public partial class Form1 : Form
    {
        PictureBox pictureBox1;
        bool up = false;
        bool down = false;
        bool left = false;
        bool right = false;
        public Form1()
        {
            InitializeComponent();
            pictureBox1 = drawPlayer();
            timer1.Enabled = true;
            Size mapSize = new Size(5700, 3000);
            Point nullPoint = new Point(0, 0);
            //this.Bounds = new Rectangle(nullPoint, mapSize);
        }

        public PictureBox drawPlayer()
        {
            // Make a GraphicsPath and add the circle.
            GraphicsPath path = new GraphicsPath();
            path.AddEllipse(0, 0, 20, 20);

            // Convert the GraphicsPath into a Region.
            Region region = new Region(path);

            PictureBox pictureBox = new PictureBox();
            pictureBox.BackColor = Color.Black;
            pictureBox.Region = region;
            pictureBox.Size = new Size(20, 20);
            this.Controls.Add(pictureBox);
            return pictureBox;
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            //Vector2 mousePos = new Vector2(e.X, e.Y);
            //Vector2 playerPos = new Vector2(pictureBox1.Location.X, pictureBox1.Location.Y);
            //Vector2 newPos = Vector2.Lerp(playerPos, mousePos, 0.01f);
            //pictureBox1.Location = new Point((int)newPos.X, (int)newPos.Y);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            
            if(e.KeyCode == Keys.Left)
            {
                left = true;
            }
            if(e.KeyCode == Keys.Right)
            {
                right = true;
            }
            if(e.KeyCode == Keys.Up)
            {
                up = true;
            }
            if(e.KeyCode == Keys.Down)
            {
                down = true;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int speed = 10;
            if (left == true && pictureBox1.Location.X > 1)
            {
                for(int i = 0; i < speed; i++)
                {
                    pictureBox1.Location = new Point(pictureBox1.Location.X - 1, pictureBox1.Location.Y);
                }
            }
            if (right == true && pictureBox1.Location.X < 1899)
            {
                for (int i = 0; i < speed; i++)
                {
                    pictureBox1.Location = new Point(pictureBox1.Location.X + 1, pictureBox1.Location.Y);
                }
            }
            if (up == true && pictureBox1.Location.Y > 1)
            {
                for (int i = 0; i < speed; i++)
                {
                    pictureBox1.Location = new Point(pictureBox1.Location.X, pictureBox1.Location.Y - 1);
                }
            }
            if (down == true && pictureBox1.Location.Y < 999)
            {
                for (int i = 0; i < speed; i++)
                {
                    pictureBox1.Location = new Point(pictureBox1.Location.X, pictureBox1.Location.Y + 1);
                }
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                left = false;
            }
            if (e.KeyCode == Keys.Right)
            {
                right = false;
            }
            if (e.KeyCode == Keys.Up)
            {
                up = false;
            }
            if (e.KeyCode == Keys.Down)
            {
                down = false;
            }
        }
    }
}
