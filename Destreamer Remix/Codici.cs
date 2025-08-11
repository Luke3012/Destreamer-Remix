using System;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Net;
using System.Windows.Forms;
using System.ComponentModel;

namespace Destreamer_Remix
{
    class Codici
    {
        public static string versione = "02";
        public static string percorso;
        public static string link_destreamer = "https://onedrive.live.com/download?cid=3781DC0B8F8FC809&resid=3781DC0B8F8FC809%2139539&authkey=AIhVFav0BbngDwg";

        public static bool updatechecking = false;
        public static async Task<bool> ControlloAggiornamenti()
        {
            if (updatechecking) return false;
            updatechecking = true;
            string nv = "0";

            await Task.Run(() => {
                try
                {
                    WebClient client = new WebClient();
                    nv = client.DownloadString(new Uri("https://onedrive.live.com/download?cid=3781DC0B8F8FC809&resid=3781DC0B8F8FC809%2139581&authkey=AKCtfXVhG55o3bI"));
                }
                catch { }
            });

            updatechecking = false;

            if (int.Parse(nv) > int.Parse(versione))
            {
                //Nuovo aggiornamento disponibile!
                updateform uppy = new updateform(nv);
                uppy.Show();
                return true;
            }

            return false;
        }

        public static string LeggiOpzione(string settingfile, string chiave)
        {
            if (settingfile == "") settingfile = percorso + @"\impostazioni.ini";

            try
            {
                if (File.Exists(settingfile))
                {
                    List<string> sw = new List<string>(File.ReadAllLines(settingfile));

                    for (int i = 0; i < sw.Count; i++)
                    {
                        string lettura = sw[i];
                        if (lettura.Contains(chiave + "="))
                        {
                            return lettura.Replace(chiave + "=", "");
                        }
                    }
                }

            }
            catch { }


            return "";
        }

        public static void SalvaOpzione(string settingfile, string chiave, string valore, Boolean darimuovere)
        {
            if (settingfile == "")
            {
                settingfile = percorso + @"\impostazioni.ini";
                if (Directory.Exists(percorso) == false) Directory.CreateDirectory(percorso);
            }
            else
            {
                List<string> a = new List<string>(settingfile.Split('\\').ToList());

                if (a.Count - 1 > 0)
                {
                    string eccola = "";

                    for (int i = 0; i < a.Count - 1; i++)
                    {
                        eccola = eccola + a[i] + @"\";
                    }

                    if (Directory.Exists(eccola) == false)
                    {
                        try
                        {
                            Directory.CreateDirectory(eccola);
                        }
                        catch { }
                    }
                }
            }

            List<string> ueue = new List<string>();

            if (File.Exists(settingfile))
            {
                ueue = new List<string>(File.ReadAllLines(settingfile));

                for (int i = 0; i < ueue.Count; i++)
                {
                    string lettura = ueue[i];
                    if (lettura.Contains(chiave + "="))
                    {
                        ueue.RemoveAt(i);
                        if (darimuovere == true) break;
                        ueue.Insert(i, chiave + "=" + valore);
                        File.WriteAllLines(settingfile, ueue);
                        return;
                    }

                }
            }

            if (darimuovere == false) ueue.Add(chiave + "=" + valore);
            File.WriteAllLines(settingfile, ueue);
        }


        //parte download
        public static bool scaricato = false;
        public static ProgressBar progresss = null;
        public static Label labelll = null;
        public static async Task<bool> Downloader(string url, string salva, ProgressBar progress, Label label)
        {
            WebClient webClient = new WebClient();
            progresss = progress;
            labelll = label;

            await Task.Run(() =>
            {
                webClient.OpenRead(url);
            });
            
            Int64 bytes_total = Convert.ToInt64(webClient.ResponseHeaders["Content-Length"]);
            webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
            if (progress != null || label != null) webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
            webClient.DownloadFileAsync(new Uri(url), salva);

            while (!scaricato) await Task.Delay(500);

            if (File.Exists(salva))
            {
                long length = new System.IO.FileInfo(salva).Length;
                if (length != bytes_total || length == 0)
                {
                    File.Delete(salva);
                    return false;
                }
            }
            else return false;

            return true;
        }
        public static void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            double bytesIn = double.Parse(e.BytesReceived.ToString());
            double totalBytes = double.Parse(e.TotalBytesToReceive.ToString());
            double percentage = bytesIn / totalBytes * 100;
            int gg = int.Parse(Math.Truncate(percentage).ToString());
            if (labelll != null) labelll.Text = "Percentuale completamento: " + gg + "%";
            if (progresss != null) progresss.Value = gg;
        }

        public static void Completed(object sender, AsyncCompletedEventArgs e)
        {
            scaricato = true;
        }

        //parte estrazione
        public static async Task Extract(string file, string dove)
        {
            await Task.Run(() => {

                ZipFile.ExtractToDirectory(file, dove);
                File.Delete(file);
                });

        }

        public static async Task Elimina(string darimuovere, bool secartella)
        {
            await Task.Run(() => {
                try {
                    if (secartella) Directory.Delete(darimuovere, true);
                    else File.Delete(darimuovere);
                } catch { }
            });
        }
    }
}
