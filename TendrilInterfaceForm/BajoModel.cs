using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenGL;

namespace TendrilInterfaceForm
{
    class BajoModel
    {
        // List of variables

        private float ContactThreshold { get; set; }
        private Matrix3 Jacobian;
        private Vector3 TensionLoads;
        private DateTime timeLast, timeCurrent;
        private TimeSpan timeElapsed;
        private bool PrintEnabled;


        //private TendrilStateSingleton tendrilState;

        // initialization

        public BajoModel()
        {
            ContactThreshold = 10;
            TensionLoads = new Vector3();
            Jacobian = new Matrix3();
            timeLast = new DateTime();
            timeCurrent = new DateTime();
            timeElapsed = new TimeSpan();
            timeLast = DateTime.Now;
            PrintEnabled = false;
            
        }

        public void ContactDetection(int section, Matrix3 J)
        {
            TendrilStateSingleton tendrilState = TendrilStateSingleton.Instance;
            float[] dTension = new float[9];
            float dT = new float();

            timeCurrent = DateTime.Now;

            timeElapsed = timeCurrent.Subtract(timeLast);

            timeLast = timeCurrent;

            CalculateTensionLoads(section);

            //dTension = tendrilState.GetDeltaTension();
            dTension = tendrilState.GetTensionsFloats();
            //dT = (float)timeElapsed.TotalSeconds;
            dT = 0.0098f;

            Vector3 Tao = new Vector3(dTension[0] * dT, dTension[1] * dT, dTension[2] * dT);
            
            Jacobian = J.Transpose();

            Vector3 result = (Jacobian * Tao - TensionLoads);
           

            double Theta = Math.Sqrt(Math.Pow(result.X,2) + Math.Pow(result.Y, 2) + Math.Pow(result.Z, 2));

            if (PrintEnabled)
            {
                Console.WriteLine("K: " + tendrilState.GetCurvature(section).ToString() + ", Phi : " + tendrilState.GetCurveAngle(section).ToString());
                Console.WriteLine("Delta Tension: " + dTension[0].ToString() + ", " + dTension[1].ToString() + ", " + dTension[2].ToString());
                Console.WriteLine("Time Interval: " + timeElapsed.TotalSeconds.ToString() + " seconds");
                Console.WriteLine("Tao: " + Tao[0].ToString() + ", " + Tao[1].ToString() + ", " + Tao[2].ToString());
                Console.WriteLine("Result: " + result[0].ToString() + ", " + result[1].ToString() + ", " + result[2].ToString());
                if (Theta > ContactThreshold) Console.WriteLine("CONTACT! Theta = " + Theta.ToString());
                else Console.WriteLine("No contact... Theta = " + Theta.ToString());
                PrintEnabled = false;
            }
        }

        private void CalculateTensionLoads(int section)
        {
            TendrilStateSingleton tendrilState = TendrilStateSingleton.Instance;

            float m = tendrilState.GetSectionMass(section);
            float L = tendrilState.GetSectionLength(section);

            float Theta = L * tendrilState.GetCurvature(section);

            float I = tendrilState.GetSectionMomentInertia(section);


            float E = tendrilState.GetSectionStiffness(section);


            float F1 = (float) (m * 9.8 * ((float)Math.Cos(Theta) - 0.5 - (float)Math.Cos(2 * Theta) / 2)) / (3 * (float)Math.Pow(Theta, 2));
            float F2 = (E * I * Theta / L);
            float F3 = (float)((m * 9.8 * L) / Math.Pow(Theta, 2));
            float F4 = ((((float)Math.Cos(2 * Theta) - 2 * (float)Math.Cos(Theta) + 1) / Theta) + ((float)Math.Sin(2 * Theta) - (float)Math.Sin(Theta)));


            //for i = 1:1:3
            
            //    Falpha = (2 * cos(phi + (i - 1) * 2 * pi / 3)) / (3 * r)
            
            //    F(i) = F1 - Falpha * (F2 + (F3 * F4));
            //end
            


            for (int i = 0; i < 3; i++)
            {
                float Falpha = (2 * (float)Math.Cos(tendrilState.GetCurveAngle(section) + (i - 1) * 2 * Math.PI / 3)) / (3 * tendrilState.GetSpacerRadius(section));
                TensionLoads[i] = F1 - Falpha * (F2 + (F3 * F4));
            }
        }

        public void PrintBajoOuput()
        {
            if (!PrintEnabled) PrintEnabled = true;
        }

    }
}
