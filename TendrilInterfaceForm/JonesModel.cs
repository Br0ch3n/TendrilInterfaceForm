using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TendrilInterfaceForm
{
    class JonesModel
    {

        const float dShaft = 0.006f;
        const float dShaftB = 0.0063f;
        const float lengthPerRot = (float)Math.PI * dShaft;
        const float lengthPerRotB = (float)Math.PI * dShaftB;
        const int countPerRot = 720;
        const int countPerRotB = 5256;

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
        const double ten_bs = 0.917;
        const double ten_ms = 0.225;
        const double ten_ts = 0.2;

        const int nb = 12;
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

        public JonesModel()
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

        public void Update(float[] enc)
        {
            
 
            //convert motor counts to lengths for tendril
            for (int i = 0; i < 3; i++)
            {
                base_s[i] = enc[i]/(-(countPerRotB / lengthPerRotB)) + ten_bs;
                mid_s[i] = enc[i + 3] / (-(countPerRot / lengthPerRot)) + ten_ms;
                tip_s[i] = enc[i + 6] / (-(countPerRot / lengthPerRot)) + ten_ts;
            }

            //Calculate immediate curvature for each section (bryan Jones method)
            k_bmi = 2 * Math.Sqrt(Math.Pow(base_s[0], 2) + Math.Pow(base_s[1], 2) + Math.Pow(base_s[2], 2) - base_s[0] * base_s[1] - base_s[1] * base_s[2] - base_s[0] * base_s[2]) / (db * (base_s[0] + base_s[1] + base_s[2]));
            k_mmi = 2 * Math.Sqrt(Math.Pow(mid_s[0], 2) + Math.Pow(mid_s[1], 2) + Math.Pow(mid_s[2], 2) - mid_s[0] * mid_s[1] - mid_s[1] * mid_s[2] - mid_s[0] * mid_s[2]) / (dm * (mid_s[0] + mid_s[1] + mid_s[2]));
            k_tmi = 2 * Math.Sqrt(Math.Pow(tip_s[0], 2) + Math.Pow(tip_s[1], 2) + Math.Pow(tip_s[2], 2) - tip_s[0] * tip_s[1] - tip_s[1] * tip_s[2] - tip_s[0] * tip_s[2]) / (dt * (tip_s[0] + tip_s[1] + tip_s[2]));

            k_mmi = k_mmi * 0.1f;
            k_tmi = k_tmi * 0.2f;

            //Calculate direction for each section
            phi_bm = Math.Atan2(Math.Sqrt(3) * (base_s[2] + base_s[1] - 2 * base_s[0]), (3 * (base_s[1] - base_s[2]))); //[deg]
            phi_mm = Math.Atan2(Math.Sqrt(3) * (mid_s[2] + mid_s[1] - 2 * mid_s[0]), (3 * (mid_s[1] - mid_s[2])));      //[deg]
            phi_tm = Math.Atan2(Math.Sqrt(3) * (tip_s[2] + tip_s[1] - 2 * tip_s[0]), (3 * (tip_s[1] - tip_s[2])));      //[deg]

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
            ////determine tendril base section lengths
            //base_s[0] = 2 * nb * Math.Sin(ten_bs * k_bm / (2 * nb)) * ((1 / k_bm) - (db * Math.Sin(phi_bm)));
            //base_s[1] = 2 * nb * Math.Sin(ten_bs * k_bm / (2 * nb)) * ((1 / k_bm) + (db * Math.Sin((Math.PI / 3) + phi_bm)));
            //base_s[2] = 2 * nb * Math.Sin(ten_bs * k_bm / (2 * nb)) * ((1 / k_bm) - (db * Math.Cos((Math.PI / 6) + phi_bm)));

            ////determine tendril mid section lengths
            //mid_s[0] = 2 * nm * Math.Sin(ten_ms * k_mm / (2 * nm)) * ((1 / k_mm) - (dm * Math.Sin((2 * Math.PI / 9) + phi_mm)));
            //mid_s[1] = 2 * nm * Math.Sin(ten_ms * k_mm / (2 * nm)) * ((1 / k_mm) + (dm * Math.Sin((5 * Math.PI / 9) + phi_mm)));
            //mid_s[2] = 2 * nm * Math.Sin(ten_ms * k_mm / (2 * nm)) * ((1 / k_mm) - (dm * Math.Cos((7 * Math.PI / 18) + phi_mm)));

            ////determine tendril tip section lengths
            //tip_s[0] = 2 * nt * Math.Sin(ten_ts * k_tm / (2 * nt)) * ((1 / k_tm) - (dt * Math.Sin((4 * Math.PI / 9) + phi_tm)));
            //tip_s[1] = 2 * nt * Math.Sin(ten_ts * k_tm / (2 * nt)) * ((1 / k_tm) + (dt * Math.Sin((7 * Math.PI / 9) + phi_tm)));
            //tip_s[2] = 2 * nt * Math.Sin(ten_ts * k_tm / (2 * nt)) * ((1 / k_tm) - (dt * Math.Cos((11 * Math.PI / 18) + phi_tm)));
        }


    }
}
