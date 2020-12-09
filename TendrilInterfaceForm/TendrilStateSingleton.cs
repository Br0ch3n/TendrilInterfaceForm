using System;
using System.IO;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TendrilInterfaceForm
{
    public class TendrilStateSingleton
    {
        /* ============= Members ===============*/
        // Architecture members
        private static TendrilStateSingleton instance = null;

        // Tendril variables
        private int[] SensorReading;
        private float[] Tension;
        private float[] TensionTarget;

        private int[] Encoder;
        private int[] EncoderTarget;

        private float[] TendonLength;

        private int FirstMotor;
        private int LastMotor;

        private int EncSmlIncrement;
        private int EncLrgIncrement;

        // Tendril physical parameters
        private float BaseLength, MidLength, TipLength;
        private float BaseMass, MidMass, TipMass;
        private float BaseModulus, MidModulus, TipModulus;

        private float SpacerHoleDiameter, TendonDiameter;
        private float LrgMotShaftDiameter, SmlMotShaftDiameter;
        private int CountsPerRotationSmall, CountsPerRotationLarge;
        private float LengthPerRotationLarge, LengthPerRotationSmall;

        private float[] CalibrationScale;
        private float[] CalibrationOffset;




        //Tracking variables
        private KalmanFilter[] TrackedSensor;
        private KalmanFilter[] TrackedTension;
        private KalmanFilter[] TrackedEncoder;
        private KalmanFilter[] TrackedTendonLength;


        

        /* ============= Methods ===============*/

       


        private TendrilStateSingleton()
        {
            // setup and initialization
            SensorReading = new int[9];
            Tension = new float[9];
            TensionTarget = new float[9];
            Encoder = new int[9];
            EncoderTarget = new int[9];
            TendonLength = new float[9];
            FirstMotor = 0;
            LastMotor = 8;
            EncSmlIncrement = 50;
            EncLrgIncrement = 200;
            
            
            CalibrationOffset = new float[9];
            CalibrationScale = new float[9];

            ReadConfigFile();
        }

        public static TendrilStateSingleton getInstance()
        {
            if (instance == null)
            {
                instance = new TendrilStateSingleton();
            }

            return instance;
        }


        private void ReadConfigFile()
        {
            string s;
            string[] lines, configParams, CalibOffsets, CalibScales;
            // Displays an OpenFileDialog so the user can select a Cursor.  
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "txt files (*.txt)|*.txt";
            openFileDialog1.Title = "Select a tendril configuration file.";

            // Show the Dialog.  
            // If the user clicked OK in the dialog and  
            // a .CUR file was selected, open it.  
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                s = File.ReadAllText(openFileDialog1.FileName);
                lines = s.Split(';');
                if (lines.Length != 3) Console.WriteLine("Config File not 3 lines.");
                configParams = lines[0].Split(',');
                if (configParams.Length != 17) Console.WriteLine("Config File first line not 17 parameters.");
                CalibOffsets = lines[1].Split(',');
                CalibScales = lines[2].Split(',');
                BaseLength = float.Parse(configParams[0]);
                MidLength = float.Parse(configParams[1]);
                TipLength = float.Parse(configParams[2]);
                BaseMass = float.Parse(configParams[3]);
                MidMass = float.Parse(configParams[4]);
                TipMass = float.Parse(configParams[5]);
                BaseModulus = float.Parse(configParams[6]);
                MidModulus = float.Parse(configParams[7]);
                TipModulus = float.Parse(configParams[8]);
                SpacerHoleDiameter = float.Parse(configParams[9]);
                TendonDiameter = float.Parse(configParams[10]);
                LrgMotShaftDiameter = float.Parse(configParams[11]);
                SmlMotShaftDiameter = float.Parse(configParams[12]);
                CountsPerRotationLarge = Int32.Parse(configParams[13]);
                CountsPerRotationSmall = Int32.Parse(configParams[14]);
                LengthPerRotationLarge = float.Parse(configParams[15]);
                LengthPerRotationSmall = float.Parse(configParams[16]);
                for(int i = 0; i < CalibrationOffset.Length; i++)
                {
                    CalibrationOffset[i] = float.Parse(CalibOffsets[i]);
                    CalibrationScale[i] = float.Parse(CalibScales[i]);
                }
            }
        }

        public void UpdateTensions(String[] input)
        {
            for (int i = 0; i < input.Length; i++)
            {
                this.SensorReading[i] =  int.Parse(input[i]);
            }
            ProcessTendrilInput();
        }

        public void UpdateEncoders(String[] input)
        {
            for (int i = 0; i < input.Length; i++)
            {
                this.Encoder[i] = Int32.Parse(input[i]);
            }


            // Add update to tendon lengths
            //convert motor counts to lengths for tendril
            for (int i = 0; i < 3; i++)

                // How are we storing S, K, Phi?
            {
                //base_s[i] = this.Encoder[i] / (-(CountsPerRotationLarge / LengthPerRotationLarge)) + BaseLength;
                //mid_s[i] = this.Encoder[i + 3] / (-(CountsPerRotationLarge / LengthPerRotationSmall)) + MidLength; 
                //tip_s[i] = this.Encoder[i + 6] / (-(CountsPerRotationSmall / LengthPerRotationSmall)) + TipLength;
            }


        }

        public void ProcessTendrilInput()
        {
            for (int i = FirstMotor; i <= LastMotor; i++)
            {
                this.Tension[i] = CalibrationScale[i] * SensorReading[i] + CalibrationOffset[i];
            }
        }

        public int GetSmlIncrement()
        {
            return EncSmlIncrement;
        }

        public int GetLrgIncrement()
        {
            return EncLrgIncrement;
        }

        public void UpdateEncoderTargets(String input)
        {
            String[] temp;
            temp = input.Split(',');
            for (int i = FirstMotor; i <= LastMotor; i++)
            {
                this.EncoderTarget[i] = Int32.Parse(temp[i]);
            }
        }

        public float GetSectionLength(int section)
        {
            if (section < TendrilUtils.BASE_SECTION && section > TendrilUtils.TIP_SECTION) return 0.0f;

            if (section == TendrilUtils.BASE_SECTION) return BaseLength;
            else if (section == TendrilUtils.MID_SECTION) return MidLength;
            else return TipLength;

        }

        public float GetSectionStiffness(int section)
        {
            if (section < TendrilUtils.BASE_SECTION && section > TendrilUtils.TIP_SECTION) return 0.0f;

            if (section == TendrilUtils.BASE_SECTION) return BaseModulus;
            else if (section == TendrilUtils.MID_SECTION) return MidModulus;
            else return TipModulus;
        }
        
        public float GetSectionMass(int section)
        {
            if (section < TendrilUtils.BASE_SECTION && section > TendrilUtils.TIP_SECTION) return 0.0f;

            if (section == TendrilUtils.BASE_SECTION) return BaseMass;
            else if (section == TendrilUtils.MID_SECTION) return MidMass;
            else return TipMass;
        }

        public float GetMotorShaftDiameter(int motor)
        {
            if (motor < TendrilUtils.LRG_MOTOR && motor > TendrilUtils.SML_MOTOR) return 0.0f;

            if (motor == TendrilUtils.LRG_MOTOR) return LrgMotShaftDiameter;
            else return SmlMotShaftDiameter;
        }

        public float GetSpacerHoleDiamter()
        {
            return SpacerHoleDiameter;
        }

        public float GetTendonDiameter()
        {
            return TendonDiameter;
        }

        public String[] GetTensions()
        {
            String[] s = new String[9];

            for (int i = FirstMotor; i < LastMotor; i++)
            {
                s[i] = Tension[i].ToString();
            }

            return s;
        }

        public String[] GetEncoders()
        {
            String[] s = new String[9];

            for (int i = FirstMotor; i < LastMotor; i++)
            {
                s[i] = Encoder[i].ToString();
            }

            return s;
        }
    }
}
