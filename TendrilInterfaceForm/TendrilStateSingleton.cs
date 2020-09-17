using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        private float LrgMotShaftDiameter, SmlMotShaftDiameter;
        private float BaseMass, MidMass, TipMass;
        private float BaseStiff, MidStiff, TipStiff;
        private float[] CalibrationScale;
        private float[] CalibrationOffset;
        private float SpacerHoleRadius, TendonDiameter;



        //Tracking parameters
        private float[] SensorVelocity;
        private float[] TensionVelocity;
        private float[] EncoderVelocity; 


        

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

            //RetrieveConfigFile();
            BaseLength = 0;
            MidLength = 0;
            TipLength = 0;
            LrgMotShaftDiameter = 0;
            SmlMotShaftDiameter = 0;
            CalibrationOffset = new float[9];
            CalibrationScale = new float[9];

        }

        public static TendrilStateSingleton getInstance()
        {
            if (instance == null)
            {
                instance = new TendrilStateSingleton();
            }

            return instance;
        }

        public void SetTensions(String[] input)
        {
            for (int i = 0; i < input.Length; i++)
            {
                this.SensorReading[i] =  int.Parse(input[i]);
            }
            ProcessTendrilInput();
        }

        public void SetEncoders(String[] input)
        {
            for (int i = 0; i < input.Length; i++)
            {
                this.Encoder[i] = int.Parse(input[i]);
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
    }
}
