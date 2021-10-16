using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace d2r_pwe_client.D2R
{
    public class GameIPService
    {
        private int _processId { get; set; }
        public int ProcessId
        {
            get
            {
                Process[] processes = Process.GetProcessesByName("D2R");
                this._processId = processes.Length <= 0 ? 0 : processes[0].Id;
                return this._processId;
            }
        }

        private List<string> baselineAddresses = new List<string>();
        public List<string> GetConnectedAddresses(bool baseline = false)
        {
            if (this.ProcessId <= 0)
            {
                return new List<string>();
            }

            List<string> addressList = new List<string>();

            using (Process process = new Process())
            {
                process.StartInfo.FileName = "netstat.exe";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.Arguments = "-ano";
                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                process.Start();


                using (StreamReader reader = process.StandardOutput)
                {
                    string output = reader.ReadToEnd();
                    process.WaitForExit();

                    string[] lines = output.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

                    foreach (string line in lines)
                    {
                        string[] entries = line.Split(' ');
                        if (entries.Length < 5) continue;
                        if (entries.Any(s => s.Contains("Proto"))) continue;


                        List<string> validEntries = new List<string>();

                        foreach (string entry in entries)
                        {
                            if (entry.Trim() == "") continue;

                            validEntries.Add(entry);
                        }

                        if (validEntries.Count < 5)
                        {
                            continue;
                        }

                        if (validEntries[4] == this.ProcessId.ToString())
                        {
                            string address = validEntries[2].Split(':')[0];
                            addressList.Add(address);
                        }
                    }
                }
            }

            if (baseline)
            {
                baselineAddresses.Clear();
                baselineAddresses = addressList;
            }

            return addressList;
        }

        public List<string> GetAddress()
        {
            List<string> serverAddresses = new List<string>();

            List<string> connectedAddresses = GetConnectedAddresses();

            foreach (string address in connectedAddresses)
            {
                if (!baselineAddresses.Any(s => s.Contains(address)))
                {
                    serverAddresses.Add(address);
                }
            }

            return serverAddresses;
        }
    }
}
