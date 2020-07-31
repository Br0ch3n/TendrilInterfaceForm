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



        // Control system parameters *Jones*


        

        /* ============= Methods ===============*/

        private TendrilStateSingleton()
        {
            //setup and initialization
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
