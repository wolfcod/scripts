namespace miosync
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.fileListBox = new System.Windows.Forms.CheckedListBox();
            this.trackContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.downloadTrackItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeTrackItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonDownload = new System.Windows.Forms.Button();
            this.deviceID = new System.Windows.Forms.Label();
            this.timerDevice = new System.Windows.Forms.Timer(this.components);
            this.folderBrowser = new System.Windows.Forms.FolderBrowserDialog();
            this.button1 = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.garminRadio = new System.Windows.Forms.RadioButton();
            this.sportsRadio = new System.Windows.Forms.RadioButton();
            this.stravaRadio = new System.Windows.Forms.RadioButton();
            this.tcForumRadio = new System.Windows.Forms.RadioButton();
            this.buttonRemoveZero = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.trackContextMenu.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // fileListBox
            // 
            this.fileListBox.ContextMenuStrip = this.trackContextMenu;
            this.fileListBox.FormattingEnabled = true;
            this.fileListBox.Location = new System.Drawing.Point(3, 29);
            this.fileListBox.Name = "fileListBox";
            this.fileListBox.Size = new System.Drawing.Size(322, 199);
            this.fileListBox.TabIndex = 0;
            // 
            // trackContextMenu
            // 
            this.trackContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.downloadTrackItem,
            this.removeTrackItem});
            this.trackContextMenu.Name = "trackContextMenu";
            this.trackContextMenu.Size = new System.Drawing.Size(122, 48);
            // 
            // downloadTrackItem
            // 
            this.downloadTrackItem.Name = "downloadTrackItem";
            this.downloadTrackItem.Size = new System.Drawing.Size(121, 22);
            this.downloadTrackItem.Text = "&Download";
            this.downloadTrackItem.Click += new System.EventHandler(this.downloadTrackItem_Click);
            // 
            // removeTrackItem
            // 
            this.removeTrackItem.Name = "removeTrackItem";
            this.removeTrackItem.Size = new System.Drawing.Size(121, 22);
            this.removeTrackItem.Text = "&Elimina";
            this.removeTrackItem.Click += new System.EventHandler(this.removeTrackItem_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(142, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Tracciati presenti sul device:";
            // 
            // buttonDownload
            // 
            this.buttonDownload.Location = new System.Drawing.Point(331, 56);
            this.buttonDownload.Name = "buttonDownload";
            this.buttonDownload.Size = new System.Drawing.Size(75, 23);
            this.buttonDownload.TabIndex = 2;
            this.buttonDownload.Text = "Download";
            this.buttonDownload.UseVisualStyleBackColor = true;
            this.buttonDownload.Click += new System.EventHandler(this.button1_Click);
            // 
            // deviceID
            // 
            this.deviceID.AutoSize = true;
            this.deviceID.Location = new System.Drawing.Point(290, 7);
            this.deviceID.Name = "deviceID";
            this.deviceID.Size = new System.Drawing.Size(35, 13);
            this.deviceID.TabIndex = 3;
            this.deviceID.Text = "label2";
            // 
            // timerDevice
            // 
            this.timerDevice.Interval = 5000;
            this.timerDevice.Tick += new System.EventHandler(this.timerDevice_Tick);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(331, 29);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Cartella";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(331, 238);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 23);
            this.buttonClose.TabIndex = 5;
            this.buttonClose.Text = "Chiudi";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(331, 85);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 6;
            this.button2.Text = "Converti";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "GPX files|*.gpx|All files|*.*";
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.Filter = "GPX files|*.gpx|All files|*.*";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(3, 234);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(65, 17);
            this.checkBox1.TabIndex = 7;
            this.checkBox1.Text = "Converti";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.garminRadio);
            this.groupBox1.Controls.Add(this.sportsRadio);
            this.groupBox1.Controls.Add(this.stravaRadio);
            this.groupBox1.Controls.Add(this.tcForumRadio);
            this.groupBox1.Location = new System.Drawing.Point(7, 257);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(318, 100);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Formato Conversione";
            // 
            // garminRadio
            // 
            this.garminRadio.AutoSize = true;
            this.garminRadio.Location = new System.Drawing.Point(152, 19);
            this.garminRadio.Name = "garminRadio";
            this.garminRadio.Size = new System.Drawing.Size(82, 17);
            this.garminRadio.TabIndex = 3;
            this.garminRadio.Text = "Garmin TCX";
            this.garminRadio.UseVisualStyleBackColor = true;
            // 
            // sportsRadio
            // 
            this.sportsRadio.AutoSize = true;
            this.sportsRadio.Location = new System.Drawing.Point(9, 65);
            this.sportsRadio.Name = "sportsRadio";
            this.sportsRadio.Size = new System.Drawing.Size(123, 17);
            this.sportsRadio.TabIndex = 2;
            this.sportsRadio.Text = "SPORTS-TRACKER";
            this.sportsRadio.UseVisualStyleBackColor = true;
            // 
            // stravaRadio
            // 
            this.stravaRadio.AutoSize = true;
            this.stravaRadio.Location = new System.Drawing.Point(9, 42);
            this.stravaRadio.Name = "stravaRadio";
            this.stravaRadio.Size = new System.Drawing.Size(68, 17);
            this.stravaRadio.TabIndex = 1;
            this.stravaRadio.Text = "STRAVA";
            this.stravaRadio.UseVisualStyleBackColor = true;
            // 
            // tcForumRadio
            // 
            this.tcForumRadio.AutoSize = true;
            this.tcForumRadio.Checked = true;
            this.tcForumRadio.Location = new System.Drawing.Point(9, 19);
            this.tcForumRadio.Name = "tcForumRadio";
            this.tcForumRadio.Size = new System.Drawing.Size(98, 17);
            this.tcForumRadio.TabIndex = 0;
            this.tcForumRadio.TabStop = true;
            this.tcForumRadio.Text = "BDC/TC Forum";
            this.tcForumRadio.UseVisualStyleBackColor = true;
            // 
            // buttonRemoveZero
            // 
            this.buttonRemoveZero.Location = new System.Drawing.Point(331, 114);
            this.buttonRemoveZero.Name = "buttonRemoveZero";
            this.buttonRemoveZero.Size = new System.Drawing.Size(75, 23);
            this.buttonRemoveZero.TabIndex = 9;
            this.buttonRemoveZero.Text = "[ZERO]";
            this.buttonRemoveZero.UseVisualStyleBackColor = true;
            this.buttonRemoveZero.Click += new System.EventHandler(this.buttonRemoveZero_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(331, 143);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 10;
            this.button3.Text = "Vecchia Conv.";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Visible = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(410, 363);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.buttonRemoveZero);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.buttonDownload);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.deviceID);
            this.Controls.Add(this.fileListBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "MIO Cyclo 100/105HC sync tool";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.trackContextMenu.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckedListBox fileListBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonDownload;
        private System.Windows.Forms.Label deviceID;
        private System.Windows.Forms.Timer timerDevice;
        private System.Windows.Forms.FolderBrowserDialog folderBrowser;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton tcForumRadio;
        private System.Windows.Forms.RadioButton garminRadio;
        private System.Windows.Forms.RadioButton sportsRadio;
        private System.Windows.Forms.RadioButton stravaRadio;
        private System.Windows.Forms.ContextMenuStrip trackContextMenu;
        private System.Windows.Forms.ToolStripMenuItem downloadTrackItem;
        private System.Windows.Forms.ToolStripMenuItem removeTrackItem;
        private System.Windows.Forms.Button buttonRemoveZero;
        private System.Windows.Forms.Button button3;
    }
}

