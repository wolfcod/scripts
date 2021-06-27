using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace miosync
{
    public partial class MainForm : Form
    {
        private ieMioPlugInLib.IEinterfaceClass _DeviceControl = null;
        private string folderOutput = string.Empty;

        public MainForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (string selected in fileListBox.SelectedItems)
            {
                string devicePath = string.Format("{0}{1}", _DeviceControl.CYC100_GetSystemPath(), selected);
                string outputFile = string.Format("{0}\\{1}", this.folderOutput, selected);

                AdapterType outputType = AdapterType.NONE;

                if (this.checkBox1.Checked == true && this.garminRadio.Checked)
                {
                    int i = outputFile.LastIndexOf(".gpx");
                    outputType = AdapterType.GARMIN;
                    if (i >= 0)
                    {
                        outputFile = outputFile.Substring(0, i) + ".tcx";
                    }
                }

                try
                {
                    _DeviceControl.CYC100_saveFile(selected, null);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Errore DEVICE_CONTROL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                while (System.IO.File.Exists(devicePath) == false)
                {   // put log in output
                    System.Threading.Thread.Sleep(500);
                }

                if (this.checkBox1.Checked)
                {   // 

                    if (this.tcForumRadio.Checked)
                        outputType = AdapterType.MTBFORUM;

                    if (this.stravaRadio.Checked)
                        outputType = AdapterType.STRAVA;

                    if (this.sportsRadio.Checked)
                        outputType = AdapterType.SPORTSTRACKER;
                }

                try
                {
                    adapter.Convert(devicePath, outputFile, outputType);
                    System.IO.File.Delete(devicePath);
                    MessageBox.Show(string.Format("{0} trasferito con successo", selected), "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch(Exception exception)
                {   // ignorare questo errore
                    MessageBox.Show(exception.Message, "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                    
                
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.Text += string.Format(" {0}", Application.ProductVersion);

            config c = config.getConfiguration();

            folderOutput = c.DownloadFolder;
            folderBrowser.SelectedPath = folderOutput;

            switch (c.Type)
            {
                case AdapterType.NONE:
                    this.checkBox1.Checked = false;
                    break;
                case AdapterType.GARMIN:
                    this.checkBox1.Checked = true;
                    this.garminRadio.Checked = true;
                    break;
                case AdapterType.MTBFORUM:
                    this.checkBox1.Checked = true;
                    this.tcForumRadio.Checked = true;
                    break;
                case AdapterType.STRAVA:
                    this.checkBox1.Checked = true;
                    this.stravaRadio.Checked = true;
                    break;
                case AdapterType.SPORTSTRACKER:
                    this.checkBox1.Checked = true;
                    this.sportsRadio.Checked = true;
                    break;
            }

            this.checkBox1.Checked = c.Convert;

            _DeviceControl = new ieMioPlugInLib.IEinterfaceClass();

            _DeviceControl.CYC100_SetDeviceConnect(this);

            string devId = _DeviceControl.CYC100_GetDeviceID();

            folderOutput = folderBrowser.RootFolder.ToString();

            deviceID.Text = devId;
            if (devId.CompareTo("No Device!!") == 0)
            {
                buttonDownload.Enabled = false;
                timerDevice.Enabled = true;
            }
            else
                refreshCombo(sender);

        }

        private void refreshCombo(object sender)
        {
            fileListBox.Items.Clear();

            string [] fileList = _DeviceControl.CYC100_LoadFileList(0, "").Split('|');

            foreach (string file in fileList)
                fileListBox.Items.Add(file);
        }

        private void timerDevice_Tick(object sender, EventArgs e)
        {
            string devId = _DeviceControl.CYC100_GetDeviceID();

            if (devId.CompareTo("No Device!!") != 0)
            {
                timerDevice.Enabled = false;
                buttonDownload.Enabled = true;
                deviceID.Text = devId;
                refreshCombo(sender);
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (folderBrowser.ShowDialog() == DialogResult.OK)
            {
                this.folderOutput = folderBrowser.SelectedPath;
            }
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {   // disable device connect
            _DeviceControl.CYC100_SetDeviceConnect(null);

            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string saveFormat = "GPX Files|*.gpx|All Files|*.*";
            AdapterType outputType = AdapterType.NONE;

            if (garminRadio.Checked)
            {
                saveFormat = "TCX Files|*.tcx|All Files|*.*";
                saveFileDialog.Filter = saveFormat;
                outputType = AdapterType.GARMIN;
            }
            else
            {
                if (this.tcForumRadio.Checked)
                {
                    outputType = AdapterType.MTBFORUM;
                }
                if (this.stravaRadio.Checked)
                    outputType = AdapterType.STRAVA;

                if (this.sportsRadio.Checked)
                    outputType = AdapterType.SPORTSTRACKER;

                saveFileDialog.Filter = saveFormat;
            }

            if (openFileDialog.ShowDialog() == DialogResult.OK &&
                saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try 
                {
                    adapter.Convert(openFileDialog.FileName, saveFileDialog.FileName, outputType);
                    MessageBox.Show("Conversione completata", "Esito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message, "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void downloadTrackItem_Click(object sender, EventArgs e)
        {
            if (this.fileListBox.SelectedItem != null)
            {
                this.buttonDownload.Select();
            }

        }

        private void removeTrackItem_Click(object sender, EventArgs e)
        {
            if (this.fileListBox.SelectedItem != null)
            {
                string fileName = this.fileListBox.SelectedItem.ToString();

                if (MessageBox.Show(string.Format("Vuoi eliminare {0} dal dispositivo?", fileName), "Attenzione", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    _DeviceControl.CYC100_deleteDrvFile(fileName, 0, "");
                    MessageBox.Show("Traccia eliminata dal dispositivo.");
                    refreshCombo(sender);
                }
            }
        }

        private void buttonRemoveZero_Click(object sender, EventArgs e)
        {
            string saveFormat = "GPX Files|*.gpx|All Files|*.*";
            AdapterType outputType = AdapterType.PAUSE_DETECTION;

            saveFileDialog.Filter = saveFormat;

            openFileDialog.Title = "Selezionare il file da cui rimuovere le pause";
            saveFileDialog.Title = "Selezionare il file di destinazione";

            if (openFileDialog.ShowDialog() == DialogResult.OK &&
                saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    adapter.Convert(openFileDialog.FileName, saveFileDialog.FileName, outputType);
                    MessageBox.Show("Conversione completata", "Esito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message, "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string saveFormat = "GPX Files|*.gpx|All Files|*.*";
            AdapterType outputType = AdapterType.NONE;

            if (garminRadio.Checked)
            {
                saveFormat = "TCX Files|*.tcx|All Files|*.*";
                saveFileDialog.Filter = saveFormat;
                outputType = AdapterType.GARMIN;
            }
            else
            {
                if (this.tcForumRadio.Checked)
                {
                    outputType = AdapterType.MTBFORUM;
                }
                if (this.stravaRadio.Checked)
                    outputType = AdapterType.STRAVA;

                if (this.sportsRadio.Checked)
                    outputType = AdapterType.SPORTSTRACKER;

                saveFileDialog.Filter = saveFormat;
            }

            if (openFileDialog.ShowDialog() == DialogResult.OK &&
                saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    StreamReader r = new StreamReader(openFileDialog.FileName);
                    StreamWriter w = new StreamWriter(saveFileDialog.FileName);
                    adapter.adapt(outputType, r, w);

                    r.Close();
                    w.Close();
                    //adapter.Convert(openFileDialog.FileName, saveFileDialog.FileName, outputType);
                    MessageBox.Show("Conversione completata", "Esito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message, "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            config c = config.getConfiguration();

            c.Convert = this.checkBox1.Checked;
            c.DownloadFolder = this.folderOutput;

            if (this.tcForumRadio.Checked)
                c.Type = AdapterType.MTBFORUM;
            else if (this.sportsRadio.Checked)
                c.Type = AdapterType.SPORTSTRACKER;
            else if (this.stravaRadio.Checked)
                c.Type = AdapterType.STRAVA;
            else if (this.garminRadio.Checked)
                c.Type = AdapterType.GARMIN;
            else
                c.Type = AdapterType.NONE;

            c.Update();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
