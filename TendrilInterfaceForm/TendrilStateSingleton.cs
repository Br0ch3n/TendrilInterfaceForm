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
    public sealed class TendrilStateSingleton
    {
        /* ============= Members ===============*/
        // Architecture members
        private static TendrilStateSingleton instance = null;
        private static readonly object _lock = new object();

        // Tendril variables
        private int[] SensorReading;
        private float[] Tension;
        private float[] oldTension;
        private float[] DeltaTension;

        private int[] Encoder;
        private int[] EncoderTarget;

        private float[] TendonLength;

        private int FirstMotor;
        private int LastMotor;

        private int EncSmlIncrement;
        private int EncLrgIncrement;

        // Tendril physical parameters
        private float BaseLength, MidLength, TipLength;
        private float BaseMass, MidMass, TipMass; // 13g = base mass
        private float BaseModulus, MidModulus, TipModulus;
        private float BaseMomentInertia, MidMomentInertia, TipMomentInertia;

        private int[] SpacerCount;
        private float[] SpacerRadius;

        private float SpacerHoleDiameter, TendonDiameter;
        private float LrgMotShaftDiameter, SmlMotShaftDiameter;
        private int CountsPerRotationSmall, CountsPerRotationLarge;
        private float LengthPerRotationLarge, LengthPerRotationSmall;

        private float[] CalibrationScale;
        private float[] CalibrationOffset;

        //Tracking variables
        private bool FilteringActive;
        private KalmanFilter[] TrackedSensor;
        private KalmanFilter[] TrackedTension;
        private KalmanFilter[] TrackedEncoder;
        private KalmanFilter[] TrackedTendonLength;

        //Modelling variables

        JonesModel jonesModel;

        BajoModel bajoModel;


        /* ============= Methods ===============*/

        public static TendrilStateSingleton Instance
        {
            //lock (_lock)
            //{
                
            //}
            get
            {
                if (instance != null) return instance;

                lock (_lock)
                {
                    if (instance == null)
                    {
                        Console.WriteLine("Instance is null...");
                        instance = new TendrilStateSingleton();
                    }
                }

                if (instance == null) throw new Exception("WTF it is still sending null");
                return instance;
            }

        }

        static TendrilStateSingleton()
        {
            Console.WriteLine("Static Constructor called");
        }
        private TendrilStateSingleton()
        {
            Console.WriteLine("1");
            // setup and initialization
            SensorReading = new int[9];
            Tension = new float[9];
            DeltaTension = new float[9];
            oldTension = new float[9];
            Encoder = new int[9];
            EncoderTarget = new int[9];
            TendonLength = new float[9];
            FirstMotor = 0;
            LastMotor = 8; // Normally 8, because 9 motors
            EncSmlIncrement = 50;
            EncLrgIncrement = 200;
            FilteringActive = false;

            SpacerCount = new int[3];
            SpacerRadius = new float[3];

            
            CalibrationOffset = new float[9];
            CalibrationScale = new float[9];

            ReadConfigFile();

            LengthPerRotationLarge = (float)Math.PI * LrgMotShaftDiameter;

            // Tracking system initialization
            TrackedSensor = new KalmanFilter[9];
            TrackedTension = new KalmanFilter[9];
            TrackedEncoder = new KalmanFilter[9];
            TrackedTendonLength = new KalmanFilter[9];
            
            for (int ndx = FirstMotor; ndx <= LastMotor; ndx++)
            {
                TrackedSensor[ndx] = new KalmanFilter(.009f, 1.0f);
                TrackedTension[ndx] = new KalmanFilter(.009f, 1.0f);
                TrackedEncoder[ndx] = new KalmanFilter(.009f, 1.0f);
                TrackedTendonLength[ndx] = new KalmanFilter(.009f, 1.0f);
            }
            
            
        }

        public void InitializeModels()
        {
            jonesModel = new JonesModel();
            bajoModel = new BajoModel();
        }


        public void ReadConfigFile(string s)
        {
           //string s;
            string[] lines, configParams, CalibOffsets, CalibScales;
            // Displays an OpenFileDialog so the user can select a Cursor.  
            //OpenFileDialog openFileDialog1 = new OpenFileDialog();
            //openFileDialog1.Filter = "txt files (*.txt)|*.txt";
            //openFileDialog1.Title = "Select a tendril configuration file.";

            // Show the Dialog.  
            // If the user clicked OK in the dialog and  
            // a .CUR file was selected, open it.  
            //if (openFileDialog1.ShowDialog() == DialogResult.OK)
            //{
            //s = File.ReadAllText(openFileDialog1.FileName);
            lines = s.Split(';');
            if (lines.Length != 4) Console.WriteLine("Config File not 4 lines.");
            configParams = lines[0].Split(',');
            if (configParams.Length != 22) Console.WriteLine("Config File first line not 22 parameters.");
            CalibScales = lines[1].Split(',');
            CalibOffsets = lines[2].Split(',');
            BaseLength = float.Parse(configParams[0]) / 1000;
            MidLength = float.Parse(configParams[1]) / 1000;
            TipLength = float.Parse(configParams[2]) / 1000;
            BaseMass = float.Parse(configParams[3]) / 1000;
            MidMass = float.Parse(configParams[4]) / 1000;
            TipMass = float.Parse(configParams[5]) / 1000;
            BaseModulus = float.Parse(configParams[6]);
            MidModulus = float.Parse(configParams[7]);
            TipModulus = float.Parse(configParams[8]);
            SpacerHoleDiameter = float.Parse(configParams[9]);
            TendonDiameter = float.Parse(configParams[10]);
            LrgMotShaftDiameter = float.Parse(configParams[11]);
            SmlMotShaftDiameter = float.Parse(configParams[12]);
            CountsPerRotationLarge = Int32.Parse(configParams[13]);
            CountsPerRotationSmall = Int32.Parse(configParams[14]);
            SpacerRadius[0] = float.Parse(configParams[15]);
            SpacerRadius[1] = float.Parse(configParams[16]);
            SpacerRadius[2] = float.Parse(configParams[17]);
            SpacerCount[0] = Int32.Parse(configParams[18]);
            SpacerCount[1] = Int32.Parse(configParams[19]);
            SpacerCount[2] = Int32.Parse(configParams[20]);
            BaseMomentInertia = float.Parse(configParams[21]);

            //Console.WriteLine("Moment of Inertia = " + BaseMomentInertia.ToString());
            

            for (int i = 0; i < CalibrationOffset.Length; i++)
            {
                CalibrationOffset[i] = float.Parse(CalibOffsets[i]);
                CalibrationScale[i] = float.Parse(CalibScales[i]);
            }
            //}
            //else Console.WriteLine("Dialog Box Not OK?");
        }

        public void ReadConfigFile()
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
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                s = File.ReadAllText(openFileDialog1.FileName);
                lines = s.Split(';');
                if (lines.Length != 4) Console.WriteLine("Config File not 4 lines.");
                configParams = lines[0].Split(',');
                if (configParams.Length != 22) Console.WriteLine("Config File first line not 22 parameters.");
                CalibScales = lines[1].Split(',');
                CalibOffsets = lines[2].Split(',');
                
                BaseLength = float.Parse(configParams[0]) / 1000;
                MidLength = float.Parse(configParams[1]) / 1000;
                TipLength = float.Parse(configParams[2]) / 1000;
                BaseMass = float.Parse(configParams[3]) / 1000;
                MidMass = float.Parse(configParams[4]) / 1000;
                TipMass = float.Parse(configParams[5]) / 1000;
                BaseModulus = float.Parse(configParams[6]);
                MidModulus = float.Parse(configParams[7]);
                TipModulus = float.Parse(configParams[8]);
                SpacerHoleDiameter = float.Parse(configParams[9]);
                TendonDiameter = float.Parse(configParams[10]);
                LrgMotShaftDiameter = float.Parse(configParams[11]);
                SmlMotShaftDiameter = float.Parse(configParams[12]);
                CountsPerRotationLarge = Int32.Parse(configParams[13]);
                CountsPerRotationSmall = Int32.Parse(configParams[14]);
                SpacerRadius[0] = float.Parse(configParams[15]);
                SpacerRadius[1] = float.Parse(configParams[16]);
                SpacerRadius[2] = float.Parse(configParams[17]);
                SpacerCount[0] = Int32.Parse(configParams[18]);
                SpacerCount[1] = Int32.Parse(configParams[19]);
                SpacerCount[2] = Int32.Parse(configParams[20]);
                BaseMomentInertia = float.Parse(configParams[21]);

                for (int i = 0; i < CalibrationOffset.Length; i++)
                {
                    CalibrationOffset[i] = float.Parse(CalibOffsets[i]);
                    CalibrationScale[i] = float.Parse(CalibScales[i]);
                }
            }
            else Console.WriteLine("Dialog Box Not OK?");
        }

        public void UpdateTensions(String[] input)
        {
            for (int i = 0; i < input.Length; i++)
            {
                this.SensorReading[i] =  Int32.Parse(input[i]);
            }
            ProcessTendrilInput();
        }

        public void UpdateEncoders(String[] input)
        {
            for (int i = 0; i < input.Length; i++)
            {
                this.Encoder[i] = Int32.Parse(input[i]);
            }

            // convert motor counts to lengths for tendril
            for (int i = FirstMotor; i <= LastMotor; i++)
            {
                if (i < 3)
                {
                    TendonLength[i] = this.Encoder[i] / (-(CountsPerRotationLarge / LengthPerRotationLarge)) + BaseLength;
                } else if (i >= 6)
                {
                    TendonLength[i] = this.Encoder[i] / (-(CountsPerRotationSmall / LengthPerRotationSmall)) + TipLength;
                } else
                {
                    TendonLength[i] = this.Encoder[i] / (-(CountsPerRotationSmall / LengthPerRotationSmall)) + MidLength;
                }
            }
        }

        public void ProcessTendrilInput()
        {
           
            oldTension = Tension;
            for (int i = FirstMotor; i <= LastMotor; i++)
            {
                this.Tension[i] = (float)((CalibrationScale[i] * (float)SensorReading[i]) + (float)CalibrationOffset[i]);
                DeltaTension[i] = Tension[i] - oldTension[i];
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

        public float[] GetDeltaTension()
        {
            return DeltaTension;
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

        public float GetSectionMomentInertia (int section)
        {
            if (section < TendrilUtils.BASE_SECTION && section > TendrilUtils.TIP_SECTION) return 0.0f;

            if (section == TendrilUtils.BASE_SECTION) return BaseMomentInertia;
            else if (section == TendrilUtils.MID_SECTION) return MidMomentInertia;
            else return TipMomentInertia;
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

        public float[] GetTensionsFloats()
        {
            return Tension;
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

        public float GetSpacerRadius(int section)
        {
            return SpacerRadius[section];
        }

        public int GetSpacerCount(int section)
        {
            return SpacerCount[section];
        }

        // Filtering code

        public void StartFiltering()
        {
            FilteringActive = true;
        }

        public void UpdateFilters()
        {
            if (FilteringActive)
            {
                for (int i = FirstMotor; i <= LastMotor; i++)
                {
                    TrackedSensor[i].Predict();
                    TrackedTension[i].Predict();
                    TrackedEncoder[i].Predict();
                    TrackedTendonLength[i].Predict();
                }

                for (int i = FirstMotor; i <= LastMotor; i++)
                {
                    TrackedSensor[i].Update(SensorReading[i]); 
                    TrackedTension[i].Update(Tension[i]);
                    TrackedEncoder[i].Update(Encoder[i]);
                    TrackedTendonLength[i].Update(TendonLength[i]);
                }
            }
        }

        public void UpdateModels()
        {
            jonesModel.Update(TendonLength, TendrilUtils.BASE_SECTION);
            bajoModel.ContactDetection(TendrilUtils.BASE_SECTION, jonesModel.CreateSingleSectionJacobian(TendrilUtils.BASE_SECTION));
        }

        public float GetCurvature(int section)
        {
            return (float)jonesModel.getCurvature(section);
        }

        public float GetCurveAngle(int section)
        {
            return (float)jonesModel.getPhi(section);
        }

        public void PrintBajoModelData()
        {
            

            Console.Write("Sensor Values: ");
            for (int i = FirstMotor; i <= LastMotor; i++)
            {
                Console.Write(SensorReading[i].ToString() + ", ");
            }
            Console.WriteLine(" ");

            Console.Write("Tension Values: ");
            for (int i = FirstMotor; i <= LastMotor; i++)
            {
                Console.Write(Tension[i].ToString() + ", ");
            }
            Console.WriteLine(" ");

            Console.Write("Calib Scales: ");
            for (int i = FirstMotor; i <= LastMotor; i++)
            {
                Console.Write(CalibrationScale[i].ToString() + ", ");
            }
            Console.WriteLine(" ");

            Console.Write("Calib Offsets: ");
            for (int i = FirstMotor; i <= LastMotor; i++)
            {
                Console.Write(CalibrationOffset[i].ToString() + ", ");
            }
            Console.WriteLine(" ");

            bajoModel.PrintBajoOuput();
        }
    }
}
