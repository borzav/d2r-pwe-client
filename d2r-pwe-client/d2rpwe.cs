using d2r_pwe_client.D2R;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace d2r_pwe_client
{
    public partial class d2rpwe : Form
    {
        private bool isStarted = false;
        private bool isHotIP = false;
        private GameIPService gis = new GameIPService();
        private PWEApi api = new PWEApi();
        private Timer gameChecker = new Timer();

        private Timer gameTimer = new Timer();
        private int gameTime = 0;
        private int checkerIteration = 0;

        string currentGame = "";

        public d2rpwe()
        {
            InitializeComponent();

            gameChecker.Interval = 3000;
            gameChecker.Tick += GameChecker_Tick;
            gameTimer.Interval = 1000;
            gameTimer.Tick += GameTimer_Tick;
            if (gis.ProcessId <= 0)
            {
                tbGame.Text = "D2R not running";
            }
            else
            {
                tbGame.Text = "Start in the lobby";
            }
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            gameTime++;
            tbSec.Text = string.Format("{0} s", gameTime);
        }

        private void GameChecker_Tick(object sender, EventArgs e)
        {
            string address = gis.GetAddress().FirstOrDefault();
            tbGame.Text = isHotIP && !string.IsNullOrWhiteSpace(address) ? "HOT IP" : (address ?? "Lobby");
            bool _isHotIP = false;

            if (!string.IsNullOrWhiteSpace(address))
            {
                if (currentGame != address)
                {
                    gameTime = 0;
                    gameTimer.Start();
                    currentGame = address;
                    _isHotIP = api.sendGameIp(currentGame, tbToken.Text);
                }
                else if (checkerIteration % 5 == 0)
                {
                    _isHotIP = api.sendGameIp(currentGame, tbToken.Text);
                }
            }
            else
            {
                if (gameTimer.Enabled || checkerIteration % 5 == 0)
                {
                    api.sendGameIp("0.0.0.0", tbToken.Text);
                }
                isHotIP = false;
                tbSec.Text = "";
                gameTimer.Stop();
            }

            if (_isHotIP && !isHotIP) {
                isHotIP = true;
                MessageBox.Show("HOT IP");
            }
            checkerIteration++;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (!isStarted)
            {
                if (gis.ProcessId <= 0)
                {
                    tbGame.Text = "D2R not running";
                    return;
                }


                btnStart.Text = "Stop";
                gis.GetConnectedAddresses(true);
                isStarted = true;
                tbGame.Text = "Lobby";
                gameChecker.Start();
            }
            else
            {
                btnStart.Text = "I'm in lobby";
                isStarted = false;
                tbGame.Text = "Start in the lobby";
                gameChecker.Stop();
            }
        }

        private void btnVerify_Click(object sender, EventArgs e)
        {
            bool result = api.verify(tbToken.Text);
            if (result) MessageBox.Show("Ok");
            if (!result) MessageBox.Show("Bad");
        }
    }
}
