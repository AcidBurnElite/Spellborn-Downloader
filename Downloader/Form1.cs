using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Threading;
using System.Windows.Forms;
using System.IO;
using IOfile=System.IO.File;
using System.Net;
using System.Windows;
using System.Runtime.InteropServices;
using System.IO.Compression;
using System.Runtime.InteropServices;
using Shell32;
using IWshRuntimeLibrary;



namespace Downloader
{
    
    public partial class Form1 : Form
    {
#region Handel Window Move
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd,int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
#endregion

#region Variables
        string RegValStr = null;
        string SBRegPath = null;
        string SBRegPath1 = null;
        string SBRegPath2 = null;
        string RecomendedSetup = null;
        //string SBDownloadPath = @"c:\";
        string SBDownloadPath = null;
        string SBDownloadPath1 = null;
        string SBInstallPath = null;
        string SBInstallPath1 = null;
        string SBInstallPath2 = null;
        string SBPathSelected = null;
        string SBUpdatePath = null;
        string SBDisplayName = null;
        string SBDisplayName1 = null;
        string SBDisplayName2 = null;
        string SelectedSetup;
        string selectedPath = null;
        string txtstr = "";
        string txtstr1, txtstr2 = "";
        bool setupok;
        string TextToDisplay=""+Environment.NewLine;
        int DownloadCount;
        int DownloadCount1;
        int DownloadCount2;
        int TotalDownloadCount;
        string FileDownloadStatus;

        string keypath1 = "HKEY_LOCAL_MACHINE\\SOFTWARE\\WOW6432Node\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\The Chronicles of Spellborn_is1";
        string keypath2 = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\The Chronicles of Spellborn_is1";
        List<string> DownloadsClient = new List<string>();
        List<string> DownloadsAltCient = new List<string>();
        List<string> DownloadsLauncher = new List<string>();
        List<string> DownloadsAltLauncher = new List<string>(); // No locaton available a.t.m.
        List<string> DownloadsToUse = new List<string>();
       
        
        

#endregion

#region Form

        public Form1()
        {
            InitializeComponent();
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            label2.Text = "Version: 1.0.0" + Environment.NewLine + "By: Acid Burn" + Environment.NewLine;
            if (Environment.Is64BitOperatingSystem)
            {
                //return Environment.GetEnvironmentVariable("SysWOW64");
                SBInstallPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + @"\The Chronicles of Spellborn";
            }
            else
            {
                //return Environment.GetEnvironmentVariable("system32");
                SBInstallPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + @"\The Chronicles of Spellborn";
            }
            textBox1.Text = SBDownloadPath;
            textBox2.Text = SBInstallPath;
            DownloadsClient.Add(@"https://spellborn.org/files/tcos_eu_0.9.0.0_setup-1a.bin");
            //DownloadsClient.Add(@"https://spellborn.org/files/tcos_eu_0.9.0.00000000000000000000000000000_setup-1a.bin");
            DownloadsClient.Add(@"https://spellborn.org/files/tcos_eu_0.9.0.0_setup-1b.bin");
            //DownloadsClient.Add(@"https://spellborn.org/files/tcos_eu_0.9.0.00000000000000000000000000000_setup-1b.bin");
            DownloadsClient.Add(@"https://spellborn.org/files/tcos_eu_0.9.0.0_setup-1c.bin");
            //DownloadsClient.Add(@"https://spellborn.org/files/tcos_eu_0.9.0.00000000000000000000000000000_setup-1c.bin");
            DownloadsClient.Add(@"https://spellborn.org/files/tcos_eu_0.9.0.0_setup.exe");
            //DownloadsClient.Add(@"https://spellborn.org/files/tcos_eu_0.9.0.00000000000000000000000000000_setup.exe");
            DownloadsLauncher.Add(@"https://spellborn.org/files/Launcher.zip");

            DownloadsAltCient.Add(@"http://www.spellborn.eu/Spellborn%20Storage/Clients/0.9.0.0/tcos_eu_0.9.0.0_setup-1a.bin");
            DownloadsAltCient.Add(@"http://www.spellborn.eu/Spellborn%20Storage/Clients/0.9.0.0/tcos_eu_0.9.0.0_setup-1b.bin");
            DownloadsAltCient.Add(@"http://www.spellborn.eu/Spellborn%20Storage/Clients/0.9.0.0/tcos_eu_0.9.0.0_setup-1c.bin");
            DownloadsAltCient.Add(@"http://www.spellborn.eu/Spellborn%20Storage/Clients/0.9.0.0/tcos_eu_0.9.0.0_setup.exe");
            DownloadsAltLauncher.Add(@"http://www.spellborn.eu/Spellborn%20Storage/Launcher/Launcher.zip");
        }

#endregion

#region Buttons

        private void button_exit_Click(object sender, EventArgs e)
        {
            //Exit in Panel2
            this.Close();
        }

        private void button_start_Click(object sender, EventArgs e)
        {
            //Next in Panel2
            panel3.Visible = true; panel2.Visible = false;
        }

        private void button_previous_1_Click(object sender, EventArgs e)
        {
            //back in Panel3
            panel2.Visible = true; panel3.Visible = false;
        }

        private void button_next_1_Click(object sender, EventArgs e)
        {
            //Next in Panel3
            
            switch (SelectedSetup)
            {
                case "Full Install":
                    {
                        panel4.Visible = true; panel3.Visible = false;
                        label10.Text = "Please select in both cases, a root folder in wich we will create a subfolder." + Environment.NewLine + Environment.NewLine + "The total installation will require a total 11,54GB of free disk space.";
                        break;
                    }
                case "Select":
                    {
                        radioButton3.Text = SBInstallPath1;
                        radioButton4.Text = SBInstallPath2;
                        panel5.Visible = true; panel3.Visible = false;
                        break;
                    }
                case "Update":
                    {
                        panel7.Visible = true; panel3.Visible = false;
                        break;
                    }

            }
            
        }

        private void button_previous_2_Click(object sender, EventArgs e)
        {
            //Back in Panel4
            panel3.Visible = true; panel4.Visible = false;
        }

        private void button_startchecks_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd1 = new FolderBrowserDialog();
            fbd1.ShowNewFolderButton = false;
            fbd1.SelectedPath = textBox1.Text;           
            
            if (fbd1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string str = fbd1.SelectedPath;
                if (str.EndsWith(@"\"))
                {
                    str = str.Remove(str.Length - 1);
                    fbd1.SelectedPath = str;
                }
                textBox1.Text = fbd1.SelectedPath + @"\The Chronicles of Spellborn Setup Files";
                SBDownloadPath = fbd1.SelectedPath;
                SBDownloadPath1 = fbd1.SelectedPath + "\\The Chronicles of Spellborn Setup Files";
                selectedPath = SBDownloadPath1;
            }
            
        }

        private void button8_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd2 = new FolderBrowserDialog();
            fbd2.ShowNewFolderButton = false;
            fbd2.SelectedPath = textBox2.Text;

            if (fbd2.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string str=fbd2.SelectedPath;
                if (str.EndsWith(@"\"))
                
                {
                    str=str.Remove(str.Length -1 );
                    fbd2.SelectedPath=str;
                }
                textBox2.Text = fbd2.SelectedPath + @"\The Chronicles of Spellborn";
                SBInstallPath = textBox2.Text;
                SBInstallPath1 = fbd2.SelectedPath + "\\The Chronicles of Spellborn";
            }
                
        }

        private void button9_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void radioButton1_Click(object sender, EventArgs e)
        {
            if (RecomendedSetup == "SelectOrFullSetup")
            {
                SelectedSetup = "Select";
                button4.Text = "Select";
                button4.Visible = true;
            }
            else
            {
                if (RecomendedSetup == "ReuseOrFullSetup")
                {
                    SelectedSetup = "Update";
                    button4.Text = "Update";
                    button4.Visible = true;
                }
            }
        }

        private void radioButton2_Click(object sender, EventArgs e)
        {
            SelectedSetup = "Full Install";
            button4.Text = "Full Install";
            button4.Visible = true;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            panel4.Visible = false; panel7.Visible = true;             
        }

        private void button10_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
        }

        private void panel2_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
        }

        private void panel3_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
        }

        private void panel4_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
        }

        private void panel5_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
        }

        private void panel6_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
        }

        private void panel7_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
        }
        
        private void button11_Click(object sender, EventArgs e)
        {
            panel5.Visible = false; panel3.Visible = true;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            selectedPath = radioButton3.Text;
            button12.Visible = true;
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            selectedPath = radioButton4.Text;
            button12.Visible = true;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            panel5.Visible = false; panel7.Visible = true;
        }
#endregion

#region System Checks

#region Check if a registry exists
        public static bool RegistryKeyExists(String path)
        {
            RegistryKey regkey = null;

            foreach (String key in path.Split('\\'))
            {
                switch (key)
                {
                    case "HKEY_LOCAL_MACHINE":
                        regkey = Registry.LocalMachine;
                        continue;

                    case "HKEY_CLASSES_ROOT":
                        regkey = Registry.ClassesRoot;
                        continue;

                    case "HKEY_CURRENT_USER":
                        regkey = Registry.CurrentUser;
                        continue;

                    case "HKEY_USERS":
                        regkey = Registry.Users;
                        continue;

                    case "HKEY_CURRENT_CONFIG":
                        regkey = Registry.CurrentConfig;
                        continue;

                    default:
                        if (regkey == null) return false;
                        regkey = regkey.OpenSubKey(key);
                        break;
                }
            }

            return regkey != null;
        }
#endregion

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            // Do not use any GUI components here. Just do some work.            


            // Check if a registry is found
            bool RgVal1 = RegistryKeyExists(keypath1);
            bool RgVal2 = RegistryKeyExists(keypath2);

            bool str1, str2;


            #region IF No Registry Keys found
            if ((RgVal1 == false) & (RgVal2 == false)) // No Registry Keys found
            {
                RegValStr = "No usable installations found.";
                RecomendedSetup = "FullSetup";
            }
            #endregion
            else
            {
                #region IF Two Registry Keys found
                if ((RgVal1 == true) & (RgVal2 == true)) // Two Registry Keys found 
                {
                    //RegValStr = "Two installations found.";
                    
                    SBRegPath1 = Registry.GetValue(keypath1, "InstallLocation", "").ToString();
                    SBRegPath2 = Registry.GetValue(keypath2, "InstallLocation", "").ToString();
                    SBDisplayName1 = Registry.GetValue(keypath1, "DisplayName", "").ToString();
                    SBDisplayName2 = Registry.GetValue(keypath2, "DisplayName", "").ToString();
                    bool SBDispCheck1 = SBDisplayName1.Contains("0.9.0");
                    bool SBDispCheck2 = SBDisplayName2.Contains("0.9.0");
                    str1 = Directory.Exists(SBRegPath1);
                    str2 = Directory.Exists(SBRegPath2);
                    #region No usable installation - Do full setup
                    //checking if the paths still exist.
                    if ((str1 == false) & (str2 == false))
                    {
                        RegValStr = "No usable installation was found.";
                        //RegValStr = RegValStr + " None of the corresponding paths were found.";
                        RecomendedSetup = "FullSetup";
                    }
                    #endregion
                    else
                    {
                        if ((str1 == true) & (str2 == true))
                        {
                            //RegValStr = RegValStr + " Both corresponding paths were found.";
                            //Checking version
                            #region No usable installation - Do full setup
                            if ((SBDispCheck1 == false) & (SBDispCheck2 == false))
                            {
                                //Both paths have the wrong version
                                RegValStr = "No usable installation was found.";
                                RecomendedSetup = "FullSetup";
                            }
                            #endregion
                            else
                            {
                                if ((SBDispCheck1 == true) & (SBDispCheck2 == true))
                                {
                                    //Both paths have the right version
                                    RegValStr = "Two usable installation were found.";
                                    RecomendedSetup = "SelectOrFullSetup";
                                    SBInstallPath1 = SBRegPath1;
                                    SBInstallPath2 = SBRegPath2;
                                }
                                else
                                {
                                    if (SBDispCheck1 == true)
                                    {
                                        RegValStr = "A usable installation was found.";
                                        RecomendedSetup = "ReuseOrFullSetup";
                                        SBInstallPath1 = SBRegPath1;
                                        SBUpdatePath = SBInstallPath1;
                                    }
                                    else
                                    {
                                        RegValStr = "A usable installation was found.";
                                        RecomendedSetup = "ReuseOrFullSetup"; 
                                        SBInstallPath2 = SBRegPath2;
                                        SBUpdatePath = SBInstallPath2;
                                    }
                                }
                            }                            
                        }
                        else //Not all paths are corresponding
                        {
                            if (str1 == true)
                            {
                                //RegValStr = RegValStr + " Only one corresponding paths was found.";
                                if (SBDispCheck1 == true) //right version
                                {
                                    RegValStr = "A usable installation was found.";
                                    RecomendedSetup = "ReuseOrFullSetup";
                                    SBInstallPath1 = SBRegPath1;
                                    SBUpdatePath = SBInstallPath1;
                                }
                                else
                                {
                                    RegValStr = "No usable installation was found.";
                                    RecomendedSetup = "FullSetup";
                                    //SBInstallPath2 = SBRegPath2;
                                }
                            }
                            else
                            {
                                //RegValStr = RegValStr + " Only one corresponding paths was found.";
                                if (SBDispCheck2 == true) //right version
                                {
                                    RegValStr = "A usable installation was found.";
                                    RecomendedSetup = "ReuseOrFullSetup";
                                    SBInstallPath2 = SBRegPath2;
                                    SBUpdatePath = SBInstallPath2;
                                }
                                else
                                {
                                    RegValStr = "No usable installation was found.";
                                    RecomendedSetup = "FullSetup";
                                    //SBInstallPath2 = SBRegPath2;
                                }
                            }
                        }
                    }


                }
                #endregion
                else
                {
                    #region IF Only One Registry Key found using WOW6432Node
                    if (RgVal1 == true)
                    {
                        SBRegPath = Registry.GetValue(keypath1, "InstallLocation", "").ToString();
                        SBDisplayName1 = Registry.GetValue(keypath1, "DisplayName", "").ToString();
                        bool SBDispCheck1 = SBDisplayName1.Contains("0.9.0");
                        //bool SBDispCheck2 = SBDisplayName2.Contains("0.9.0");

                        str1 = Directory.Exists(SBRegPath);
                        
                        //RegValStr = "One installation found using WOW6432Node.";
                        //checking if the paths still exist.
                        if (str1 == true)
                        {
                            //RegValStr = RegValStr + " The Corresponding path was found";
                            //RegValStr = RegValStr + "A previous installation was found.";
                            if (SBDispCheck1 == true) //right version
                            {
                                RegValStr = "A usable installation was found.";
                                RecomendedSetup = "ReuseOrFullSetup";
                                SBInstallPath1 = SBRegPath;
                                SBUpdatePath = SBInstallPath1;
                                selectedPath = SBUpdatePath;
                            }
                            else
                            {
                                RegValStr = "No usable installation was found.";
                                RecomendedSetup = "FullSetup";
                                //SBInstallPath2 = SBRegPath2;
                            }
                        }
                        else
                        {                            
                            RegValStr = "No usable installation found.";
                            RecomendedSetup = "FullSetup";
                        }
                    }
                    #endregion
                    else
                    #region ELSE Only One Registry Key found NOT using WOW6432Node
                    {
                        RegValStr = "One installation found.";

                        SBRegPath = Registry.GetValue(keypath2, "InstallLocation", "").ToString();
                        SBDisplayName2 = Registry.GetValue(keypath2, "DisplayName", "").ToString();
                        
                        bool SBDispCheck2 = SBDisplayName2.Contains("0.9.0");

                        str2 = Directory.Exists(SBRegPath);
                        
                        //checking if the paths still exist.
                        if (str2 == true)
                        {
                            
                            if (SBDispCheck2 == true) //right version
                            {
                                RegValStr = "A usable installation was found.";
                                RecomendedSetup = "ReuseOrFullSetup";
                                SBInstallPath1 = SBRegPath;
                                SBUpdatePath = SBInstallPath1;
                                selectedPath = SBUpdatePath;
                            }
                            else
                            {
                                RegValStr = "No usable installation found.";
                                RecomendedSetup = "FullSetup";
                                //SBInstallPath2 = SBRegPath2;
                            }
                        }
                        else
                        {
                            RegValStr = "No usable installation found.";
                            //RegValStr = RegValStr + " The Corresponding path was not found";
                            RecomendedSetup = "FullSetup";
                        }
                    }
                    #endregion
                }
            }
            backgroundWorker1.ReportProgress(100);
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // This is where GUI components get updated from DoWork.
            
            progressBar1.Value = e.ProgressPercentage;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //Called when the heavy operation is completed. Can accept GUI components.
            
            switch (RecomendedSetup)
            {
                case "FullSetup":
                    {
                        label6.Text = RegValStr + Environment.NewLine + "Recomended Action: Full Setup";
                        SelectedSetup = "Full Install";
                        button4.Text = "Full Install";
                        button4.Visible = true;

                        break;
                    }
                case "SelectOrFullSetup":
                    {
                        label6.Text = RegValStr + Environment.NewLine + "Recomended Action: Update a previous installation.";
                        label11.Visible = true;
                        groupBox1.Visible = true;
                        radioButton1.Text = "Update a previous installation";
                        radioButton2.Text = "Full install";
                        break;
                    }
                case "ReuseOrFullSetup":
                    {
                        label6.Text = RegValStr + Environment.NewLine + "Recomended Action: Update the current installation.";
                        label11.Visible = true;
                        groupBox1.Visible = true;
                        radioButton1.Text = "Update the current installation";
                        radioButton2.Text = "Full install";                        
                        break;
                    }
                case null:
                    {
                        
                        break;
                    }
            }
        }

#endregion      



#region Install Spellborn
        public virtual bool InstallSpellborn(string InstallApp, string InstallArgs)
        {
            
            System.Diagnostics.Process installProcess = new System.Diagnostics.Process();
            //settings up parameters for the install process
            installProcess.StartInfo.FileName = InstallApp;
            installProcess.StartInfo.Arguments = InstallArgs;            
            installProcess.Start();            
            installProcess.WaitForExit();            
            // Check for sucessful completion            
            return (installProcess.ExitCode == 0) ? true : false;
        }
        public void LaunchSetup()
        {
            const string quote = "\"";
            bool sfb;
                string sf = textBox1.Text + @"\tcos_eu_0.9.0.0_setup.exe";
                //string sfarg = "/VERYSILENT /LOG /DIR=" + quote + SBInstallPath1 + quote;
                string sfarg = "/SILENT /NOCANCEL /LOG /DIR=" + quote + SBInstallPath + quote;
                if ((sfb = IOfile.Exists(sf)) == true)
                {
                    InstallSpellborn(sf, sfarg);
                }
        }
#endregion

#region Handel ZIP
        public void UnzipLauncher(string ZIPLocation,string ZIPExtractionDestination)
        {
            bool spfb;
            //Extract ZIP file
            string spf = ZIPLocation + @"\Launcher.zip";
            //string spfe = SBInstallPath1 + quote + @"\bin\client\";
            string spfe = ZIPExtractionDestination + @"\bin\client\";
            //MessageBox.Show("selectedPath(spf):" + spf + Environment.NewLine + "SBInstallPath(spfe):" + spfe);
            if ((spfb = IOfile.Exists(spf)) == true)
            {
                ZipFile.ExtractToDirectory(spf, spfe);
            }
        }
#endregion

#region Download Actions

        public void BatchDowload()
        {            
            //Build list of files to download
            //DownloadsToUse.Clear();
            
            switch (SelectedSetup)
            {
                #region Case Full Setup
                case "Full Install":
                    {
                        DownloadsToUse.Clear();
                        const string quote = "\"";
                        //Checking first the client URIs                        
                        CheckClientURI();
                        //Checking Launcher URIs
                        CheckLauncherUri();
                        //Downloading
                        HandelingDownloads();
                        //Install Spellborn
                        backgroundWorker2.RunWorkerAsync();
                        button9.Visible = true;
                        break;
                    }
                #endregion
                case "Select":
                    {
                        break;
                    }
                case "Update":
                    {
                        DownloadsToUse.Clear();
                        const string quote = "\"";
                        //Checking Launcher URIs
                        CheckLauncherUri();
                        //Downloading
                        HandelingDownloads();
                        //Install Spellborn
                        backgroundWorker2.RunWorkerAsync();
                        button9.Visible = true;
                        break;
                    }
            }
        }

        public bool RemoteDownloadExists(string url)
        {
            try
            {
                //Creating the HttpWebRequest
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                //Setting the Request method HEAD, you can also use GET too.
                request.Method = "HEAD";
                //Getting the Web Response.
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                //Returns TRUE if the Status code == 200
                response.Close();               
                //return (response.StatusCode == HttpStatusCode.OK);
                return true;
            }
            catch
            {
                //Any exception will returns false.
                return false;
            }
        }

        public void HandelingDownloads()
        {
            foreach (string DLActive in DownloadsToUse)
            {
                //label14.Text = "Downloading:" + Environment.NewLine + DLActive;
                DownloadThisFile(DLActive, selectedPath);
            }
            //break;
        }

        public void CheckLauncherUri()
        {
            DownloadCount1 = DownloadsLauncher.Count;
            DownloadCount2 = DownloadsAltLauncher.Count;
            if (DownloadCount1 > DownloadCount2)
            {
                DownloadCount = DownloadCount1;
            }
            else DownloadCount = DownloadCount2;
            TotalDownloadCount = DownloadsAltCient.Count + DownloadsAltLauncher.Count;


            for (int i = 0; i < DownloadCount; i++)
            {
                string s1, s2;
                bool sr1, sr2;

                if ((i > DownloadCount1) & (i > DownloadCount2))
                {
                    s1 = null;
                    sr1 = false;
                    s2 = null;
                    sr2 = false;
                    //MessageBox.Show("Not Downloading any files");
                }
                else
                {
                    if ((i <= DownloadCount1) & (i <= DownloadCount2))
                    {
                        //sr1=true;
                        //sr2 = true;
                        s1 = DownloadsLauncher[i];
                        sr1 = RemoteDownloadExists(s1);
                        s2 = DownloadsAltLauncher[i];
                        sr2 = RemoteDownloadExists(s2);
                        //MessageBox.Show("Downloading possible from both locations.");
                    }
                    else
                    {
                        if ((i <= DownloadCount1) & (i > DownloadCount2))
                        {
                            s1 = DownloadsLauncher[i];
                            sr1 = RemoteDownloadExists(s1);
                            s2 = null;
                            sr2 = false;
                            //MessageBox.Show("Downloading only possible from primary loaction.");
                        }
                        else
                        {
                            s1 = null;
                            sr1 = false;
                            s2 = DownloadsAltLauncher[i];
                            sr2 = RemoteDownloadExists(s2);
                            //MessageBox.Show("Downloading only possible from secondary loaction.");
                        }
                    }
                }


                if ((sr1 == false) & (sr2 == false))
                {
                    label14.Text = "Unable to install the Spellborn Launcher" + Environment.NewLine + "Some files are not available for download at this momment." + Environment.NewLine + "Please tray again later.";
                    setupok = false;
                    break;
                }
                else
                {
                    if ((sr1 == true) & (sr2 == true))
                    {
                        //using primary download
                        DownloadsToUse.Add(s1);
                        TextToDisplay = TextToDisplay + s1 + Environment.NewLine;
                    }
                    else
                    {
                        if ((sr1 == true) & (sr2 == false))
                        {
                            DownloadsToUse.Add(s1);
                            TextToDisplay = TextToDisplay + s1 + Environment.NewLine;
                        }
                        else
                        {
                            DownloadsToUse.Add(s2);
                            TextToDisplay = TextToDisplay + s2 + Environment.NewLine;
                        }
                    }
                }
            }
        }

        public void CheckClientURI()
        {
            DownloadCount1 = DownloadsClient.Count;
            DownloadCount2 = DownloadsAltCient.Count;
            if (DownloadCount1 > DownloadCount2)
            {
                DownloadCount = DownloadCount1;
            }
            else DownloadCount = DownloadCount2;
            TotalDownloadCount = DownloadsAltCient.Count + DownloadsAltLauncher.Count;


            for (int i = 0; i < DownloadCount; i++)
            {
                string s1, s2;
                bool sr1, sr2;

                if ((i > DownloadCount1) & (i > DownloadCount2))
                {
                    s1 = null;
                    sr1 = false;
                    s2 = null;
                    sr2 = false;
                    //MessageBox.Show("Not Downloading any files");
                }
                else
                {
                    if ((i <= DownloadCount1) & (i <= DownloadCount2))
                    {
                        //sr1=true;
                        //sr2 = true;
                        s1 = DownloadsClient[i];
                        sr1 = RemoteDownloadExists(s1);
                        s2 = DownloadsAltCient[i];
                        sr2 = RemoteDownloadExists(s2);
                        //MessageBox.Show("Downloading possible from both locations.");
                    }
                    else
                    {
                        if ((i <= DownloadCount1) & (i > DownloadCount2))
                        {
                            s1 = DownloadsClient[i];
                            sr1 = RemoteDownloadExists(s1);
                            s2 = null;
                            sr2 = false;
                            //MessageBox.Show("Downloading only possible from primary loaction.");
                        }
                        else
                        {
                            s1 = null;
                            sr1 = false;
                            s2 = DownloadsAltCient[i];
                            sr2 = RemoteDownloadExists(s2);
                            //MessageBox.Show("Downloading only possible from secondary loaction.");
                        }
                    }
                }
                if ((sr1 == false) & (sr2 == false))
                {
                    label14.Text = "Unable to install The Chronicles ofSpellborn." + Environment.NewLine + "Some files are not available for download at this momment." + Environment.NewLine + "Please tray again later.";
                    setupok = false;
                    break;
                }
                else
                {
                    if ((sr1 == true) & (sr2 == true))
                    {
                        //using primary download
                        DownloadsToUse.Add(s1);
                        TextToDisplay = TextToDisplay + s1 + Environment.NewLine;
                        //label14.Text = "Downloading:"+Environment.NewLine+TextToDisplay;
                    }
                    else
                    {
                        if ((sr1 == true) & (sr2 == false))
                        {
                            DownloadsToUse.Add(s1);
                            TextToDisplay = TextToDisplay + s1 + Environment.NewLine;
                            //label14.Text = "Downloading:" + Environment.NewLine + TextToDisplay;
                        }
                        else
                        {
                            DownloadsToUse.Add(s2);
                            TextToDisplay = TextToDisplay + s2 + Environment.NewLine;
                            //label14.Text = "Downloading:" + Environment.NewLine + TextToDisplay;
                        }
                    }

                }
            }
        }

        private bool downloadComplete = false;
        
        public void DownloadThisFile(string SBfile,string DownloadPath)
        {
            bool SBDownloadPathChk;
            if ((SBDownloadPathChk = Directory.Exists(DownloadPath)) != true)
            {             
                //create folder
                Directory.CreateDirectory(DownloadPath);
            }

            Uri FUri = new Uri(SBfile);
            string SBfilename = System.IO.Path.GetFileName(FUri.AbsolutePath);
            
            WebClient client = new WebClient();
            client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
            client.DownloadFileCompleted += new AsyncCompletedEventHandler(client_DownloadFileCompleted);

            client.DownloadFileAsync(FUri, DownloadPath + @"\" + SBfilename);
            //return (client. .ExitCode == 0) ? true : false;
            while (!downloadComplete)
            {
                Application.DoEvents();
            }

            downloadComplete = false;
         
        }        

        private void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            //throw new NotImplementedException();
            label14.Text = "File download completed.";
            this.BeginInvoke((MethodInvoker)delegate
            {
                //textBoxLog.AppendText("OK");
                downloadComplete = true;
            });
            
        }

        private void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            //throw new NotImplementedException();
            
            progressBar2.Maximum =100;
            progressBar2.Value = e.ProgressPercentage;
            label14.Text = e.ProgressPercentage.ToString()+"% completed.";
            //FileDownloadStatus = e.ProgressPercentage;
        }
        
#endregion
        
        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            if (SelectedSetup == "Full Install")
            {
                LaunchSetup();
                //MessageBox.Show("selectedPath:" + selectedPath + Environment.NewLine + "SBInstallPath:" + SBInstallPath1);
                backgroundWorker2.ReportProgress(90);
                UnzipLauncher(selectedPath, SBInstallPath);
                string LauncherExePath = SBInstallPath + @"\bin\client";
                MessageBox.Show("Spellborn Launcher" + Environment.NewLine + Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + Environment.NewLine + LauncherExePath + @"\\SpellbornLauncher.exe" + Environment.NewLine + LauncherExePath);
                //CreateShortcut("Spellborn Launcher", Environment.GetFolderPath(Environment.SpecialFolder.Desktop), SBInstallPath + @"bin\client\SpellbornLauncher.exe",LauncherExePath);
                CreateShortcut("Spellborn Launcher", Environment.GetFolderPath(Environment.SpecialFolder.Desktop), LauncherExePath + @"\\SpellbornLauncher.exe", LauncherExePath);
                backgroundWorker2.ReportProgress(100);
            }

            if ((SelectedSetup == "Select") || (SelectedSetup == "Update"))
            {
                //MessageBox.Show("selectedPath:" + selectedPath + Environment.NewLine);
                UnzipLauncher(selectedPath, selectedPath);
                string LauncherExePath=selectedPath+ @"bin\client";
                CreateShortcut("Spellborn Launcher", Environment.GetFolderPath(Environment.SpecialFolder.Desktop), selectedPath + @"bin\client\SpellbornLauncher.exe",LauncherExePath);
                backgroundWorker2.ReportProgress(100);
            }
        }

        private void backgroundWorker2_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // This is where GUI components get updated from DoWork.
            progressBar3.Value = e.ProgressPercentage;
        }

        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //Called when the heavy operation is completed. Can accept GUI components.

        }

        //public static void CreateShortcut(string shortcutName, string shortcutPath, string targetFileLocation)
        public static void CreateShortcut(string shortcutDisplayName, string shortcutDestinationPath, string targetFileWithPath,string startInPath)    
        {
            string targetFileLocation = targetFileWithPath;

            //Build the shortcutLocation that will be used to store the location plus the filename to save it:
            string shortcutSaveLocation = System.IO.Path.Combine(shortcutDestinationPath, shortcutDisplayName + ".lnk");

            // Create a new instance of WshShellClass
            WshShell shell = new WshShell();

            //shortcutLocation is the location plus the filename to save it:
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutSaveLocation); 
            
            // Description for the shortcut
            shortcut.Description = "Spellborn Launcher";   // The description of the shortcut
            
            // Location for the shortcut's icon
            shortcut.IconLocation = targetFileLocation;           // The icon of the shortcut

            // Where the shortcut should point to:
            shortcut.TargetPath = targetFileLocation;                 // The path of the file that will launch when the shortcut is run (Path to the EXEcutable including filename)
            shortcut.WorkingDirectory = startInPath;
            // Create the shortcut at the given path
            shortcut.Save();                                    // Save the shortcut
        }
#region Start Processing....

        private void panel7_VisibleChanged(object sender, EventArgs e)
        {
            switch (SelectedSetup)
            {
                case "Full Install":
                    {
                        label12.Text = "Processing...";
                        DownloadsToUse.Clear();
                        const string quote = "\"";                     
                        CheckClientURI();
                        CheckLauncherUri();
                        HandelingDownloads();
                        backgroundWorker2.RunWorkerAsync();
                        button9.Visible = true;
                        break;
                    }

                case "Select":
                    {
                        label12.Text = " Updating... ";
                        DownloadsToUse.Clear();
                        const string quote = "\"";
                        CheckLauncherUri();
                        HandelingDownloads();
                        backgroundWorker2.RunWorkerAsync();
                        button9.Visible = true;                        
                        break;
                    }
                case "Update":
                    {                        
                        label12.Text = " Updating... ";
                        DownloadsToUse.Clear();
                        const string quote = "\"";
                        CheckLauncherUri();
                        HandelingDownloads();                       
                        backgroundWorker2.RunWorkerAsync();
                        button9.Visible = true;
                        break;
                    }
                default:
                    break;
            }
        }
#endregion

        
    }
}
