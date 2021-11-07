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

        private bool isHotIP = false;
        private GameIPService gis = new GameIPService();
        private PWEApi api = new PWEApi();

        private DateTime _lastGameCheck = DateTime.Now;
        private bool inLobby = true;
        private string lastAddresses = "";


        private DateTime _lastGameCreated = DateTime.Now;
        private DateTime _lastReportedGame = DateTime.Now;
        public Timer _appTimer { get; set; }

        public d2rpwe()
        {
            InitializeComponent();

            tbGame.Text = "Lobby / Wrong IP pool";

            _appTimer = new Timer();
            _appTimer.Interval = 1000;
            _appTimer.Tick += _appTimer_Tick;
            _appTimer.Start();


            TextBox.CheckForIllegalCrossThreadCalls = false;
        }

        private void _appTimer_Tick(object sender, EventArgs e)
        {
            GameCreateTimer();
            bool isGameRunning = GameNotRunning();

            if (isGameRunning)
            {
                GameReport();
            }
        }

        private const string NO_GAME_ADDRESS = "0.0.0.0";
        private string _lastAddress = NO_GAME_ADDRESS;

        private void GameCreateTimer()
        {
            tbSec.Text = string.Format("{0} s", Math.Round((DateTime.Now - _lastGameCreated).TotalSeconds));
        }

        private void GameReport()
        {
            if (Math.Round((DateTime.Now - _lastGameCheck).TotalSeconds) > 3)
            {
                _lastGameCheck = DateTime.Now;
                Task.Run(() =>
                {
                    var ips = gis.getGameConnections();
                    string ipAddresses = String.Join(";", ips.Distinct());
                    var gameIp = _lastAddress;
                    var _isHotIP = isHotIP;

                    if (lastAddresses != ipAddresses || Math.Round((DateTime.Now - _lastReportedGame).TotalSeconds) > 15)
                    {
                        lastAddresses = ipAddresses;
                        var result = api.sendGameIp(ipAddresses, gis.ProcessId, tbToken.Text);
                        _lastReportedGame = DateTime.Now;
                        gameIp = result.GameIp;
                        _isHotIP = result.IsHotIp;
                    }

                    if (_isHotIP && !isHotIP)
                    {
                        isHotIP = true;
                        tbGame.Text = "HOT IP";

                        System.Media.SoundPlayer player = new System.Media.SoundPlayer(Properties.Resources.beep);
                        player.Play();
                    }
                    else
                    {
                        isHotIP = _isHotIP;
                        if (!_isHotIP)
                        {
                            tbGame.Text = gameIp == NO_GAME_ADDRESS ? "Lobby / Wrong IP pool" : gameIp;
                        }
                    }


                    if (gameIp == NO_GAME_ADDRESS)
                    {
                        inLobby = true;
                        _lastAddress = gameIp;
                    }
                    else
                    {
                        if (inLobby || gameIp != _lastAddress)
                        {
                            _lastGameCreated = DateTime.Now;
                            _lastAddress = gameIp;
                        }
                        inLobby = false;
                    }

                });
            }
        }

        private bool GameNotRunning()
        {
            if (gis.ProcessId <= 0)
            {
                tbGame.Text = "D2R not running";
                return false;
            }

            return true;
        }


        private void btnVerify_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                var result = api.verify(gis.ProcessId, tbToken.Text);
                MessageBox.Show(!string.IsNullOrEmpty(result.Message) ? result.Message : (result.IsSuccess ? "Valid!" : "Bad"), "Verification");

                if (result.IsSuccess)
                {
                    MessageBox.Show("Now the tool will play the HOT IP alert sound!\r\nHit OK to listen to the test.", "Hot IP sound alert test");
                    System.Media.SoundPlayer player = new System.Media.SoundPlayer(Properties.Resources.beep);
                    player.Play();
                }
            });
        }
    }
}
