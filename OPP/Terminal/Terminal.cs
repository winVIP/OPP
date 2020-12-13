using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace OPP.Terminal
{
    public partial class Terminal : Form
    {
        public Terminal()
        {
            InitializeComponent();
        }

        private void Terminal_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string URL = "https://localhost:44320/api/game/commandline";
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(URL);

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpContent content = new StringContent(JsonConvert.SerializeObject(textBox1.Text),
                    Encoding.UTF8,
                    "application/json");

                HttpResponseMessage response = client.PostAsync("", content).Result;

                string dataObject;
                string result = "";

                if (response.IsSuccessStatusCode)
                {
                    dataObject = response.Content.ReadAsStringAsync().Result;
                    string[] results = dataObject.Split("\\r\\n");
                    for (int i = 0; i < results.Length; i++)
                    {
                        result = result + results[i] + Environment.NewLine;
                    }
                }

                else
                {
                    dataObject = response.StatusCode + " " + response.ReasonPhrase;
                }

                e.SuppressKeyPress = true;
                if (textBox2.Text == "")
                {
                    textBox2.Text = "Pressed enter and wrote: " + textBox1.Text + Environment.NewLine + "Response: " + Environment.NewLine + result;
                    textBox1.Text = "";
                }
                else
                {
                    textBox2.Text = textBox2.Text + Environment.NewLine + "Pressed enter and wrote: " + textBox1.Text + Environment.NewLine + "Response: " + Environment.NewLine + result;
                    textBox1.Text = "";
                }

                textBox2.SelectionStart = textBox2.Text.Length;
                textBox2.ScrollToCaret();
            }
        }
    }
}
