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
using System.Net.Http;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using System.Diagnostics;

namespace OPP
{
    public partial class Form1 : Form
    {
        private static readonly HttpClient client = new HttpClient();
        Map map = new Map();

        PictureBox playerPictureBox;
        bool up = false;
        bool down = false;
        bool left = false;
        bool right = false;

        Color playerColor = Color.Green;
        public Form1()
        {
            InitializeComponent();
            playerPictureBox = drawPlayer();
            timer1.Enabled = true;
            timer2.Enabled = true;
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
            pictureBox.BackColor = playerColor;
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


        

        public void getMapAsync()
        {
            string responseString = client.GetStringAsync("https://agar.azurewebsites.net/api/game").Result;

            map.ClearFood();

            List<UnitData> unitData = JsonConvert.DeserializeObject<List<UnitData>>(responseString);

            foreach (var item in unitData)
            {
                if(item.type == 0)
                {
                    map.addPlayer(new Unit(item.position, item.playerColor, item.playerSize));
                }
                else
                {
                    map.addFood(new Unit(item.position));
                }
            }

            CheckCollisions();

        }

        private void CheckCollisions()
        {
            Unit me = map.getPlayers().Find(x => x.getColor() == playerColor);
            List<Unit> allExceptMe = map.GetAllUnitsExceptMe(playerColor);

            foreach(Unit unit in allExceptMe)
            {
                if(me.getPosition() == unit.getPosition() && unit.getType() == 1)
                {
                    //TODO send a msg to server that I ate something
                    //Need to convert to json string and then send. At server deserialize
                    var content = new FormUrlEncodedContent(me, unit);

                    var response = await client.PostAsync("http://www.example.com/recepticle.aspx", content);
                }
            }
        }


        private void timer1_Tick(object sender, EventArgs e)
        {

            int speed = 10;
            if (left == true && playerPictureBox.Location.X > 1)
            {
                for(int i = 0; i < speed; i++)
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

        private void timer2_Tick(object sender, EventArgs e)
        {

            Debug.WriteLine(DateTime.Now.ToString());
            getMapAsync();
            FormMap();
        }

        private void FormMap()
        {
            //Clearing stuff from form before adding again
            foreach(Control item in Controls)
            {
                if(item.BackColor != Color.Green)
                {
                    Controls.Remove(item);
                }
            }

            foreach(Unit unit in map.getFood())
            {
                // Make a GraphicsPath and add the circle.
                GraphicsPath path = new GraphicsPath();
                path.AddEllipse(0, 0, 20, 20);

                // Convert the GraphicsPath into a Region.
                Region region = new Region(path);

                PictureBox pictureBox = new PictureBox();
                pictureBox.BackColor = Color.Black;
                pictureBox.Region = region;
                pictureBox.Size = new Size(10, 10);
                pictureBox.Location = unit.getPosition();
                this.Controls.Add(pictureBox);
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
