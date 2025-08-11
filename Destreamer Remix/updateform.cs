using Destreamer_Remix.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Destreamer_Remix
{
    public partial class updateform : Form
    {
        public updateform(string versione)
        {
            InitializeComponent();
            labelversion.Text = "Nuova versione: " + versione.Insert(1, ".");

            //nasconde i form
            for (int i = Application.OpenForms.Count - 1; i >= 0; i--)
            {
                if (Application.OpenForms[i].Name != "updateform")
                    Application.OpenForms[i].Hide();
            }

            Aggiorna();
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

        private async Task Aggiorna()
        {
            string linky = "";

            await Task.Run(() => {
                try
                {
                    WebClient client = new WebClient();
                    linky = client.DownloadString(new Uri("https://onedrive.live.com/download?cid=3781DC0B8F8FC809&resid=3781DC0B8F8FC809%2139582&authkey=ANm4DouCOwNxLRU"));
                }
                catch { }
            });

            if (linky == "") linky = "https://onedrive.live.com/download?cid=3781DC0B8F8FC809&resid=3781DC0B8F8FC809%2139602&authkey=AGZHkaOxRExPpss";

            //Scarica l'eseguibile
            scaricamento = await Codici.Downloader(linky, Application.StartupPath + @"\DestreamerRemixupdate", null, null);
            
            if (scaricamento)
            {
                await Task.Run(() =>
                {
                    try
                    {
                        File.WriteAllText(Application.StartupPath + @"\updatedes.bat", @"taskkill /F /IM """ + Path.GetFileName(Application.ExecutablePath) + @""" & if exist DestreamerRemixupdate del """ + Path.GetFileName(Application.ExecutablePath) + @""" & rename DestreamerRemixupdate """ + Path.GetFileName(Application.ExecutablePath) + @""" & start """" """ + Path.GetFileName(Application.ExecutablePath) + @"""", System.Text.Encoding.Default);
                    }
                    catch { scaricamento = false; }
                });

                if (File.Exists(Application.StartupPath + @"\updatedes.bat") == false) scaricamento = false;

                labelversion.Text = "Riavvio in corso...";
                labeltitle.Text = "Aggiornamento riuscito.";
                labeltext.Text = "Lo scaricamento degli aggiornamenti di Destreamer Remix è andato a buon fine, l'applicazione si riavvierà tra meno di 5 secondi.";
            }
            
            if (scaricamento == false)
            {
                labelversion.Text = "Chiusura in corso...";
                labeltitle.Text = "Aggiornamento fallito.";
                labeltext.Text = "Si sono verificate delle problematiche durante l'applicazione degli aggiornamenti di Destreamer Remix, l'applicazione si chiuderà tra meno di 5 secondi.";
                object O = Resources.ResourceManager.GetObject("close");
                pictureBox1.Image = O as Image;
            }

            timer1.Start();
        }

        public static bool scaricamento = false;
        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();

            if (scaricamento == true)
            {
                try
                {
                    Process p = new Process();
                    p.StartInfo.CreateNoWindow = true;
                    p.StartInfo.FileName = ("updatedes.bat");
                    p.StartInfo.WorkingDirectory = Application.StartupPath + @"\";
                    p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    p.Start();  
                }
                catch
                {
                    Process p = new Process();
                    p.StartInfo.CreateNoWindow = true;
                    p.StartInfo.FileName = ("updatedes.bat");
                    p.StartInfo.WorkingDirectory = Application.StartupPath;
                    p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    p.Start();
                }
            }

            //chiude tutti i form
            for (int i = Application.OpenForms.Count - 1; i >= 0; i--)
            {
                    Application.OpenForms[i].Close();
            }
        }

        private void updateform_Load(object sender, EventArgs e)
        {

        }
    }
}
