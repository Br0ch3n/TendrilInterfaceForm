﻿using System;
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

        private CSVReader simFile;
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
        private bool flgProcessing, flgWriting, flgTimer;
        private OpenGLSim glSimDisplay;
        private KalmanFilter[] KFTensions;
        private System.Timers.Timer time;

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
            ERR_THRESH = 20;
            MnlTBArray = new TextBox[] { tbMnlMot0, tbMnlMot1, tbMnlMot2, tbMnlMot3, tbMnlMot4, tbMnlMot5, tbMnlMot6, tbMnlMot7, tbMnlMot8 };
            KFTensions = new KalmanFilter[9];
            encValues = new float[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            time = new System.Timers.Timer();

            time.Interval = 10000;
            time.Elapsed += OnTimeElapsed;
            flgTimer = false;

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
        private void btnCSVWriterBegin_Click(object sender, EventArgs e)
        {
            if (outputFile != null && outputFile.strmActive)
            {
                flgWriting = true;
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
            btnCSVWriterBegin.Enabled = true;
            btnCSVWriterEnd.Enabled = false;
        }

        

        //////////////////////////////////////////////////////////////////////
        ////////////////////       Manual Control Code      //////////////////
        //////////////////////////////////////////////////////////////////////

        private void manualControlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Width = Width + pnlMnlControl.Width;
            pnlMnlControl.Enabled = true;
            pnlMnlControl.Visible = true;
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

        private void NewData(object sender, EventArgs e)
        {
            String[] lines = rxString.Split('\t');
            if (lines.Length != 2) return;
            String[] evenOutput;
            String tempString;
            bool targetMet = true;

            ProcessTendrilInput(lines);

            if (tensFeedback.Length != 9 || cntsFeedback.Length != 9) return;
            cntsSim = simCurrTxLabel.Text.Split(',');

            TendrilUtils.SetProgressBars(this, tensFeedback);
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
            if (flgWriting)
            {
                outputFile.CSV_WriteLine(outputFile.CSV_PrepareLog(cntsFeedback,tensFeedback));
            }
            flgProcessing = false;

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

        void ProcessTendrilInput(String[] input)
        {
            //String[] tenTemp = input[0].Split(',');
            //String temp = "";
            
            cntStatusLabel.Text = input[1];
            cntsFeedback = input[1].Split(',');
            //glSimDisplay.JModel.Update(GetCounts(cntsFeedback))
            ParseCounts(cntsFeedback);
            //glSimDisplay.SetCounts(encValues);
            tenStatusLabel.Text = input[0];
            tensFeedback = input[0].Split(',');
            //for (int i  = 0; i < tenTemp.Length; i++) tenAvg[i].AddValue(Int32.Parse(tenTemp[i]));

            //for (int i = 0; i < tenTemp.Length; i++)
            //{
            //    temp += tenAvg[i].avgI.ToString();
            //    if (i < tenTemp.Length - 1) temp += ',';
            //    tensFeedback[i] = tenAvg[i].avgI.ToString();
            //}
            //tenStatusLabel.Text = temp;
        }

        public void ParseCounts(String[] cnts)
        {
            for (int ndx = 0; ndx < 9; ndx++)
            {
                encValues[ndx] = float.Parse(cnts[ndx]);
            }
        }


    }
}