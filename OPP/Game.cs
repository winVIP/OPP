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
using System.Globalization;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using OPP.Interfaces;
using OPP.Behaviors;
using OPP.Sound;

namespace OPP
{
    public partial class Game : Form
    {
        //public static string hostip = "https://85.206.189.49:44320";
        public static string hostip = "https://localhost:44320";

        private SoundMachine soundMachine = new SoundMachine();

        public int i = 0;
        private Random random = new Random();

        private Map map = new Map();
        private List<Color> allColors = new List<Color>();

        private PictureBox playerPictureBox;
        private IWalkBehavior normalWalkBehavior = new NormalWalk();
        private IWalkBehavior confusedWalkBehavior = new ConfusedWalk();
        private IWalkBehavior confusedWalkBehavior2 = new ConfusedWalk2();
        private IWalkBehavior confusedWalkBehavior3 = new ConfusedWalk3();

        private bool up = false;
        private bool down = false;
        private bool left = false;
        private bool right = false;

        private Color playerColor = Color.White;
        private int index;
        private int playerSpeed = 10;

        //Should be false by default
        private bool isConfused = false;
        private bool confusionChecked = false;

        private static HttpClient client;

        public Game(int mode)
        {
            var handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    return true;
                };

            client = new HttpClient(handler);

            Terminal.Terminal myTerminal = new Terminal.Terminal();
            myTerminal.Show();

            

            InitializeComponent();
            FirstPost(mode);

            allColors.Add(Color.Red);
            allColors.Add(Color.Blue);
            allColors.Add(Color.Yellow);
            allColors.Add(Color.Green);
            allColors.Add(Color.Pink);
            allColors.Add(Color.Brown);
            allColors.Add(Color.Orange);
            allColors.Add(Color.Violet);

            Task initialMapGet;
            initialMapGet = getMapAsyncAwait();
            initialMapGet.Wait();

            playerPictureBox = drawPlayer();
            timer1.Enabled = true;
            timer2.Enabled = true;
            timer3.Enabled = true;
            timer4.Enabled = true;
            timer5.Enabled = true;
            POSTplayerPosition.Enabled = true;
            Size mapSize = new Size(5700, 3000);
            Point nullPoint = new Point(0, 0);
            //this.Bounds = new Rectangle(nullPoint, mapSize);
            updateGenerator();
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
            if (e.KeyCode == Keys.R)
            {
                sendSetRewind();
            }
            if (e.KeyCode == Keys.T)
            {
                sendTriggerRewind();
            }
        }

        void sendSetRewind()
        {
            string responseString = client.GetStringAsync(hostip + "/api/game/rewind/" + playerPictureBox.BackColor.Name + "/set").Result; // Original 44320, jonas 5001
            Debug.WriteLine(responseString);
        }
        void sendTriggerRewind()
        {
            string responseString = client.GetStringAsync(hostip + "/api/game/rewind/" + playerPictureBox.BackColor.Name + "/trigger").Result; // Original 44320, jonas 5001
            string[] XY = responseString.Split(";");
            Point rewindPos = new Point( int.Parse(XY[0]), int.Parse(XY[1]));
            playerPictureBox.Location = rewindPos;
            Debug.WriteLine(responseString);
        }


        /* public void getMapAsync()
         {
             string responseString = client.GetStringAsync("https://localhost:44320/api/game").Result;

             map.ClearFoodAndPlayers();

             List<UnitData> unitData = JsonConvert.DeserializeObject<List<UnitData>>(responseString);

             foreach (var item in unitData)
             {
                 if(item.type == 0)
                 {
                     //if player is not confused he checks data from server server wether he is confused
                     if(playerColor == item.playerColor && isConfused == false)
                     {
                         isConfused = item.confused;
                     }
                     map.addPlayer(new Unit(item.position, item.playerColor, item.playerSize));
                 }
                 else
                 {
                     map.addFood(new Unit(item.position, item.type));
                 }
             }

             playerPictureBox.BackColor = map.getPlayers()[index].getColor();

             GraphicsPath path = new GraphicsPath();
             path.AddEllipse(0, 0, map.getPlayers()[index].getSize().Width, map.getPlayers()[index].getSize().Height);

             Region region = new Region(path);
             playerPictureBox.Region = region;

             playerPictureBox.Size = new Size(map.getPlayers()[index].getSize().Width, map.getPlayers()[index].getSize().Height);
         }*/

        public void getPlayers()
        {
            string responseString = client.GetStringAsync(hostip + "/api/game/players").Result;

            map.ClearPlayers();

            List<UnitData> playerUnitData = JsonConvert.DeserializeObject<List<UnitData>>(responseString);

            foreach (var item in playerUnitData)
            {
                if (item.type == 0)
                {
                    //if player is not confused he checks data from server server wether he is confused
                    if (playerColor == item.playerColor && isConfused == false)
                    {
                        isConfused = item.confused;
                    }
                    if (playerColor == item.playerColor && item.confused == true)
                    {
                        soundMachine.PlaySound("Confused.wav");
                        item.confused = false;
                    }
                    if (playerColor == item.playerColor && item.eatenNormal == true)
                    {
                        soundMachine.PlaySound("Normal.wav");
                        item.eatenNormal = false;
                    }
                    if (playerColor == item.playerColor && item.sizingDown == true)
                    {
                        soundMachine.PlaySound("SizeDown.wav");
                        item.sizingDown = false;
                    }
                    if (playerColor == item.playerColor && item.sizingUp == true)
                    {
                        soundMachine.PlaySound("SizeUp.wav");
                        item.sizingUp = false;
                    }
                    //Checking if we need to get food too
                    if (item.playerColor == playerColor)
                    {
                        //Task updateFoodTask;
                        //updateFoodTask = updateFood();
                        //updateFoodTask.Wait();
                        updateFood();
                    }
                    map.addPlayer(new Unit(item.position, item.playerColor, item.playerSize));
                }
            }

            //update player position in form (visually)
            playerPictureBox.BackColor = map.getPlayers()[index].getColor();

            GraphicsPath path = new GraphicsPath();
            path.AddEllipse(0, 0, map.getPlayers()[index].getSize().Width, map.getPlayers()[index].getSize().Height);

            Region region = new Region(path);
            playerPictureBox.Region = region;

            playerPictureBox.Size = new Size(map.getPlayers()[index].getSize().Width, map.getPlayers()[index].getSize().Height);

            updatePlayers();

        }

        public async Task getMapAsyncAwait()
        {
            string responseString = client.GetStringAsync(hostip + "/api/game").Result;

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
                    }
                    map.addPlayer(player);
                }
                else
                {
                    map.addFood(new Unit(item.position, item.type, item.playerColor));
                }
            }

            updateFoodinForm();
            index = allColors.IndexOf(playerColor);
        }
        
        void updateFood()
        {
            string responseString = client.GetStringAsync(hostip + "/api/game/food").Result;
            List<UnitData> unitData = JsonConvert.DeserializeObject<List<UnitData>>(responseString);

            for (int i = 0; i < unitData.Count; i++)
            {
                if(map.getFood()[i].getPosition() != unitData[i].position)
                {
                    foreach(Control item in Controls)
                    {
                        if (item.Text.Equals(i.ToString()))
                        {
                            item.Location = unitData[i].position;
                        }
                    }
                    //map.getFood()[i].setPosition(unitData[i].position);
                    map.setFood(i, new Unit(unitData[i].position, unitData[i].type, unitData[i].playerColor));
                }
            }

        }

        public void updateGenerator()
        {
            string responseString = client.GetStringAsync(hostip + "/api/game/generator").Result;
            Generator generator = JsonConvert.DeserializeObject<Generator>(responseString);
            Console.WriteLine(generator);
            Debug.WriteLine("Generator: " + generator.color.Name + generator.position.X.ToString());
            map.setGenerator(generator);
            foreach (Control item in Controls)
            {
                if (item.Text == "generator")
                {
                    Controls.Remove(item);
                }
            }
            updateGeneratorsinForm();
        }

        public void updateCross()
        {
            string responseString2 = client.GetStringAsync(hostip + "/api/game/cross").Result;
            Cross cross = JsonConvert.DeserializeObject<Cross>(responseString2);
            map.setCross(cross);
            foreach (Control item in Controls)
            {
                if (item.Text == "cross")
                {
                    Controls.Remove(item);
                }
            }
            updateCrossinForm();
        }

        public void updateCircle()
        {
            string responseString3 = client.GetStringAsync(hostip + "/api/game/circle").Result;
            Circle circle = JsonConvert.DeserializeObject<Circle>(responseString3);
            map.setCircle(circle);
            foreach (Control item in Controls)
            {
                if (item.Text == "circle")
                {
                    Controls.Remove(item);
                }
            }
            updateCircleinForm();
        }

        void updateCrossinForm()
        {
            var cross = map.getCross();

            var path = new GraphicsPath();
            Rectangle rect1 = new Rectangle(50, 0, 30, 130);
            Rectangle rect2 = new Rectangle(0, 50, 130, 30);
            Rectangle[] arr = new Rectangle[2];
            arr[0] = rect1;
            arr[1] = rect2;
            path.AddRectangles(arr);
            //path.AddRectangle(rect2);
            path.FillMode = FillMode.Winding;
            // Convert the GraphicsPath into a Region.
            var region = new Region(path);
            var pictureBox = new PictureBox();
            pictureBox.BackColor = cross.color;
            pictureBox.Region = region;
            pictureBox.Size = new Size(130, 130);
            pictureBox.Location = cross.position;
            pictureBox.Text = "cross";
            this.Controls.Add(pictureBox);
            Debug.WriteLine("Cross X: " + cross.color.Name + cross.position.X.ToString());

        }

        void updateCircleinForm()
        {
            var circle = map.getCircle();
            var path = new GraphicsPath();
            path.AddEllipse(0, 0, 120, 120);
            // Convert the GraphicsPath into a Region.
            var region = new Region(path);
            var pictureBox = new PictureBox();
            pictureBox.BackColor = circle.color;
            pictureBox.Region = region;
            pictureBox.Size = new Size(130, 130);
            pictureBox.Location = circle.position;
            pictureBox.Text = "circle";
            this.Controls.Add(pictureBox);
            Debug.WriteLine("Circle X: " + circle.color.Name + circle.position.X.ToString());
        }

        void updateGeneratorsinForm()
        {
            var gen = map.getGenerator();
            // Make a GraphicsPath and add the circle.
            GraphicsPath path = new GraphicsPath();
            Rectangle rect = new Rectangle(0,0,130,130);
            path.AddRectangle(rect);
            // Convert the GraphicsPath into a Region.
            Region region = new Region(path);
            PictureBox pictureBox = new PictureBox();
            pictureBox.BackColor = gen.color;
            pictureBox.Region = region;
            pictureBox.Size = new Size(80, 80);
            pictureBox.Location = gen.position;
            pictureBox.Text = "generator";
            this.Controls.Add(pictureBox);
            Debug.WriteLine("Generator X: " + gen.color.Name + gen.position.X.ToString());
        }


        void updateFoodinForm()
        {

            foreach (Unit unit in map.getFood())
            {

                Debug.WriteLine("Food color: " + unit.getColor());
                Color color = unit.getColor();

                // Make a GraphicsPath and add the circle.
                GraphicsPath path = new GraphicsPath();
                path.AddEllipse(0, 0, 10, 10);

                // Convert the GraphicsPath into a Region.
                Region region = new Region(path);

                PictureBox pictureBox = new PictureBox();
                pictureBox.BackColor = color;
                pictureBox.Region = region;
                pictureBox.Size = new Size(10, 10);
                pictureBox.Location = unit.getPosition();
                pictureBox.Text = unit.index.ToString();
                this.Controls.Add(pictureBox);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (isConfused)
            {
                switch (random.Next(0, 3))
                {
                    case 0:
                        confusedWalkBehavior.Walk(playerSpeed, left, right, up, down, playerPictureBox);
                        break;
                    case 1:
                        confusedWalkBehavior2.Walk(playerSpeed, left, right, up, down, playerPictureBox);
                        break;
                    case 2:
                        confusedWalkBehavior3.Walk(playerSpeed, left, right, up, down, playerPictureBox);
                        break;
                    default:
                        break;

                }

                if (!confusionChecked)
                {
                    confusionChecked = true;
                    Task.Delay(new TimeSpan(0, 0, 2)).ContinueWith(o => { isConfused = false; confusionChecked = false;});
                }
            }
            else
            {
                normalWalkBehavior.Walk(playerSpeed, left, right, up, down, playerPictureBox);
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            //Players are always moving therefore need to be updated constantly, unlike food.
            getPlayers();
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            //Generators are always moving therefore need to be updated constantly, unlike food.
            updateGenerator();
        }

        private void timer4_Tick(object sender, EventArgs e)
        {
            //Generators are always moving therefore need to be updated constantly, unlike food.
            updateCircle();
        }

        private void timer5_Tick(object sender, EventArgs e)
        {
            //Generators are always moving therefore need to be updated constantly, unlike food.
            updateCross();
        }

        private void updatePlayers()
        {
            //Clearing players from form before adding again

            foreach(Control item in Controls)
            {
                if(item.BackColor != playerColor && allColors.Contains(item.BackColor) && item.Text != "DONT-REMOVE" && item.Text != "generator" && item.Text != "circle" && item.Text != "cross")
                {
                    Debug.WriteLine("I just removed an item of this " + item.BackColor + " color!");
                    Controls.Remove(item);
                }
            }

            foreach(Unit unit in map.getPlayers())
            {
                if (unit.getColor() == playerColor || unit.getColor() == Color.White) continue;
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
            PostBasicAsync(forSending, new CancellationToken()).Wait();

        }

        void FirstPost(int mode)
        {
            UnitData unitData = new UnitData();
            unitData.playerColor = Color.White;
            unitData.position = new Point(-9999, -9999);
            unitData.playerSize = new Size(20,20);
            unitData.type = 0;
            unitData.mode = mode;

            string forSending = string.Format("{{ \"position\":\"{0}, {1}\",\"type\":{2},\"playerColor\":\"{3}\",\"playerSize\":\"{4}, {5}\",\"confused\":{6},\"mode\":\"{7}\"}}",
                unitData.position.X, unitData.position.Y.ToString(), unitData.type.ToString(), unitData.playerColor.Name,
                unitData.playerSize.Width.ToString(), unitData.playerSize.Height.ToString(), isConfused.ToString().ToLower(), mode);
            PostBasicAsync(forSending, new CancellationToken());
        }

        private static async Task PostBasicAsync(object content1, CancellationToken cancellationToken)
        {
            string tokenURL = hostip + "/api/game";

            var handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    return true;
                };

            HttpClient client2 = new HttpClient(handler);
            client2.BaseAddress = new Uri(tokenURL);

            client2.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            HttpContent content = new StringContent(JsonConvert.SerializeObject(content1), Encoding.UTF8, "application/json");

            HttpResponseMessage response = client2.PostAsync("", content).Result;
            if (response.IsSuccessStatusCode)
            {

            }
            else
            {
                Debug.WriteLine("Nepavyko");
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }

            client2.Dispose();
            //using (var client = new HttpClient())
            //using (var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:44320/api/game"))
            //{
            //    var json = JsonConvert.SerializeObject(content);
            //    using (var stringContent = new StringContent(json, Encoding.UTF8, "application/json"))
            //    {
            //        request.Content = stringContent;

            //        using (var response = await client
            //            .SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken)
            //            .ConfigureAwait(false))
            //        {
            //            response.EnsureSuccessStatusCode();
            //        }
            //    }
            //}
        }
    }
}
