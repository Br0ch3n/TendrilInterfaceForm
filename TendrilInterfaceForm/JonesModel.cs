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
        const float dShaftB = 0.00634f;//Diameter:: shaft: 6.34 mm 0: 7.34 mm, 1: 7.06 mm, 2: 7.58 mm <- spooled tendon diameters


        //Average curvatures and phi
        public double[] k_avg;
        //public double k_bm;
        //public double k_tm;
        public double[] phi_avg;
        //public double phi_mm;
        //public double phi_tm;

        //Intermediate curvatures
        double[] k_avg_i;
        //double k_bmi;
        //double k_tmi;

        //const double db = 0.006;    //radius of base
        //const double dm = 0.003;    //radius of mid
        //const double dt = 0.003;    //radius of tip

        //lengths of tendril sections [m]
        const double ten_bs = 0.917; // measured: 931 mm = 0.931 m
        const double ten_ms = 0.225;
        const double ten_ts = 0.2;

        const int nb = 12; // measured: 12
        const int nm = 4;
        const int nt = 7;

        //Maximum Curvatures
        double[] k_max;
        //double km_max;
        //double kt_max;

        //length array for tendrils
        double[] base_s = new double[3];
        double[] mid_s = new double[3];
        double[] tip_s = new double[3];

        RunningAverage k_bmArray;
        RunningAverage k_mmArray;
        RunningAverage k_tmArray;
        RunningAverage[] motors;

        private TendrilStateSingleton TendrilState;

        public JonesModel()
        {
            TendrilState = TendrilStateSingleton.getInstance();

            k_max = new double[3] { Math.PI / ten_bs, (2 * Math.PI) / ten_ms, Math.PI / (2 * ten_ts) };
            
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

            if (section == TendrilUtils.MID_SECTION) sectionalOffset = 3;
            else if (section == TendrilUtils.TIP_SECTION) sectionalOffset = 6;


            //Calculate immediate curvature for each section (bryan Jones method)
            k_avg_i[section] = 2 * Math.Sqrt(Math.Pow(tendonlength[0 + sectionalOffset], 2) + Math.Pow(tendonlength[1 + sectionalOffset], 2) + Math.Pow(tendonlength[2 + sectionalOffset], 2)
                                - tendonlength[0 + sectionalOffset] * tendonlength[1 + sectionalOffset] - tendonlength[1 + sectionalOffset] * tendonlength[2 + sectionalOffset] - tendonlength[0 + sectionalOffset]
                                * tendonlength[2 + sectionalOffset]) / (db * (tendonlength[0 + sectionalOffset] + tendonlength[1 + sectionalOffset] + tendonlength[2 + sectionalOffset]));


            //if (lastMotor > 2) k_mmi = 2 * Math.Sqrt(Math.Pow(tendonlength[3], 2) + Math.Pow(tendonlength[4], 2) + Math.Pow(tendonlength[5], 2) - tendonlength[3] * tendonlength[4] - tendonlength[4] * tendonlength[5] - tendonlength[3] * tendonlength[5]) / (dm * (tendonlength[3] + tendonlength[4] + tendonlength[5]));
            
            //if (lastMotor > 5) k_tmi = 2 * Math.Sqrt(Math.Pow(tendonlength[6], 2) + Math.Pow(tendonlength[7], 2) + Math.Pow(tendonlength[8], 2) - tendonlength[6] * tendonlength[7] - tendonlength[7] * tendonlength[8] - tendonlength[6] * tendonlength[8]) / (dt * (tendonlength[6] + tendonlength[7] + tendonlength[8]));


            //if (lastMotor > 2) k_mmi = k_mmi * 0.1f; // need to verify that these are necessary
            //if (lastMotor > 5) k_tmi = k_tmi * 0.2f;

            //Calculate direction for each section
            phi_avg[section] = Math.Atan2(Math.Sqrt(3)
                                * (tendonlength[2 + sectionalOffset] + tendonlength[1 + sectionalOffset] - 2 * tendonlength[0 + sectionalOffset]), 
                                (3 * (tendonlength[1 + sectionalOffset] - tendonlength[2 + sectionalOffset])));      //[deg]
            //if (lastMotor > 2) phi_mm = Math.Atan2(Math.Sqrt(3) * (tendonlength[5] + tendonlength[4] - 2 * tendonlength[3]), (3 * (tendonlength[4] - tendonlength[5])));      //[deg]
            //if (lastMotor > 5) phi_tm = Math.Atan2(Math.Sqrt(3) * (tendonlength[8] + tendonlength[7] - 2 * tendonlength[6]), (3 * (tendonlength[7] - tendonlength[8])));      //[deg]

            //Add curvature to running average
            k_bmArray.AddValue((float)k_avg_i[section]);

            //obtain running average
            k_avg[section] = k_bmArray.avgF;
            //if (lastMotor > 2) k_mm = k_mmArray.avgF;
            //if (lastMotor > 5) k_tm = k_tmArray.avgF;

            //address curvature threshold and nan values for direction
            if (k_avg[section] < 0.4 || k_avg[section] == Double.NaN) { phi_avg[section] = 0; }
            //if (lastMotor > 2 && k_mm < 0.4 || k_mm == Double.NaN) { phi_mm = 0; }
            //if (lastMotor > 5 && k_tm < 0.4 || k_tm == Double.NaN) { phi_tm = 0; }

            //assign minimum curvature for nan case
            if (k_bm ==  Double.NaN) { k_bm = 0.01; }
            if (lastMotor > 2 && k_mm == Double.NaN) { k_mm = 0.01; }
            if (lastMotor > 5 && k_tm == Double.NaN) { k_tm = 0.01; }

            //assess maximum curvature threshold
            if (k_bm > kb_max) { k_bm = kb_max; }
            if (lastMotor > 2 && k_mm > km_max) { k_mm = km_max; }
            if (lastMotor > 5 && k_tm > kt_max) { k_tm = kt_max; }

            Console.WriteLine("K's: " + k_bm.ToString() + ", " + k_mm.ToString() + ", " + k_tm.ToString());
            Console.WriteLine("Phi's: " + phi_bm.ToString() + ", " + phi_mm.ToString() + ", " + phi_tm.ToString());
            
        }


        public double[] CalculateTendonLengths(int section)
        {
            double[] lengths;
            lengths = new double[9];

            ////determine tendril base section lengths
            lengths[0] = 2 * nb * Math.Sin(ten_bs * k_bm / (2 * nb)) * ((1 / k_bm) - (db * Math.Sin(phi_bm)));
            lengths[1] = 2 * nb * Math.Sin(ten_bs * k_bm / (2 * nb)) * ((1 / k_bm) + (db * Math.Sin((Math.PI / 3) + phi_bm)));
            lengths[2] = 2 * nb * Math.Sin(ten_bs * k_bm / (2 * nb)) * ((1 / k_bm) - (db * Math.Cos((Math.PI / 6) + phi_bm)));

            ////determine tendril mid section lengths
            if (lastMotor > 2)
            {
                lengths[3] = 2 * nm * Math.Sin(ten_ms * k_mm / (2 * nm)) * ((1 / k_mm) - (dm * Math.Sin((2 * Math.PI / 9) + phi_mm)));
                lengths[4] = 2 * nm * Math.Sin(ten_ms * k_mm / (2 * nm)) * ((1 / k_mm) + (dm * Math.Sin((5 * Math.PI / 9) + phi_mm)));
                lengths[5] = 2 * nm * Math.Sin(ten_ms * k_mm / (2 * nm)) * ((1 / k_mm) - (dm * Math.Cos((7 * Math.PI / 18) + phi_mm)));
            }

            //determine tendril tip section lengths
            if (lastMotor > 5)
            {
                lengths[6] = 2 * nt * Math.Sin(ten_ts * k_tm / (2 * nt)) * ((1 / k_tm) - (dt * Math.Sin((4 * Math.PI / 9) + phi_tm)));
                lengths[7] = 2 * nt * Math.Sin(ten_ts * k_tm / (2 * nt)) * ((1 / k_tm) + (dt * Math.Sin((7 * Math.PI / 9) + phi_tm)));
                lengths[8] = 2 * nt * Math.Sin(ten_ts * k_tm / (2 * nt)) * ((1 / k_tm) - (dt * Math.Cos((11 * Math.PI / 18) + phi_tm)));
            }
                

            return lengths;

        }

        // add accessors for the s, k, and phi variables as they should be stored here.
        public double getCurvature(int section)
        {
            if (section == TendrilUtils.BASE_SECTION) return k_bm;
            else if (section == TendrilUtils.MID_SECTION) return k_mm;
            else return k_tm;
        }



        public double getPhi(int section)
        {
            if (section == TendrilUtils.BASE_SECTION) return phi_bm;
            else if (section == TendrilUtils.MID_SECTION) return phi_mm;
            else return phi_tm;
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

        public static Matrix3 CreateSingleSectionJacobian(double s, double k, double phi)
        {
            Matrix3 Transform = new Matrix3(
                    new Vector3(0, (float)(-s * db * Math.Cos(phi)), (float)(s * db * Math.Sin(phi))),
                    new Vector3(0, (float)(Math.Cos(phi) * Math.Cos(s * k)), (float)(-Math.Cos(phi) * Math.Sin(s * k))),
                    new Vector3(0, (float)Math.Sin(s * k), (float)Math.Cos(s * k))
                    );
            //Console.WriteLine(Transform.ToString());
            return Transform;
        }

    }
}
