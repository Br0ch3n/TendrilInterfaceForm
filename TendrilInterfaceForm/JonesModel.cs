﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenGL;

namespace TendrilInterfaceForm
{
    public class JonesModel
    {

        const float dShaft = 0.006f;
        const float dShaftB = 0.00634f;//Diameter:: shaft: 6.34 mm 0: 7.34 mm, 1: 7.06 mm, 2: 7.58 mm <- spooled tendon diameters


        //Average curvatures and phi
        public double[] k_avg;
        public double[] phi_avg;

        //Intermediate curvatures
        double[] k_avg_i;

       

        //Maximum Curvatures
        double[] k_max;

        RunningAverage k_bmArray;
        RunningAverage k_mmArray;
        RunningAverage k_tmArray;
        RunningAverage[] motors;

        //private TendrilStateSingleton tendrilState;

        public JonesModel()
        {
            TendrilStateSingleton tendrilState = TendrilStateSingleton.Instance;

            k_max = new double[3] { Math.PI / tendrilState.GetSectionLength(TendrilUtils.BASE_SECTION), 
                                    (2 * Math.PI) /tendrilState.GetSectionLength(TendrilUtils.MID_SECTION), 
                                    Math.PI / (2 * tendrilState.GetSectionLength(TendrilUtils.TIP_SECTION)) };

            k_avg_i = new double[3];


            //km_max = (2 * Math.PI) / ten_ms;
            //kt_max = Math.PI / (2 * ten_ts);
            k_bmArray = new RunningAverage(25);
            k_mmArray = new RunningAverage(25);
            k_tmArray = new RunningAverage(25);

            motors = new RunningAverage[9];
            for (int ndx = 0; ndx < 9; ndx++)
            {
                motors[ndx] = new RunningAverage(25);
                motors[ndx].Clear();
            }
        }

        public void Update(float[] tendonlength, int section)
        {
            int sectionalOffset = 0;

            TendrilStateSingleton tendrilState = TendrilStateSingleton.Instance;

            if (section == TendrilUtils.MID_SECTION) sectionalOffset = 3;
            else if (section == TendrilUtils.TIP_SECTION) sectionalOffset = 6;


            //Calculate immediate curvature for each section (bryan Jones method)
            k_avg_i[section] = 2 * Math.Sqrt(Math.Pow(tendonlength[0 + sectionalOffset], 2) + Math.Pow(tendonlength[1 + sectionalOffset], 2) + Math.Pow(tendonlength[2 + sectionalOffset], 2)
                                - tendonlength[0 + sectionalOffset] * tendonlength[1 + sectionalOffset] - tendonlength[1 + sectionalOffset] * tendonlength[2 + sectionalOffset] - tendonlength[0 + sectionalOffset]
                                * tendonlength[2 + sectionalOffset]) / (tendrilState.GetSpacerRadius(section) * (tendonlength[0 + sectionalOffset] + tendonlength[1 + sectionalOffset] + tendonlength[2 + sectionalOffset]));


            //if (lastMotor > 2) k_mmi = k_mmi * 0.1f; // need to verify that these are necessary
            //if (lastMotor > 5) k_tmi = k_tmi * 0.2f;

            //Calculate direction for each section
            phi_avg[section] = Math.Atan2(Math.Sqrt(3) * (tendonlength[2 + sectionalOffset] + tendonlength[1 + sectionalOffset] - 2 * tendonlength[0 + sectionalOffset]), 
                                (3 * (tendonlength[1 + sectionalOffset] - tendonlength[2 + sectionalOffset])));      //[deg]

            //Add curvature to running average
            k_bmArray.AddValue((float)k_avg_i[section]);

            //obtain running average
            k_avg[section] = k_bmArray.avgF;

            //address curvature threshold and nan values for direction
            if (k_avg[section] < 0.4 || k_avg[section] == Double.NaN) { phi_avg[section] = 0; }

            //assign minimum curvature for nan case
            if (k_avg[section] ==  Double.NaN) { k_avg[section] = 0.01; }

            //assess maximum curvature threshold
            if (k_avg[section] > k_max[section]) { k_avg[section] = k_max[section]; }

            Console.WriteLine("K: " + k_avg[section].ToString() + ", Phi: " + phi_avg[section].ToString());
            //Console.WriteLine("Phi's: " + phi_bm.ToString() + ", " + phi_mm.ToString() + ", " + phi_tm.ToString());
            
        }


        public double[] CalculateTendonLengths(int section)
        {
            TendrilStateSingleton TendrilState = TendrilStateSingleton.Instance;
            int sectionalOffset = 0;

            if (section == TendrilUtils.MID_SECTION) sectionalOffset = 3;
            else if (section == TendrilUtils.TIP_SECTION) sectionalOffset = 6;
            double[] lengths;
            float s = TendrilState.GetSectionLength(section);
            float r = TendrilState.GetSpacerRadius(section);
            int n = TendrilState.GetSpacerCount(section);

            
            lengths = new double[9];

            //determine tendril base section lengths
            // will only work on base section (need to add sectionalOffset)
            lengths[0] = 2 * n * Math.Sin(s * k_avg[section] / (2 * n)) * ((1 / k_avg[section]) - (r * Math.Sin(phi_avg[section])));
            lengths[1] = 2 * n * Math.Sin(s * k_avg[section] / (2 * n)) * ((1 / k_avg[section]) + (r * Math.Sin((Math.PI / 3) + phi_avg[section])));
            lengths[2] = 2 * n * Math.Sin(s * k_avg[section] / (2 * n)) * ((1 / k_avg[section]) - (r * Math.Cos((Math.PI / 6) + phi_avg[section])));


            return lengths;

        }

        // add accessors for the s, k, and phi variables as they should be stored here.
        public double getCurvature(int section)
        {
            return k_avg[section];
        }



        public double getPhi(int section)
        {
            return phi_avg[section];
        }


        public static Matrix4 CreateMatrixJones(double s, double k, double phi)
        {
            Matrix4 Transform = new Matrix4(
                    new Vector4((float)Math.Cos(phi), (float)(-Math.Sin(phi) * Math.Cos(s * k)), (float)(Math.Sin(phi) * Math.Sin(s * k)), (float)((1 / k) * (Math.Sin(phi) * (1 - Math.Cos(s * k))))),
                    new Vector4((float)Math.Sin(phi), (float)(Math.Cos(phi) * Math.Cos(s * k)), (float)(-Math.Cos(phi) * Math.Sin(s * k)), (float)(-(1 / k) * (Math.Cos(phi) * (1 - Math.Cos(s * k))))),
                    new Vector4(0, (float)Math.Sin(s * k), (float)Math.Cos(s * k), (float)((1 / k) * Math.Sin(s * k))),
                    new Vector4(0, 0, 0, 1)
                    );
            //Console.WriteLine(Transform.ToString());
            return Transform;
        }

        public Matrix3 CreateSingleSectionJacobian(int section)
        {
            TendrilStateSingleton tendrilState = TendrilStateSingleton.Instance;
            float s = tendrilState.GetSectionLength(section);
            float r = tendrilState.GetSpacerRadius(section);

            Matrix3 Transform = new Matrix3(
                    new Vector3(0, (float)(-s * r * Math.Cos(phi_avg[section])), (float)(s * r * Math.Sin(phi_avg[section]))),
                    new Vector3(0, (float)(Math.Cos(phi_avg[section]) * Math.Cos(s * k_avg[section])), (float)(-Math.Cos(phi_avg[section]) * Math.Sin(s * k_avg[section]))),
                    new Vector3(0, (float)Math.Sin(s * k_avg[section]), (float)Math.Cos(s * k_avg[section]))
                    );
            //Console.WriteLine(Transform.ToString());
            return Transform;
        }

    }
}
