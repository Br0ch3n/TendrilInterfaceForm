using System;
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
        const float dShaftB = 0.0063f;// 6.34 mm 0: 7.34 mm, 1: 7.06 mm, 2: 7.58 mm


        //Average curvatures and phi
        public double k_mm;
        public double k_bm;
        public double k_tm;
        public double phi_bm;
        public double phi_mm;
        public double phi_tm;

        //Intermediate curvatures
        double k_mmi;
        double k_bmi;
        double k_tmi;

        const double db = 0.006;    //radius of base
        const double dm = 0.003;    //radius of mid
        const double dt = 0.003;    //radius of tip

        //lengths of tendril sections [m]
        const double ten_bs = 0.917; // measured: 931 mm = 0.931 m
        const double ten_ms = 0.225;
        const double ten_ts = 0.2;

        const int nb = 12; // measured: 12
        const int nm = 4;
        const int nt = 7;

        //Maximum Curvatures
        double kb_max;
        double km_max;
        double kt_max;

        //length array for tendrils
        double[] base_s = new double[3];
        double[] mid_s = new double[3];
        double[] tip_s = new double[3];

        RunningAverage k_bmArray;
        RunningAverage k_mmArray;
        RunningAverage k_tmArray;
        RunningAverage[] motors;

        public JonesModel() // add initialization for variables passed from singleton like section lengths, etc.
        {
            kb_max = Math.PI / ten_bs;
            km_max = (2 * Math.PI) / ten_ms;
            kt_max = Math.PI / (2 * ten_ts);
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

        public void Update(float[] tendonlength, int firstMotor, int LastMotor)
        {


            //Calculate immediate curvature for each section (bryan Jones method)
            k_bmi = 2 * Math.Sqrt(Math.Pow(tendonlength[0], 2) + Math.Pow(tendonlength[1], 2) + Math.Pow(tendonlength[2], 2) - tendonlength[0] * tendonlength[1] - tendonlength[1] * tendonlength[2] - tendonlength[0] * tendonlength[2]) / (db * (tendonlength[0] + tendonlength[1] + tendonlength[2]));
            k_mmi = 2 * Math.Sqrt(Math.Pow(tendonlength[3], 2) + Math.Pow(tendonlength[4], 2) + Math.Pow(tendonlength[5], 2) - tendonlength[3] * tendonlength[4] - tendonlength[4] * tendonlength[5] - tendonlength[3] * tendonlength[5]) / (dm * (tendonlength[3] + tendonlength[4] + tendonlength[5]));
            k_tmi = 2 * Math.Sqrt(Math.Pow(tendonlength[6], 2) + Math.Pow(tendonlength[7], 2) + Math.Pow(tendonlength[8], 2) - tendonlength[6] * tendonlength[7] - tendonlength[7] * tendonlength[8] - tendonlength[6] * tendonlength[8]) / (dt * (tendonlength[6] + tendonlength[7] + tendonlength[8]));

            
            k_mmi = k_mmi * 0.1f; // need to verify that these are necessary
            k_tmi = k_tmi * 0.2f;

            //Calculate direction for each section
            phi_bm = Math.Atan2(Math.Sqrt(3) * (tendonlength[2] + tendonlength[1] - 2 * tendonlength[0]), (3 * (tendonlength[1] - tendonlength[2])));      //[deg]
            phi_mm = Math.Atan2(Math.Sqrt(3) * (tendonlength[5] + tendonlength[4] - 2 * tendonlength[3]), (3 * (tendonlength[4] - tendonlength[5])));      //[deg]
            phi_tm = Math.Atan2(Math.Sqrt(3) * (tendonlength[8] + tendonlength[7] - 2 * tendonlength[6]), (3 * (tendonlength[7] - tendonlength[8])));      //[deg]

            //Add curvature to running average
            k_bmArray.AddValue((float)k_bmi);
            k_mmArray.AddValue((float)k_mmi);
            k_tmArray.AddValue((float)k_tmi);

            //obtain running average
            k_bm = k_bmArray.avgF;
            k_mm = k_mmArray.avgF;
            k_tm = k_tmArray.avgF;

            //address curvature threshold and nan values for direction
            if (k_bm < 0.4 || k_bm == Double.NaN) { phi_bm = 0; }
            if (k_mm < 0.4 || k_mm == Double.NaN) { phi_mm = 0; }
            if (k_tm < 0.4 || k_tm == Double.NaN) { phi_tm = 0; }

            //assign minimum curvature for nan case
            if (k_bm ==  Double.NaN) { k_bm = 0.01; }
            if (k_mm == Double.NaN) { k_mm = 0.01; }
            if (k_tm == Double.NaN) { k_tm = 0.01; }

            //assess maximum curvature threshold
            if (k_bm > kb_max) { k_bm = kb_max; }
            if (k_mm > km_max) { k_mm = km_max; }
            if (k_tm > kt_max) { k_tm = kt_max; }

            Console.WriteLine("K's: " + k_bm.ToString() + ", " + k_mm.ToString() + ", " + k_tm.ToString());
            Console.WriteLine("Phi's: " + phi_bm.ToString() + ", " + phi_mm.ToString() + ", " + phi_tm.ToString());
            
        }


        public double[] CalculateTendonLengths(int firstMotor, int lastMotor)
        {
            double[] lengths;
            lengths = new double[9];

            ////determine tendril base section lengths
            lengths[0] = 2 * nb * Math.Sin(ten_bs * k_bm / (2 * nb)) * ((1 / k_bm) - (db * Math.Sin(phi_bm)));
            lengths[1] = 2 * nb * Math.Sin(ten_bs * k_bm / (2 * nb)) * ((1 / k_bm) + (db * Math.Sin((Math.PI / 3) + phi_bm)));
            lengths[2] = 2 * nb * Math.Sin(ten_bs * k_bm / (2 * nb)) * ((1 / k_bm) - (db * Math.Cos((Math.PI / 6) + phi_bm)));

            ////determine tendril mid section lengths
            lengths[3] = 2 * nm * Math.Sin(ten_ms * k_mm / (2 * nm)) * ((1 / k_mm) - (dm * Math.Sin((2 * Math.PI / 9) + phi_mm)));
            lengths[4] = 2 * nm * Math.Sin(ten_ms * k_mm / (2 * nm)) * ((1 / k_mm) + (dm * Math.Sin((5 * Math.PI / 9) + phi_mm)));
            lengths[5] = 2 * nm * Math.Sin(ten_ms * k_mm / (2 * nm)) * ((1 / k_mm) - (dm * Math.Cos((7 * Math.PI / 18) + phi_mm)));

            //determine tendril tip section lengths
            lengths[6] = 2 * nt * Math.Sin(ten_ts * k_tm / (2 * nt)) * ((1 / k_tm) - (dt * Math.Sin((4 * Math.PI / 9) + phi_tm)));
            lengths[7] = 2 * nt * Math.Sin(ten_ts * k_tm / (2 * nt)) * ((1 / k_tm) + (dt * Math.Sin((7 * Math.PI / 9) + phi_tm)));
            lengths[8] = 2 * nt * Math.Sin(ten_ts * k_tm / (2 * nt)) * ((1 / k_tm) - (dt * Math.Cos((11 * Math.PI / 18) + phi_tm)));

            return lengths;

        }

        // add accessors for the s, k, and phi variables as they should be stored here.



    }
}
