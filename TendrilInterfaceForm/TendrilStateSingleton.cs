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

        // Tendril parameters
        private int[] SensorReadings;
        private int[] Tension;
        private int[] Encoder;
        private float[] TendonLength;
        private int FirstMotor;
        private int LastMotor;


        //Tracking parameters
        private float[] SensorVelocity;
        private float[] TensionVelocity;
        private float[] EncoderVelocity; 


        // Control system parameters



        

        /* ============= Methods ===============*/

        private TendrilStateSingleton()
        {
            //setup and initialization
            SensorReadings = new int[9];
            Tension = new int[9];
            Encoder = new int[9];
            TendonLength = new float[9];
            FirstMotor = 0;
            LastMotor = 8;
        }

        public static TendrilStateSingleton getInstance()
        {
            if (instance == null)
            {
                instance = new TendrilStateSingleton();
            }

            return instance;
        }
             
    }
}
