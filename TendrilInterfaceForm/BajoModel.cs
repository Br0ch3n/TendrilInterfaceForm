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
            
        }

        public void ContactDetection(int section, Matrix3 J)
        {
            TendrilStateSingleton tendrilState = TendrilStateSingleton.Instance;

            timeCurrent = DateTime.Now;

            timeElapsed = timeCurrent.Subtract(timeLast);

            timeLast = timeCurrent;

            CalculateTensionLoads(section);

            float[] dTension = tendrilState.GetDeltaTension();

            Vector3 Tao = new Vector3(dTension[0] / (float)timeElapsed.TotalSeconds, dTension[1] / (float)timeElapsed.TotalSeconds, dTension[2] / (float)timeElapsed.TotalSeconds);
            
            Jacobian = J.Transpose();

            Vector3 result = (Jacobian * Tao - TensionLoads);
           

            double Theta = Math.Sqrt(Math.Pow(result.X,2) + Math.Pow(result.Y, 2) + Math.Pow(result.Z, 2));

            if (Theta > ContactThreshold)
            {
                Console.WriteLine("Delta Tension: " + dTension.ToString());
                Console.WriteLine("Tao: " + Tao.ToString());
                Console.WriteLine("CONTACT! Theta = " + Theta.ToString());
            } else
            {
                //Console.WriteLine("No contact here..." + Theta.ToString());
            }
        }

        private void CalculateTensionLoads(int section)
        {
            TendrilStateSingleton tendrilState = TendrilStateSingleton.Instance;

            float m = tendrilState.GetSectionMass(section);
            float L = tendrilState.GetSectionLength(section);

            float Theta = tendrilState.GetSectionLength(section) * tendrilState.GetCurvature(section);

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



    }
}
