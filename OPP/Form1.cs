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
        List<PictureBox> pbFood = new List<PictureBox>();
        Map map = new Map();
        List<Color> allColors = new List<Color>();

        PictureBox playerPictureBox;
        bool up = false;
        bool down = false;
        bool left = false;
        bool right = false;

        Color playerColor = Color.White;
        int index;
        public Form1()
        {
            InitializeComponent();
            FirstPost();

            allColors.Add(Color.Red);
            allColors.Add(Color.Blue);
            allColors.Add(Color.Yellow);
            allColors.Add(Color.Green);
            allColors.Add(Color.Pink);
            allColors.Add(Color.Brown);
            allColors.Add(Color.Orange);
            allColors.Add(Color.Violet);

            Debug.WriteLine("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA: " + playerColor);
            Task initialMapGet;
            initialMapGet = getMapAsyncAwait();
            initialMapGet.Wait();

            playerPictureBox = drawPlayer();
            timer1.Enabled = true;
            timer2.Enabled = true;
            POSTplayerPosition.Enabled = true;
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

        private static HttpClient client = new HttpClient();

        

        public void getMapAsync()
        {
            string responseString = client.GetStringAsync("https://localhost:44320/api/game").Result;

            map.ClearFoodAndPlayers();

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

            playerPictureBox.BackColor = map.getPlayers()[index].getColor();
            Debug.WriteLine("Colonel: " + map.getPlayers()[index].getColor());

            GraphicsPath path = new GraphicsPath();
            path.AddEllipse(0, 0, map.getPlayers()[index].getSize().Width, map.getPlayers()[index].getSize().Height);

            Region region = new Region(path);
            playerPictureBox.Region = region;

            playerPictureBox.Size = new Size(map.getPlayers()[index].getSize().Width, map.getPlayers()[index].getSize().Height);

        }

        public async Task getMapAsyncAwait()
        {
            string responseString = client.GetStringAsync("https://localhost:44320/api/game").Result;

            map.ClearFoodAndPlayers();

            List<UnitData> unitData = JsonConvert.DeserializeObject<List<UnitData>>(responseString);

            foreach (var item in unitData)
            {
                if (item.type == 0)
                {
                    Unit player = new Unit(item.position, item.playerColor, item.playerSize);
                    if(player.getPosition().X == -9999 && player.getPosition().Y == -9999)
                    {
                        playerColor = player.getColor();
                        player.setPosition(new Point(1, 1));
                        Debug.WriteLine("First get playerColor: " + player.getColor());
                    }
                    map.addPlayer(player);
                }
                else
                {
                    map.addFood(new Unit(item.position));
                }
            }

            index = allColors.IndexOf(playerColor);

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

            getMapAsync();
            FormMap();
        }

        private void FormMap()
        {
            //Clearing stuff from form before adding again

            foreach(Control item in Controls)
            {
                if(item.BackColor != playerColor)
                {
                    Controls.Remove(item);
                }
            }

            foreach(Unit unit in map.getFood())
            {
                // Make a GraphicsPath and add the circle.
                GraphicsPath path = new GraphicsPath();
                path.AddEllipse(0, 0, 10, 10);

                // Convert the GraphicsPath into a Region.
                Region region = new Region(path);

                PictureBox pictureBox = new PictureBox();
                pictureBox.BackColor = Color.Black;
                pictureBox.Region = region;
                pictureBox.Size = new Size(10, 10);
                pictureBox.Location = unit.getPosition();
                this.Controls.Add(pictureBox);
            }

            foreach(Unit unit in map.getPlayers())
            {
                if (unit.getColor() == playerColor) continue;
                GraphicsPath path = new GraphicsPath();
                path.AddEllipse(0, 0, unit.getSize().Height, unit.getSize().Width);

                // Convert the GraphicsPath into a Region.
                Region region = new Region(path);

                PictureBox pictureBoxP = new PictureBox();
                pictureBoxP.BackColor = unit.getColor();
                pictureBoxP.Region = region;
                pictureBoxP.Size = new Size(unit.getSize().Width, unit.getSize().Height);
                pictureBoxP.Location = unit.getPosition();
                this.Controls.Add(pictureBoxP);
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

        private void POSTplayerPosition_Tick(object sender, EventArgs e)
        {
            UnitData unitData = new UnitData();
            unitData.playerColor = playerPictureBox.BackColor;
            unitData.position = playerPictureBox.Location;
            unitData.playerSize = playerPictureBox.Size;
            unitData.type = 0;

            string forSending = string.Format("{{ \"position\":\"{0}, {1}\",\"type\":{2},\"playerColor\":\"{3}\",\"playerSize\":\"{4}, {5}\"}}",
                unitData.position.X, unitData.position.Y.ToString(), unitData.type.ToString(), unitData.playerColor.Name, unitData.playerSize.Width.ToString(), unitData.playerSize.Height.ToString());
            PostBasicAsync(forSending, new CancellationToken());
        }

        void FirstPost()
        {
            UnitData unitData = new UnitData();
            unitData.playerColor = Color.White;
            unitData.position = new Point(-9999, -9999);
            unitData.playerSize = new Size(20,20);
            unitData.type = 0;

            string forSending = string.Format("{{ \"position\":\"{0}, {1}\",\"type\":{2},\"playerColor\":\"{3}\",\"playerSize\":\"{4}, {5}\"}}",
                unitData.position.X, unitData.position.Y.ToString(), unitData.type.ToString(), unitData.playerColor.Name, unitData.playerSize.Width.ToString(), unitData.playerSize.Height.ToString());
            PostBasicAsync(forSending, new CancellationToken()).Wait();
        }

        private static async Task PostBasicAsync(object content, CancellationToken cancellationToken)
        {
            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:44320/api/game"))
            {
                var json = JsonConvert.SerializeObject(content);
                using (var stringContent = new StringContent(json, Encoding.UTF8, "application/json"))
                {
                    request.Content = stringContent;

                    using (var response = await client
                        .SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken)
                        .ConfigureAwait(false))
                    {
                        response.EnsureSuccessStatusCode();
                    }
                }
            }
        }
    }
}
