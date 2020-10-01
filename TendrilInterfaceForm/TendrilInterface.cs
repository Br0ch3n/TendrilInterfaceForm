using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tulpep.NotificationWindow;

namespace TendrilInterfaceForm
{
    public partial class TendrilInterface : Form
    {

        public float[] encValues;

        private CSVReader simFile, configFile;
        private CSVWriter outputFile;
        private String rxString;
        private String[] cntsFeedback;
        private String[] tensFeedback;
        private String[] cntsSim;
        private int runAvgSize;
        private TextBox[] MnlTBArray;
        private RunningAverage[] tenAvg;
        private String cntsCurrent;
        private int[] cntsOffsets;
        private bool simRunning;
        private String evenOut;
        private int ERR_THRESH;
        private bool flgProcessing, flgWriting, flgWriteAll, flgTimer;
        private OpenGLSim glSimDisplay;
        private KalmanFilter[] KFTensions;
        private System.Timers.Timer time;
        private Color prevColor;
        private TendrilStateSingleton TendrilState;

        public TendrilInterface()
        {
            InitializeComponent();
            evenOut = "";
            pnlMnlControl.Enabled = false;
            pnlMnlControl.Visible = false;
            cntsOffsets = new int[] {0,0,0,0,0,0,0,0,0};
            runAvgSize = 5;
            tensFeedback = new String[9];
            cntsFeedback = new String[9];
            tenAvg = new RunningAverage[9];
            for (int i = 0; i < 9; i++) tenAvg[i] = new RunningAverage(runAvgSize);
            Width = Width - pnlMnlControl.Width;
            pnlCSVWriter.Enabled = false;
            pnlCSVWriter.Visible = false;
            Height = Height - pnlCSVWriter.Height;
            simRunning = false;
            flgProcessing = false;
            flgWriting = false;
            flgWriteAll = false;
            ERR_THRESH = 20;
            MnlTBArray = new TextBox[] { tbMnlMot0, tbMnlMot1, tbMnlMot2, tbMnlMot3, tbMnlMot4, tbMnlMot5, tbMnlMot6, tbMnlMot7, tbMnlMot8 };
            KFTensions = new KalmanFilter[9];
            encValues = new float[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            time = new System.Timers.Timer();
            time.Interval = 10000;
            time.Elapsed += OnTimeElapsed;
            flgTimer = false;
            TendrilState = TendrilStateSingleton.getInstance();

            for (int ndx = 0; ndx < 9; ndx++)
            {
                KFTensions[ndx] = new KalmanFilter(.009f,1.0f);
            }
            
        }

        
        //////////////////////////////////////////////////////////////////////
        ////////////////////        Simulation Code         //////////////////
        //////////////////////////////////////////////////////////////////////

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Displays an OpenFileDialog so the user can select a Cursor.  
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "CSV files (*.csv)|*.csv|txt files (*.txt)|*.txt";
            openFileDialog1.Title = "Select a Text or CSV File";

            // Show the Dialog.  
            // If the user clicked OK in the dialog and  
            // a .CUR file was selected, open it.  
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                simTxtLabel.Text = "Simulation: " + openFileDialog1.FileName.Split('\\').Last();
                if (simFile != null) simFile.CloseCSV();
                simFile = new CSVReader(openFileDialog1.OpenFile());
                simCurrTxLabel.Text = "File opened.";
                btnNext.Enabled = true;
                btnRun.Enabled = true;
                btnSend.Enabled = true; 
                btnReset.Enabled = true;
            }
        }
        
        private void btnNext_MouseClick(object sender, MouseEventArgs e)
        {
            simCurrTxLabel.Text = simFile.CSV_ReadNextLine();
            if (simCurrTxLabel.Text == "End of file.")
            {
                btnNext.Enabled = false;
                return;
            } else if (!btnPrev.Enabled)
            {
                btnPrev.Enabled = true;
            }
        }

        private void btnSend_MouseClick(object sender, MouseEventArgs e)
        {
            String[] temp = simCurrTxLabel.Text.Split(',');
            if (temp.Length == 9) btSerialPort.WriteLine("Read," + simCurrTxLabel.Text);
        }

        private void btnRun_MouseClick(object sender, MouseEventArgs e)
        {

            simRunning = true;
            btnNext.Enabled = false;
            btnPrev.Enabled = false;
            btnPause.Enabled = true;
            btnRun.Enabled = false;
            btnSend.Enabled = false;
            btnReset.Enabled = false;

            tensFeedback = tenStatusLabel.Text.Split(',');
            cntsFeedback = cntStatusLabel.Text.Split(',');
            cntsSim = simCurrTxLabel.Text.Split(',');

            if (simCurrTxLabel.Text == "File opened.")
            {
                simCurrTxLabel.Text = simFile.CSV_ReadNextLine();
                cntsSim = simCurrTxLabel.Text.Split(',');
                btSerialPort.WriteLine("Read," + simCurrTxLabel.Text);
            }

        }

        
        private void btnPause_Click(object sender, EventArgs e)
        {
            simRunning = false;
            btnNext.Enabled = true;
            btnPrev.Enabled = true;
            btnPause.Enabled = false;
            btnRun.Enabled = true;
            btnSend.Enabled = true;
            btnReset.Enabled = true;
        }
        private void btnPrev_MouseClick(object sender, MouseEventArgs e)
        {
            simCurrTxLabel.Text = simFile.CSV_ReadPrevLine();
            if (simCurrTxLabel.Text == "File opened.")
            {
                btnPrev.Enabled = false;
                return;
            }
            if (!btnNext.Enabled) btnNext.Enabled = true;
            
        }

        private void btnReset_MouseClick(object sender, MouseEventArgs e)
        {
            simCurrTxLabel.Text = simFile.ResetPosition();
        }

        //////////////////////////////////////////////////////////////////////
        ////////////////////          Write to CSV          //////////////////
        //////////////////////////////////////////////////////////////////////
        private void saveOutputToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Displays an OpenFileDialog so the user can select a Cursor.  
            Height = Height + pnlCSVWriter.Height;
            pnlCSVWriter.Enabled = true;
            pnlCSVWriter.Visible = true;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "CSV files (*.csv)|*.csv";
            saveFileDialog1.Title = "Select a CSV File Save Location";
            // Show the Dialog.  
            // If the user clicked OK in the dialog and  
            // a .CUR file was selected, open it.  
            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                lblCSVWriter.Text = "Output File: " + saveFileDialog1.FileName.Split('\\').Last();
                if (outputFile != null) outputFile.CloseCSV();
                outputFile = new CSVWriter(saveFileDialog1.OpenFile());
                //lblCSVWriter.Text = "File opened, Begin?...";
                String s = "Timestamp,E0,E1,E2,E3,E4,E5,E6,E7,E8,T0,T1,T2,T3,T4,T5,T6,T7,T8";
                outputFile.CSV_WriteLine(s);
            }
        }

        private void cbCSVWriterSendAll_CheckedChanged(object sender, EventArgs e)
        {
            if (cbCSVWriterSendAll.Checked) flgWriteAll = true;
            else flgWriteAll = false;
        }

        private void btnCSVWriterSend_Click(object sender, EventArgs e)
        {
            if (outputFile != null)
            {
                outputFile.CSV_WriteLine(outputFile.CSV_PrepareLog(TendrilState.GetEncoders(), TendrilState.GetTensions()));
            }
        }

        private void btnCSVWriterBegin_Click(object sender, EventArgs e)
        {
            if (outputFile != null && outputFile.strmActive)
            {
                flgWriting = true;
                if (cbCSVWriterSendAll.Checked)
                {
                    flgWriteAll = true;
                } else
                {
                    flgWriteAll = false;
                }
                btnCSVWriterBegin.Enabled = false;
                btnCSVWriterEnd.Enabled = true;
                //String s = TendrilUtils.GetTimesstamp(DateTime.Now) + ",E0,E1,E2,E3,E4,E5,E6,E7,E8,T0,T1,T2,T3,T4,T5,T6,T7,T8";
                //outputFile.CSV_WriteLine(s);
            }
        }

        private void btnCSVWriterEnd_Click(object sender, EventArgs e)
        {
            outputFile.CloseCSV();
            flgWriting = false;
            cbCSVWriterSendAll.Checked = false;
            flgWriteAll = false;
            btnCSVWriterBegin.Enabled = true;
            btnCSVWriterEnd.Enabled = false;
        }

        

        //////////////////////////////////////////////////////////////////////
        ////////////////////       Manual Control Code      //////////////////
        //////////////////////////////////////////////////////////////////////

        private void manualControlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!pnlMnlControl.Enabled)
            {
                prevColor = manualControlToolStripMenuItem.BackColor;
                manualControlToolStripMenuItem.BackColor = Color.Moccasin;
                Width = Width + pnlMnlControl.Width;
                pnlMnlControl.Enabled = true;
                pnlMnlControl.Visible = true;
            } else
            {
                manualControlToolStripMenuItem.BackColor = prevColor;
                manualControlToolStripMenuItem.Checked = false;
                Width = Width - pnlMnlControl.Width;
                pnlMnlControl.Enabled = false;
                pnlMnlControl.Visible = false;
            }
                
        }

        private void btnMnlMot0_MM_Click(object sender, EventArgs e)
        {
            MnlTBArray[0].Text = TendrilUtils.SubtractText((MnlTBArray[0].Text == "" ? "0" : MnlTBArray[0].Text), TendrilState.GetLrgIncrement().ToString());
            btnMnlSend.PerformClick();
        }

        private void btnMnlMot0_M_Click(object sender, EventArgs e)
        {
            MnlTBArray[0].Text = TendrilUtils.SubtractText((MnlTBArray[0].Text == "" ? "0" : MnlTBArray[0].Text), TendrilState.GetSmlIncrement().ToString());
            btnMnlSend.PerformClick();
        }

        private void btnMnlMot0_P_Click(object sender, EventArgs e)
        {
            MnlTBArray[0].Text = TendrilUtils.AddText((MnlTBArray[0].Text == "" ? "0" : MnlTBArray[0].Text), TendrilState.GetSmlIncrement().ToString());
            btnMnlSend.PerformClick();
        }

        private void btnMnlMot0_PP_Click(object sender, EventArgs e)
        {
            MnlTBArray[0].Text = TendrilUtils.AddText((MnlTBArray[0].Text == "" ? "0" : MnlTBArray[0].Text), TendrilState.GetLrgIncrement().ToString());
            btnMnlSend.PerformClick();
        }

        private void btnMnlMot1_MM_Click(object sender, EventArgs e)
        {
            MnlTBArray[1].Text = TendrilUtils.SubtractText((MnlTBArray[1].Text == "" ? "0" : MnlTBArray[1].Text), TendrilState.GetLrgIncrement().ToString());
            btnMnlSend.PerformClick();
        }

        private void btnMnlMot1_M_Click(object sender, EventArgs e)
        {
            MnlTBArray[1].Text = TendrilUtils.SubtractText((MnlTBArray[1].Text == "" ? "0" : MnlTBArray[1].Text), TendrilState.GetSmlIncrement().ToString());
            btnMnlSend.PerformClick();
        }

        private void btnMnlMot1_P_Click(object sender, EventArgs e)
        {
            MnlTBArray[1].Text = TendrilUtils.AddText((MnlTBArray[1].Text == "" ? "0" : MnlTBArray[1].Text), TendrilState.GetSmlIncrement().ToString());
            btnMnlSend.PerformClick();
        }

        private void btnMnlMot1_PP_Click(object sender, EventArgs e)
        {
            MnlTBArray[1].Text = TendrilUtils.AddText((MnlTBArray[1].Text == "" ? "0" : MnlTBArray[1].Text), TendrilState.GetLrgIncrement().ToString());
            btnMnlSend.PerformClick();
        }

        private void btnMnlMot2_MM_Click(object sender, EventArgs e)
        {
            MnlTBArray[2].Text = TendrilUtils.SubtractText((MnlTBArray[2].Text == "" ? "0" : MnlTBArray[2].Text), TendrilState.GetLrgIncrement().ToString());
            btnMnlSend.PerformClick();

        }

        private void btnMnlMot2_M_Click(object sender, EventArgs e)
        {
            MnlTBArray[2].Text = TendrilUtils.SubtractText((MnlTBArray[2].Text == "" ? "0" : MnlTBArray[2].Text), TendrilState.GetSmlIncrement().ToString());
            btnMnlSend.PerformClick();
        }

        private void btnMnlMot2_P_Click(object sender, EventArgs e)
        {
            MnlTBArray[2].Text = TendrilUtils.AddText((MnlTBArray[2].Text == "" ? "0" : MnlTBArray[2].Text), TendrilState.GetSmlIncrement().ToString());
            btnMnlSend.PerformClick();
        }

        private void btnMnlMot2_PP_Click(object sender, EventArgs e)
        {
            MnlTBArray[2].Text = TendrilUtils.AddText((MnlTBArray[2].Text == "" ? "0" : MnlTBArray[2].Text), TendrilState.GetLrgIncrement().ToString());
            btnMnlSend.PerformClick();
        }

        private void btnMnlMot3_MM_Click(object sender, EventArgs e)
        {
            MnlTBArray[3].Text = TendrilUtils.SubtractText((MnlTBArray[3].Text == "" ? "0" : MnlTBArray[3].Text), TendrilState.GetLrgIncrement().ToString());
            btnMnlSend.PerformClick();
        }

        private void btnMnlMot3_M_Click(object sender, EventArgs e)
        {
            MnlTBArray[3].Text = TendrilUtils.SubtractText((MnlTBArray[3].Text == "" ? "0" : MnlTBArray[3].Text), TendrilState.GetSmlIncrement().ToString());
            btnMnlSend.PerformClick();
        }

        private void btnMnlMot3_P_Click(object sender, EventArgs e)
        {
            MnlTBArray[3].Text = TendrilUtils.AddText((MnlTBArray[3].Text == "" ? "0" : MnlTBArray[3].Text), TendrilState.GetSmlIncrement().ToString());
            btnMnlSend.PerformClick();
        }

        private void btnMnlMot3_PP_Click(object sender, EventArgs e)
        {
            MnlTBArray[3].Text = TendrilUtils.AddText((MnlTBArray[3].Text == "" ? "0" : MnlTBArray[3].Text), TendrilState.GetLrgIncrement().ToString());
            btnMnlSend.PerformClick();
        }

        private void btnMnlMot4_MM_Click(object sender, EventArgs e)
        {
            MnlTBArray[4].Text = TendrilUtils.SubtractText((MnlTBArray[4].Text == "" ? "0" : MnlTBArray[4].Text), TendrilState.GetLrgIncrement().ToString());
            btnMnlSend.PerformClick();
        }

        private void btnMnlMot4_M_Click(object sender, EventArgs e)
        {
            MnlTBArray[4].Text = TendrilUtils.SubtractText((MnlTBArray[4].Text == "" ? "0" : MnlTBArray[4].Text), TendrilState.GetSmlIncrement().ToString());
            btnMnlSend.PerformClick();
        }

        private void btnMnlMot4_P_Click(object sender, EventArgs e)
        {
            MnlTBArray[4].Text = TendrilUtils.AddText((MnlTBArray[4].Text == "" ? "0" : MnlTBArray[4].Text), TendrilState.GetSmlIncrement().ToString());
            btnMnlSend.PerformClick();
        }

        private void btnMnlMot4_PP_Click(object sender, EventArgs e)
        {
            MnlTBArray[4].Text = TendrilUtils.AddText((MnlTBArray[4].Text == "" ? "0" : MnlTBArray[4].Text), TendrilState.GetLrgIncrement().ToString());
            btnMnlSend.PerformClick();
        }

        private void btnMnlMot5_MM_Click(object sender, EventArgs e)
        {
            MnlTBArray[5].Text = TendrilUtils.SubtractText((MnlTBArray[5].Text == "" ? "0" : MnlTBArray[5].Text), TendrilState.GetLrgIncrement().ToString());
            btnMnlSend.PerformClick();
        }

        private void btnMnlMot5_M_Click(object sender, EventArgs e)
        {
            MnlTBArray[5].Text = TendrilUtils.SubtractText((MnlTBArray[5].Text == "" ? "0" : MnlTBArray[5].Text), TendrilState.GetSmlIncrement().ToString());
            btnMnlSend.PerformClick();
        }

        private void btnMnlMot5_P_Click(object sender, EventArgs e)
        {
            MnlTBArray[5].Text = TendrilUtils.AddText((MnlTBArray[5].Text == "" ? "0" : MnlTBArray[5].Text), TendrilState.GetSmlIncrement().ToString());
            btnMnlSend.PerformClick();
        }

        private void btnMnlMot5_PP_Click(object sender, EventArgs e)
        {
            MnlTBArray[5].Text = TendrilUtils.AddText((MnlTBArray[5].Text == "" ? "0" : MnlTBArray[5].Text), TendrilState.GetLrgIncrement().ToString());
            btnMnlSend.PerformClick();
        }

        private void btnMnlMot6_MM_Click(object sender, EventArgs e)
        {
            MnlTBArray[6].Text = TendrilUtils.SubtractText((MnlTBArray[6].Text == "" ? "0" : MnlTBArray[6].Text), TendrilState.GetLrgIncrement().ToString());
            btnMnlSend.PerformClick();
        }

        private void btnMnlMot6_M_Click(object sender, EventArgs e)
        {
            MnlTBArray[6].Text = TendrilUtils.SubtractText((MnlTBArray[6].Text == "" ? "0" : MnlTBArray[6].Text), TendrilState.GetSmlIncrement().ToString());
            btnMnlSend.PerformClick();
        }

        private void btnMnlMot6_P_Click(object sender, EventArgs e)
        {
            MnlTBArray[6].Text = TendrilUtils.AddText((MnlTBArray[6].Text == "" ? "0" : MnlTBArray[6].Text), TendrilState.GetSmlIncrement().ToString());
            btnMnlSend.PerformClick();
        }

        private void btnMnlMot6_PP_Click(object sender, EventArgs e)
        {
            MnlTBArray[6].Text = TendrilUtils.AddText((MnlTBArray[6].Text == "" ? "0" : MnlTBArray[6].Text), TendrilState.GetLrgIncrement().ToString());
            btnMnlSend.PerformClick();
        }

        private void btnMnlMot7_MM_Click(object sender, EventArgs e)
        {
            MnlTBArray[7].Text = TendrilUtils.SubtractText((MnlTBArray[7].Text == "" ? "0" : MnlTBArray[7].Text), TendrilState.GetLrgIncrement().ToString());
            btnMnlSend.PerformClick();
        }

        private void btnMnlMot7_M_Click(object sender, EventArgs e)
        {
            MnlTBArray[7].Text = TendrilUtils.SubtractText((MnlTBArray[7].Text == "" ? "0" : MnlTBArray[7].Text), TendrilState.GetSmlIncrement().ToString());
            btnMnlSend.PerformClick();
        }

        private void btnMnlMot7_P_Click(object sender, EventArgs e)
        {
            MnlTBArray[7].Text = TendrilUtils.AddText((MnlTBArray[7].Text == "" ? "0" : MnlTBArray[7].Text), TendrilState.GetSmlIncrement().ToString());
            btnMnlSend.PerformClick();
        }

        private void btnMnlMot7_PP_Click(object sender, EventArgs e)
        {
            MnlTBArray[7].Text = TendrilUtils.AddText((MnlTBArray[7].Text == "" ? "0" : MnlTBArray[7].Text), TendrilState.GetLrgIncrement().ToString());
            btnMnlSend.PerformClick();
        }

        private void btnMnlMot8_MM_Click(object sender, EventArgs e)
        {
            MnlTBArray[8].Text = TendrilUtils.SubtractText((MnlTBArray[8].Text == "" ? "0" : MnlTBArray[8].Text), TendrilState.GetLrgIncrement().ToString());
            btnMnlSend.PerformClick();
        }

        private void btnMnlMot8_M_Click(object sender, EventArgs e)
        {
            MnlTBArray[8].Text = TendrilUtils.SubtractText((MnlTBArray[8].Text == "" ? "0" : MnlTBArray[8].Text), TendrilState.GetSmlIncrement().ToString());
            btnMnlSend.PerformClick();
        }

        private void btnMnlMot8_P_Click(object sender, EventArgs e)
        {
            MnlTBArray[8].Text = TendrilUtils.AddText((MnlTBArray[8].Text == "" ? "0" : MnlTBArray[8].Text), TendrilState.GetSmlIncrement().ToString());
            btnMnlSend.PerformClick();
        }

        private void btnMnlMot8_PP_Click(object sender, EventArgs e)
        {
            MnlTBArray[8].Text = TendrilUtils.AddText((MnlTBArray[8].Text == "" ? "0" : MnlTBArray[8].Text), TendrilState.GetLrgIncrement().ToString());
            btnMnlSend.PerformClick();
        }

        private void btMnlSend_Click(object sender, EventArgs e)
        {
            String s = "";
            for (int i = 0; i < MnlTBArray.Length; i++)
            {
                s += (MnlTBArray[i].Text == "" ? "0" : MnlTBArray[i].Text);
                if (i < MnlTBArray.Length - 1) s += ",";
            }
            btSerialPort.WriteLine("Read," + s);
            Console.WriteLine(s);

            // Update singleton encoder targets
            TendrilState.UpdateEncoderTargets(s);

        }

        private void btnMnlClose_Click(object sender, EventArgs e)
        {
            Width = Width - pnlMnlControl.Width;
            pnlMnlControl.Enabled = false;
            pnlMnlControl.Visible = false;
        }



        //////////////////////////////////////////////////////////////////////
        //////////////////////  Zeroing Tension Values  //////////////////////
        //////////////////////////////////////////////////////////////////////







        //////////////////////////////////////////////////////////////////////
        ////////////////////  Evening out Section Tensions  //////////////////
        //////////////////////////////////////////////////////////////////////
        private void btnEvenTension_MouseClick(object sender, MouseEventArgs e)
        {
            if (ddmSectionSelect.Text == "")
            {
                PopupNotifier pnMsg = new PopupNotifier();
                pnMsg.TitleText = "No Selection";
                pnMsg.ContentText = "Please select a section to even.";
                pnMsg.Popup(); // Show
            } else
            {
                cntsCurrent = cntStatusLabel.Text;
                
                evenOut = ddmSectionSelect.Text;
                System.Console.WriteLine("even " + evenOut);
            }
        }
        private void btnStopEven_Click(object sender, EventArgs e)
        {
            evenOut = "";
        }

        //////////////////////////////////////////////////////////////////
        ////////////////////   BlueTooth Serial Comms   //////////////////
        //////////////////////////////////////////////////////////////////

        private void btnConnect_MouseClick(object sender, MouseEventArgs e)
        {
            int count = 0;
            btSerialPort.PortName = tbComPort.Text;
            if (btSerialPort.PortName == "")
            {
                connectLabel.Text = "Not a valid Port name.";
                return;
            }
            
            while (!BT_Connect())
            {
                System.Threading.Thread.Sleep(500);
                count++;
                if (count > 200)
                {
                    connectLabel.Text = "Failed to open connection.";
                    return;
                }
            }
            
            connectLabel.Text = "Connected.";
            btSerialPort.WriteLine("Connect,");
            //Console.WriteLine("Connect command sent.");
            btnConnect.Enabled = false;
            btnDisconnect.Enabled = true;

        }
        bool BT_Connect ()
        {
            try
            {
                btSerialPort.Open();
            }
            catch(Exception e)
            {
                Console.WriteLine("Exception thrown: (StackTrace)");
                Console.WriteLine(e.StackTrace);
                return false;
            }
            return true;
        }

        private void btnDisconnect_MouseClick(object sender, MouseEventArgs e)
        {
            if (btSerialPort.IsOpen && !flgProcessing)
            {
                btSerialPort.WriteLine("Disconnect,");
                Console.WriteLine("Disconnect command sent.");
                btSerialPort.Close();
                connectLabel.Text = "Disconnected.";
                btnConnect.Enabled = true;
            }
        }

        private void btSerialPort_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            if (btSerialPort.IsOpen && !flgProcessing)
            {
                flgProcessing = true;
                rxString = btSerialPort.ReadLine();
                this.Invoke(new EventHandler(NewData));
            }
        }

        //
        // Main event handler for handling new data
        //

        private void NewData(object sender, EventArgs e)
        {
            // Sanitizing inputs
            String[] lines = rxString.Split('\t');
            if (lines.Length != 2) return;
            String[] evenOutput;
            String tempString;
            bool targetMet = true;

            cntStatusLabel.Text = lines[1];
            cntsFeedback = lines[1].Split(',');
            ParseCounts(cntsFeedback);
            tenStatusLabel.Text = lines[0];
            tensFeedback = lines[0].Split(',');

            if (tensFeedback.Length != 9 || cntsFeedback.Length != 9) return;

            TendrilState.UpdateTensions(tensFeedback);
            TendrilState.UpdateEncoders(cntsFeedback);

            cntsSim = simCurrTxLabel.Text.Split(',');

            TendrilUtils.SetProgressBars(this, tensFeedback);
            TendrilUtils.SetValueLabels(this, tensFeedback);
            if (!(evenOut == ""))
            {
                evenOutput = TendrilUtils.EvenOutSection(evenOut, cntsFeedback, tensFeedback, btSerialPort);
                if (evenOutput[0] == "Done")
                {
                    System.Console.WriteLine("Finished even Procedure.");
                    evenOut = "";
                    btSerialPort.WriteLine("Write," + cntsCurrent);
                }
                else
                {
                    //System.Console.WriteLine("Writing...");
                    tempString = "";
                    for (int i = 0; i < evenOutput.Length; i++)
                    {
                        tempString += evenOutput[i];
                        if (i < evenOutput.Length - 1) tempString += ",";
                    }
                    simCurrTxLabel.Text = tempString;
                    btSerialPort.WriteLine("Read," + tempString);
                }
            } 
                
            if (simRunning)
            {
                
                //System.Console.WriteLine("Sim Running...");
                if (simCurrTxLabel.Text == "End of file.")
                {
                    btnPause.PerformClick();
                    //time.Enabled = false;
                    //simRunning = false;
                    return;
                }
                for (int i = 0; i < cntsFeedback.Length; i++)
                {
                    targetMet &= (Math.Abs(Int32.Parse(cntsFeedback[i]) - Int32.Parse(cntsSim[i])) < ERR_THRESH);
                }
                if (targetMet)
                {
                    //time.Enabled = true;
                    //if (flgTimer)
                    //{
                        simCurrTxLabel.Text = simFile.CSV_ReadNextLine();
                        if (simCurrTxLabel.Text != "End of file.")
                        {
                            cntsSim = simCurrTxLabel.Text.Split(',');
                            btSerialPort.WriteLine("Read," + simCurrTxLabel.Text);
                            flgTimer = false;
                            time.Enabled = false;
                        }
                        else btnPause.PerformClick();
                    //}
                    
                }


            }
            if (flgWriting && flgWriteAll)
            {
                outputFile.CSV_WriteLine(outputFile.CSV_PrepareLog(TendrilState.GetEncoders(),TendrilState.GetTensions()));
            }
            flgProcessing = false;

        }

        public void ParseCounts(String[] cnts)
        {
            for (int ndx = 0; ndx < 9; ndx++)
            {
                encValues[ndx] = float.Parse(cnts[ndx]);
            }
        }

        private void OnTimeElapsed(Object source, System.Timers.ElapsedEventArgs e)
        {
            flgTimer = true;
        }

        private void TendrilInterface_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.NumPad0)
            {
                Console.WriteLine("You're pressing the Numpad 0 key");
            }
        }

        private void simulationToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
            Thread simulationThread = new Thread(delegate ()
            {
                glSimDisplay = new OpenGLSim(800, 720, 32, 16, 8);
            });

            simulationThread.SetApartmentState(ApartmentState.STA); // needs to be STA or throws exception
            simulationThread.Start();
            //System.Diagnostics.Process.Start(@"OpenGLTutorial.exe");
//, @"/k C:\Users\mbwoo\Google Drive\School Files\Research\Tendril\Software\TendrilInterface\OpenGLTutorial\OpenGLTutorial\bin\Release\");
        }


        private void kalmanFilterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            kalmanFilterToolStripMenuItem.Checked = !kalmanFilterToolStripMenuItem.Checked;
        }

        


    }
}
