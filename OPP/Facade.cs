using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace OPP
{
    public class Facade
    {
        /*Game GameSystem;
        Login LoginSystem;
        Map MapSystem;

        public Facade()
        {
            GameSystem = new Game();
            LoginSystem = new Login();
            MapSystem = new Map();
        }

        public void ConfigureApplicationAndStartLoginProccess()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(LoginSystem);
        }

        public string SendARequestandReturnAnswer(PictureBox playerPictureBox)
        {
            List<string> requestArgs = new List<string>();
            requestArgs.Add(playerPictureBox.BackColor.Name);
            requestArgs.Add("/set");
            Get getreq = new Get("https://localhost:5001/api/game/rewind/", requestArgs);
            return getreq.SendRequest();
        }*/

    }
}
