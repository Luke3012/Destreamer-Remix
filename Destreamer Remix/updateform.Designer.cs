namespace Destreamer_Remix
{
    partial class updateform
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(updateform));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.labeltitle = new System.Windows.Forms.Label();
            this.labeltext = new System.Windows.Forms.Label();
            this.labelversion = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Destreamer_Remix.Properties.Resources.update_checking;
            this.pictureBox1.Location = new System.Drawing.Point(180, 305);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(430, 129);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel_MouseDown);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panel_MouseMove);
            this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panel_MouseUp);
            // 
            // labeltitle
            // 
            this.labeltitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labeltitle.Location = new System.Drawing.Point(24, 72);
            this.labeltitle.Name = "labeltitle";
            this.labeltitle.Size = new System.Drawing.Size(743, 71);
            this.labeltitle.TabIndex = 21;
            this.labeltitle.Text = "Download dell\'aggiornamento in corso...";
            this.labeltitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labeltitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel_MouseDown);
            this.labeltitle.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panel_MouseMove);
            this.labeltitle.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panel_MouseUp);
            // 
            // labeltext
            // 
            this.labeltext.Font = new System.Drawing.Font("Microsoft YaHei UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labeltext.Location = new System.Drawing.Point(26, 143);
            this.labeltext.Name = "labeltext";
            this.labeltext.Size = new System.Drawing.Size(741, 188);
            this.labeltext.TabIndex = 22;
            this.labeltext.Text = "Sto scaricando l\'aggiornamento di Destreamer Remix alla versione più recente.\r\n\r\n" +
    "E\' questione di secondi...";
            this.labeltext.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.labeltext.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel_MouseDown);
            this.labeltext.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panel_MouseMove);
            this.labeltext.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panel_MouseUp);
            // 
            // labelversion
            // 
            this.labelversion.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelversion.Location = new System.Drawing.Point(180, 437);
            this.labelversion.Name = "labelversion";
            this.labelversion.Size = new System.Drawing.Size(430, 30);
            this.labelversion.TabIndex = 23;
            this.labelversion.Text = "Nuova versione: 0.0";
            this.labelversion.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.labelversion.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel_MouseDown);
            this.labelversion.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panel_MouseMove);
            this.labelversion.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panel_MouseUp);
            // 
            // timer1
            // 
            this.timer1.Interval = 5000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // updateform
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(791, 502);
            this.ControlBox = false;
            this.Controls.Add(this.labelversion);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.labeltext);
            this.Controls.Add(this.labeltitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "updateform";
            this.Opacity = 0.95D;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "updateform";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.updateform_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panel_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panel_MouseUp);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label labeltitle;
        private System.Windows.Forms.Label labeltext;
        private System.Windows.Forms.Label labelversion;
        private System.Windows.Forms.Timer timer1;
    }
}