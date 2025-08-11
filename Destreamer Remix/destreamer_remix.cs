using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Linq.Expressions;
using System.Net;
using Destreamer_Remix.Properties;
using System.Diagnostics;

namespace Destreamer_Remix
{
    public partial class destreamer_remix : Form
    {
        public destreamer_remix()
        {
            InitializeComponent();
        }
            // Cose per il pannello
            private bool drag;
            private int mousex;
            private int mousey;
            private void panel_MouseDown(object sender, MouseEventArgs e)
            {
                drag = true;
                mousex = Cursor.Position.X - this.Left;
                mousey = Cursor.Position.Y - this.Top;
                Cursor = Cursors.SizeAll;
            }
            private void panel_MouseMove(object sender, MouseEventArgs e)
            {
                if (drag)
                {
                    this.Top = Cursor.Position.Y - mousey;
                    this.Left = Cursor.Position.X - mousex;
                }
            }
            private void panel_MouseUp(object sender, MouseEventArgs e)
            {
                drag = false;
                Cursor = Cursors.Default;
            }
        //Fine cose per il pannello

        //Alla chiusura
        private void destreamer_remix_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                Process[] procs = Process.GetProcessesByName("ffmpeg");
                foreach (Process p in procs) { p.Kill(); }
            } catch { }
            
        }


        Panel latestpanel = null;
        private void Form1_Load(object sender, EventArgs e)
        {
            labelversion.Text = "versione " + Codici.versione.Insert(1, ".") + " (beta)";

            Codici.percorso = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Destreamer Remix";
            Codici.Elimina(Application.StartupPath + @"\download_temporaneo", false);
            Codici.Elimina(Application.StartupPath + @"\updatedes.bat", false);
            Codici.Elimina(Application.StartupPath + @"\DestreamerRemixupdate", false);

            Codici.ControlloAggiornamenti();

            if (Codici.LeggiOpzione("", "destreamer") == "") { Anima(0); return; }
            if (Codici.LeggiOpzione("", "path") == "") { Anima(1); return; }
            if (Isdeinst(Codici.LeggiOpzione("", "destreamer")) == false) { Anima(2); return; }

            Anima(5);
        }

        bool Isdeinst(string cartella)
        {
            bool valore = true;
            if (Directory.Exists(cartella))
            {
                if (File.Exists(cartella + @"\destreamer.exe") == false) valore = false;
                if (File.Exists(cartella + @"\ffmpeg.exe") == false) valore = false;
                //if (File.Exists(cartella + @"\.token_cache") == false) valore = false;
                if (Directory.Exists(cartella + @"\chromium") == false) valore = false;
            }
            else valore = false;

            return valore;
        }

        private void Anima(int pagina)
        {
            if (latestpanel != null) latestpanel.Visible = false;

            switch (pagina)
            {
                case 0:
                    welcomepanel1.Visible = true;
                    latestpanel = welcomepanel1;
                    next_btn.Visible = true;
                    break;
                case 1:
                    welcomepanel2.Visible = true;
                    latestpanel = welcomepanel2;
                    next_btn.Visible = true;
                    break;
                case 2:
                    label9.Text = label9.Text.Replace("[documents]", Codici.percorso + @"\Destreamer");
                    destreamerpath = Codici.percorso + @"\Destreamer";
                    welcomepanel3.Visible = true;
                    latestpanel = welcomepanel3;
                    next_btn.Visible = true;
                    break;
                case 3:
                    welcomepanel4.Visible = true;
                    latestpanel = welcomepanel4;
                    next_btn.Visible = false;
                    DownloadD();
                    break;
                case 4:
                    welcomepanel5.Visible = true;
                    latestpanel = welcomepanel5;
                    object O = Resources.ResourceManager.GetObject("checked");
                    next_btn.BackgroundImage = O as Image;
                    next_btn.Visible = true;
                    break;
                case 5:
                    panelstart.Visible = true;
                    latestpanel = panelstart;
                    next_btn.Visible = false;
                    email = Codici.LeggiOpzione("", "email");
                    //Imposta la clipboard
                    if (IsStreamLink(Clipboard.GetText(), false)) textlink.Text = Clipboard.GetText();
                    else IsStreamLink("", true);
                    break;
                case 6:
                    panelsettings.Visible = true;
                    latestpanel = panelsettings;
                    break;
                case 7:
                    panelnaming.Visible = true;
                    latestpanel = panelnaming;
                    OttieniInformazioni();
                    break;
                case 8:
                    paneldestreamer.Visible = true;
                    latestpanel = paneldestreamer;
                    ScaricaVideo();
                    break;
            }
        }

        private void next_btn_Click(object sender, EventArgs e)
        {
            if (welcomepanel1.Visible == true)
            {
                Anima(1);
                textsavepath.Text = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);
            }
            else if (welcomepanel2.Visible == true)
            {
                if (Directory.Exists(textsavepath.Text))
                {
                    Codici.SalvaOpzione("", "path", textsavepath.Text, false);
                    if (textemail.Text.Replace(" ", "").Contains("@")) Codici.SalvaOpzione("", "email", textemail.Text, false);

                    if (Isdeinst(Codici.LeggiOpzione("", "destreamer")) == false) Anima(2);
                    else Anima(4);
                }
                else
                {
                    MessageBox.Show("Non puoi impostare questo percorso! Il percorso è inesistente.", "Attenzione!", MessageBoxButtons.OK);
                }
            }
            else if (welcomepanel3.Visible == true)
            {
                Codici.Elimina(path_selector.SelectedPath + @"\destreamer.exe", false);
                Codici.Elimina(path_selector.SelectedPath + @"\ffmpeg.exe", false);
                Codici.Elimina(path_selector.SelectedPath + @"\.token_cache", false);
                Codici.Elimina(path_selector.SelectedPath + @"\chromium", true);
                Codici.SalvaOpzione("", "destreamer", destreamerpath, false);

                Anima(3);
            }
            else if (welcomepanel5.Visible == true)
            {
                Anima(5);
            }
        }

        private void closebtn_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void hidebtn_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void btnsavepath_Click(object sender, EventArgs e)
        {

            if (path_selector.ShowDialog() == DialogResult.OK)
            {
                textsavepath.Text = path_selector.SelectedPath;
            }
        }

        string destreamerpath;
        private void btndespath_Click(object sender, EventArgs e)
        {
            path_selector.Description = "Scegli il posto in cui vuoi salvare la configurazione di Destreamer.";
            if (path_selector.ShowDialog() == DialogResult.OK)
            {
                if (File.Exists(path_selector.SelectedPath + @"\Destreamer\destreamer.exe")) path_selector.SelectedPath = path_selector.SelectedPath + @"\Destreamer";

                if (Isdeinst(path_selector.SelectedPath) == true)
                {
                    destreamerpath = path_selector.SelectedPath;
                    if (MessageBox.Show("La cartella che hai selezionato contiene già l'installazione di Destreamer (Almeno sembra), vorresti utilizzare questa configurazione senza scaricarne una nuova?", "Configurazione già esistente", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        //Se la configurazione già esiste, vai avanti
                        Codici.SalvaOpzione("", "destreamer", destreamerpath, false);
                        Anima(4);
                    }
                }
                else { destreamerpath = path_selector.SelectedPath + @"\Destreamer"; }

                
                label9.Text = "Bene! Il nostro caro Destreamer sarà configurato in questa cartella: " + path_selector.SelectedPath;
                next_btn.Visible = true;
            }
        }


        private async void DownloadD()
        {
            await Codici.Downloader(Codici.link_destreamer, Application.StartupPath + @"\download_temporaneo", progressdownload, label13);
            
            label12.Text = label12.Text.Replace("Scarico", "Estraggo");
            label11.Text = label11.Text.Replace("scaricando", "estraendo");
            label13.Text = "Attendi ...";
            progressdownload.Style = ProgressBarStyle.Marquee;

            await Codici.Extract(Application.StartupPath + @"\download_temporaneo", Codici.LeggiOpzione("", "destreamer"));
            Anima(4);
        }



        //Parte del menù
        #region menu
        string email;

        private bool IsStreamLink(string link, bool setbuttons)
        {
            if (link.Contains(@"/web.microsoftstream.com/video/"))
            {
                if (setbuttons)
                {
                    object O = Resources.ResourceManager.GetObject("download");
                    pastebtn.BackgroundImage = O as Image;
                    pastelbl.Text = "Scarica";
                    panelpaste.Visible = true;
                }
                return true;
            }
            else
            {
                if (Clipboard.GetText() != textlink.Text && Clipboard.GetText().Replace(" ", "") != "" && setbuttons)
                {
                    object O = Resources.ResourceManager.GetObject("clipboard");
                    pastebtn.BackgroundImage = O as Image;
                    pastelbl.Text = "Incolla";
                    panelpaste.Visible = true;
                }
                else if ((Clipboard.GetText() == textlink.Text || Clipboard.GetText().Replace(" ", "") == "") && setbuttons)
                {
                    panelpaste.Visible = false;
                }
            }

            return false;
        }

        private void pastebtn_Click(object sender, EventArgs e)
        {
            if (pastelbl.Text == "Incolla")
            {
                textlink.Text = Clipboard.GetText();
            }
            else
            {
                Anima(7);
            }
        }

        private void textlink_TextChanged(object sender, EventArgs e)
        {
            IsStreamLink(textlink.Text, true);
        }

        private void textlink_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                pastebtn_Click(this, new EventArgs());
            }
        }

        private void settingsbtn_Click(object sender, EventArgs e)
        {
            textBoxemail.Text = email;
            textBoxvideo.Text = Codici.LeggiOpzione("", "path");
            Anima(6);
        }
        #endregion

        #region Impostazioni
        private void btnclosesettings_Click(object sender, EventArgs e)
        {
            timer1_Tick(null, null);
            Anima(5);
        }

        private async void Resetbutton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Se premi sì, cancellerai tutti i dati del programma e di Destreamer, ma i video scaricati non saranno eliminati. Premi sì se vuoi procedere.", "Sei sicuro/a di voler continuare?", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (MessageBox.Show("Questo è l'ultimo avvertimento. Premi nuovamente sì se vuoi ripristinare il programma.", "Ripristino?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Enabled = false;
                    resetlabel.Text = "Ripristino in corso...";
                    await Codici.Elimina(Codici.LeggiOpzione("", "destreamer"), true);
                    await Codici.Elimina(Codici.percorso, true);

                    if (Directory.Exists(Codici.percorso)) MessageBox.Show("Si è verificato un errore durante il ripristino del programma, premi Ok per chiudere questa finestra.", "C'è stato un problema...", MessageBoxButtons.OK);
                    else MessageBox.Show("Il programmo è stato ripristinato, premi Ok per riavviarlo.", "Tutto fatto", MessageBoxButtons.OK);

                    Application.Restart();
                }
            }
        }

        private void textBoxvideo_Click(object sender, EventArgs e)
        {
            if (path_selector.ShowDialog() == DialogResult.OK)
            {
                textBoxvideo.Text = path_selector.SelectedPath;
                Codici.SalvaOpzione("", "path", textBoxvideo.Text, false);
            }
        }
        

        private void textBoxemail_TextChanged(object sender, EventArgs e)
        {
            if (textBoxemail.Text != email) timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if ((email != textBoxemail.Text) && textBoxemail.Text.Replace(" ", "").Contains("@")) { Codici.SalvaOpzione("", "email", textBoxemail.Text, false); email = textBoxemail.Text; }

            timer1.Stop();
        }

        private async void buttoncheckupdate_Click(object sender, EventArgs e)
        {
            buttoncheckupdate.Enabled = false;
            buttoncheckupdate.Text = "Controllo in corso...";
            await Codici.ControlloAggiornamenti();
            buttoncheckupdate.Text = "Fatto!";
            buttoncheckupdate.Enabled = true;
        }

        #endregion

        #region naming
        bool namechanged;
        private void OttieniInformazioni()
        {
            namechanged = false;
            videoname = "";
            namingtext.Text = "";
            countdown = 10;
            labeltimerinfo.Visible = true;
            labeltimerinfo.Text = "Se non fai nulla procedo autonomamente tra " + countdown + " secondi...";
            timerinfo.Start();
        }

        int countdown;
        private void timerinfo_Tick(object sender, EventArgs e)
        {
            countdown -= 1;
            if (countdown < 1) labelinfonext_Click(null, null);
            labeltimerinfo.Text = "Se non fai nulla procedo autonomamente tra " + countdown + " secondi...";
            if (countdown == 1) labeltimerinfo.Text = labeltimerinfo.Text.Replace("secondi", "secondo");
        }

        private void namingtext_TextChanged(object sender, EventArgs e)
        {
            if (timerinfo.Enabled == true) { timerinfo.Stop(); labeltimerinfo.Visible = false; }
        }

        private void labelinfonext_Click(object sender, EventArgs e)
        {
            timerinfo.Stop();
            if (namingtext.Text.Replace(" ", "") != "") videoname = namingtext.Text;
            Anima(8);
        }
        #endregion

        #region destreamer
        string videoname = "";
        StringBuilder sb;
        private void ScaricaVideo()
        {
            //Impostiamo le scritte
            progressdestreamer.Visible = false;
            labeltitle.Text = "Avvio di Destreamer in corso...";
            labelsubtext.Text = "Ciò richiederà pochissimo tempo.";
            labelspeed.Text = "Calcolo in corso...";
            object O = Resources.ResourceManager.GetObject("download");
            picturetoshow.Image = O as Image;

            //Scrittura del file batch
            string outputfile = Codici.LeggiOpzione("", "destreamer") + @"\download.bat";
            string alternativetext = "";
            if (email != "") alternativetext = alternativetext + @" -u """ + email + @"""";
            if (videoname != "") alternativetext = alternativetext + @" -t '{" + "title" + @"}'";

            File.WriteAllText(outputfile, @"set link=""" + textlink.Text + @"""" + Environment.NewLine);
            File.AppendAllText(outputfile, @"destreamer.exe -k -x -v -i %link% -o """ + Codici.LeggiOpzione("", "path") + @"""" + alternativetext + Environment.NewLine);
            File.AppendAllText(outputfile, "echo [FATTOO]");

            timerdestreamer.Interval = 100;
            current = "";
            status = "";
            curstatus = "";

            sb = new StringBuilder();
                System.Diagnostics.ProcessStartInfo procStartInfo = new System.Diagnostics.ProcessStartInfo("cmd.exe", "/c download.bat");
                procStartInfo.WorkingDirectory = Codici.LeggiOpzione("", "destreamer");

                procStartInfo.RedirectStandardOutput = true;
                procStartInfo.RedirectStandardError = true;

                procStartInfo.UseShellExecute = false;
                procStartInfo.CreateNoWindow = true;
                System.Diagnostics.Process proc = new System.Diagnostics.Process();

                proc.EnableRaisingEvents = true;

                proc.StartInfo = procStartInfo;

                proc.OutputDataReceived += DoSomething;
                proc.ErrorDataReceived += Error;
                proc.Exited += (sender, args) => Finito();

                proc.Start();
                proc.BeginOutputReadLine();
                proc.BeginErrorReadLine();

            timerdestreamer.Start();
        }

        string current = "";
        string status = "";
        string curstatus = "";
        bool firsterror = false;

        void DoSomething(object sendingProcess, DataReceivedEventArgs outLine)
        {
            //Quando il testo viene aggiornato
            current = outLine.Data;
            Console.WriteLine(outLine.Data);
            if (current == null || status == "riprova") return;

            //---Speed: 131.8kbits / s, Cursor: 00:00:47.744000

            //Condizioni
            if (current.Contains("Fetching metadata..."))
            {
                status = "download";
                return;
            }
            if (current.Contains("Navigating to login page"))
            {
                status = "login";
                return;
            }
        }

        void Error(object sendingProcess, DataReceivedEventArgs outLine)
        {
            //Quando si è verificato un errore
            Console.WriteLine(outLine.Data);
            if (status != "" && status != "riprova") status = "errore";
        }

        private void timerdestreamer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (curstatus == "download" && current.Split('\n').Last().Contains("Speed: ")) labelspeed.Text = "Velocità: " + current.Split('\n').Last().Replace("--- Speed: ", "").Split(null).First().Replace(",", "");
            } catch { }
            
            if (status == curstatus && status != "riprova") return;

            labelspeed.Visible = false;

            switch (status)
            {
                case "login":
                    progressdestreamer.Visible = false;
                    labeltitle.Text = "E' necessario effettuare l'accesso!";
                    labelsubtext.Text = "A breve si aprirà il browser Chromium che chiederà di effettuare l'accesso al tuo account Microsoft (legato a Microsoft Teams).";
                    object O = Resources.ResourceManager.GetObject("chrome");
                    picturetoshow.Image = O as Image;
                    break;
                case "download":
                    labelspeed.Visible = true;
                    progressdestreamer.Visible = true;
                    labeltitle.Text = "Destreamer sta scaricando il video ...";
                    labelsubtext.Text = "Ciò richiederà un po' di tempo. Gustati un caffè nel frattempo!" + Environment.NewLine + "(ma anche 2)";
                    object O2 = Resources.ResourceManager.GetObject("direct-download");
                    picturetoshow.Image = O2 as Image;
                    break;
                case "errore":
                    timerdestreamer.Stop();
                    progressdestreamer.Visible = false;
                    object O3 = Resources.ResourceManager.GetObject("error");
                    picturetoshow.Image = O3 as Image;

                    if (firsterror)
                    {
                        labeltitle.Text = "Destreamer ha riscontrato un errore.";
                        labelsubtext.Text = "C'è stato un problema... e per questo è stato necessario chiudere Destreamer. Riprova più tardi!";
                        paneldone.Visible = true;
                    }
                    else
                    {
                        timerdestreamer.Stop();
                        timerdestreamer.Interval = 5000;
                        firsterror = true;
                        labeltitle.Text = "Destreamer ha riscontrato un errore.";
                        labelsubtext.Text = "C'è stato un problema... ma non demordere! Tra 5 secondi provo a scaricare il video per la seconda volta...";
                        status = "riprova";
                        timerdestreamer.Start();
                    }
                    
                    break;
                case "riprova":
                    timerdestreamer.Stop();
                    ScaricaVideo();
                    break;
                case "fine":
                    timerdestreamer.Stop();
                    progressdestreamer.Visible = false;
                    labeltitle.Text = "Download completato!";
                    labelsubtext.Text = "Tutto fatto! Troverai il tuo noiosissimo video nella cartella " + Codici.LeggiOpzione("", "path") + ".";
                    object O4 = Resources.ResourceManager.GetObject("folder");
                    picturetoshow.Image = O4 as Image;
                    picturetoshow.Cursor = Cursors.Hand;
                    paneldone.Visible = true;
                    break;
            }

            curstatus = status;
        }

        private void Finito()
        {
            //Quando finisce
            if (curstatus == "riprova") return;
            status = "fine";
        }

        private void buttondone_Click(object sender, EventArgs e)
        {
            labelspeed.Visible = false;
            paneldone.Visible = false;
            picturetoshow.Cursor = Cursors.Default;
            firsterror = false;
            Anima(5);
        }

        private void picturetoshow_Click(object sender, EventArgs e)
        {
            if (labeltitle.Text.Contains("completato")) Process.Start(Codici.LeggiOpzione("", "path"));
        }
        #endregion
    }


}