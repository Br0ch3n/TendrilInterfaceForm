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

        private float ContactThreshold;
        private Matrix3 Jacobian;
        private Vector3 TensionLoads;


        //private TendrilStateSingleton tendrilState;

        // initialization

        public BajoModel()
        {
            ContactThreshold = 10;
            TensionLoads = new Vector3();
            Jacobian = new Matrix3();
            
        }

        public void ContactDetection(int section, Matrix3 J)
        {
            TendrilStateSingleton tendrilState = TendrilStateSingleton.Instance;

            CalculateTensionLoads(section);

            float[] dTension = tendrilState.GetDeltaTension();
            Vector3 Tao = new Vector3(dTension[0], dTension[1], dTension[2]);
            
            Jacobian = J.Transpose();

            Vector3 result = (Jacobian * Tao - TensionLoads);
           

            double Theta = Math.Sqrt(Math.Pow(result.X,2) + Math.Pow(result.Y, 2) + Math.Pow(result.Z, 2));

            if (Theta > ContactThreshold)
            {
                Console.WriteLine("CONTACT! Theta = " + Theta.ToString());
            } else
            {
                Console.WriteLine("No contact here..." + Theta.ToString());
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

            for (int i = 0; i < 3; i++)
            {
                TensionLoads[i] = (float)((m * 9.8f * (Math.Cos(Theta) - 0.5f - Math.Cos(2 * Theta) / 2)) / (3 * Math.Pow(Theta, 2)) - ((2 * Math.Cos(i * 2 * Math.PI / 3)) / (3 * L))
                                  * (E * I * Theta / L) + (((m * 9.8f * L) / Math.Pow(Theta, 2)) * ((Math.Cos(2 * Theta) - 2 * Math.Cos(Theta) + 1) / Theta) + (Math.Sin(2 * Theta) - Math.Sin(Theta))));
            }
        }



    }
}
