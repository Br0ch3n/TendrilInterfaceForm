﻿using System;
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
        private float deltaOutput, OutputLast, Output;
        private bool PrintEnabled;
        private Vector3 Result;


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
            Output = new float();
            OutputLast = new float();
            deltaOutput = new float();
            Result = new Vector3();

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

            Result = (Tao - TensionLoads);

            Vector3 J_Tao_F = Jacobian * Result;

            OutputLast = Output;
            Output = (float)Math.Sqrt(Math.Pow(Result.X,2) + Math.Pow(Result.Y, 2) + Math.Pow(Result.Z, 2));
            deltaOutput = Output - deltaOutput;

            if (PrintEnabled)
            {
                Console.WriteLine("K: " + tendrilState.GetCurvature(section).ToString() + ", Phi : " + tendrilState.GetCurveAngle(section).ToString());
                Console.WriteLine("Encoders: " + tendrilState.GetEncoders()[0].ToString() + ", " + tendrilState.GetEncoders()[1].ToString() + ", " + tendrilState.GetEncoders()[2].ToString());
                Console.WriteLine("Tao: " + Tao[0].ToString() + ", " + Tao[1].ToString() + ", " + Tao[2].ToString());
                Console.WriteLine("F: " + TensionLoads[0].ToString() + ", " + TensionLoads[1].ToString() + ", " + TensionLoads[2].ToString());
                Console.WriteLine("Tao-F: " + Result[0].ToString() + ", " + Result[1].ToString() + ", " + Result[2].ToString());
                Console.WriteLine("Jacobian * (Tao-F): " + J_Tao_F[0].ToString() + ", " + J_Tao_F[1].ToString() + ", " + J_Tao_F[2].ToString());
                Console.WriteLine("Delta Output: " + deltaOutput.ToString());
                if (Output > ContactThreshold) Console.WriteLine("CONTACT! Theta = " + Output.ToString());
                else Console.WriteLine("No contact... Theta = " + Output.ToString());
                PrintEnabled = false;
            }
        }

        private void CalculateTensionLoads(int section)
        {
            TendrilStateSingleton tendrilState = TendrilStateSingleton.Instance;

            float Falpha = 0.0f;

            float m = tendrilState.GetSectionMass(section);
            float L = tendrilState.GetSectionLength(section);

            float Theta = L * tendrilState.GetCurvature(section);

            float I = tendrilState.GetSectionMomentInertia(section);


            float E = tendrilState.GetSectionStiffness(section);


            float F1 = (float) (m * 9.8 * ((float)Math.Cos(Theta) - 0.5 - (float)Math.Cos(2 * Theta) / 2)) / (3 * (float)Math.Pow(Theta, 2));
            float F2 = (E * I * Theta / L);
            float F3 = (float)((m * 9.8 * L) / Math.Pow(Theta, 2));
            float F4 = ((((float)Math.Cos(2 * Theta) - 2 * (float)Math.Cos(Theta) + 1) / Theta) + ((float)Math.Sin(2 * Theta) - (float)Math.Sin(Theta)));

            if (PrintEnabled)
            {
                //Console.WriteLine("F1 = " + F1.ToString() + ", F2 = " + F2.ToString());
                //Console.WriteLine("F3 = " + F3.ToString() + ", F4 = " + F4.ToString());
            }

                for (int i = 0; i < 3; i++)
            {
                
                Falpha = (2 * (float)Math.Cos(tendrilState.GetCurveAngle(section) + i * 2 * Math.PI / 3)) / (3 * tendrilState.GetSpacerRadius(section));

                if (PrintEnabled)
                {
                    Console.Write("Falpha = " + Falpha.ToString() + ", ");
                }

                TensionLoads[i] = F1 - Falpha * (F2 + (F3 * F4));
            }
            if (PrintEnabled) Console.WriteLine();
        }

        public void PrintBajoOuput()
        {
            if (!PrintEnabled) PrintEnabled = true;
        }

    }
}
