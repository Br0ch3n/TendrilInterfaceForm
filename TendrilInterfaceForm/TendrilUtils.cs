using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenGL;

namespace TendrilInterfaceForm
{
    public static class TendrilUtils
    {
        public static float[] tenValues = new float[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        

        public static void SetProgressBars(TendrilInterface t, String[] s)
        {
            t.tenProgressBar0.Value = (Int32.Parse(s[0]) > 0) ? ((Int32.Parse(s[0]) <= t.tenProgressBar0.Maximum) ? Int32.Parse(s[0]) : t.tenProgressBar0.Maximum) : 0;
            t.tenProgressBar1.Value = (Int32.Parse(s[1]) > 0) ? ((Int32.Parse(s[1]) <= t.tenProgressBar1.Maximum) ? Int32.Parse(s[1]) : t.tenProgressBar1.Maximum) : 0;
            t.tenProgressBar2.Value = (Int32.Parse(s[2]) > 0) ? ((Int32.Parse(s[2]) <= t.tenProgressBar2.Maximum) ? Int32.Parse(s[2]) : t.tenProgressBar2.Maximum) : 0;
            t.tenProgressBar3.Value = (Int32.Parse(s[3]) > 0) ? ((Int32.Parse(s[3]) <= t.tenProgressBar3.Maximum) ? Int32.Parse(s[3]) : t.tenProgressBar3.Maximum) : 0;
            t.tenProgressBar4.Value = (Int32.Parse(s[4]) > 0) ? ((Int32.Parse(s[4]) <= t.tenProgressBar4.Maximum) ? Int32.Parse(s[4]) : t.tenProgressBar4.Maximum) : 0;
            t.tenProgressBar5.Value = (Int32.Parse(s[5]) > 0) ? ((Int32.Parse(s[5]) <= t.tenProgressBar5.Maximum) ? Int32.Parse(s[5]) : t.tenProgressBar5.Maximum) : 0;
            t.tenProgressBar6.Value = (Int32.Parse(s[6]) > 0) ? ((Int32.Parse(s[6]) <= t.tenProgressBar6.Maximum) ? Int32.Parse(s[6]) : t.tenProgressBar6.Maximum) : 0;
            t.tenProgressBar7.Value = (Int32.Parse(s[7]) > 0) ? ((Int32.Parse(s[7]) <= t.tenProgressBar7.Maximum) ? Int32.Parse(s[7]) : t.tenProgressBar7.Maximum) : 0;
            t.tenProgressBar8.Value = (Int32.Parse(s[8]) > 0) ? ((Int32.Parse(s[8]) <= t.tenProgressBar8.Maximum) ? Int32.Parse(s[8]) : t.tenProgressBar8.Maximum) : 0;
            tenValues = new float[]
            {
                t.tenProgressBar0.Value, t.tenProgressBar1.Value, t.tenProgressBar2.Value,
                t.tenProgressBar3.Value, t.tenProgressBar4.Value, t.tenProgressBar5.Value,
                t.tenProgressBar6.Value, t.tenProgressBar7.Value, t.tenProgressBar8.Value
            };
        }

        public static String GetTimesstamp(this DateTime value)
        {
            return value.ToString("yyyy:MM:dd:HH:mm:ss:fff");
        }

        public static void SetValueLabels(TendrilInterface t, String[] s)
        {
            t.labValueMotor0.Text = s[0]; t.labValueMotor1.Text = s[1]; t.labValueMotor2.Text = s[2];
            t.labValueMotor3.Text = s[3]; t.labValueMotor4.Text = s[4]; t.labValueMotor5.Text = s[5];
            t.labValueMotor6.Text = s[6]; t.labValueMotor7.Text = s[7]; t.labValueMotor8.Text = s[8];
        }

        public static String AddText(String s1, String s2)
        {
            int temp = Int32.Parse(s1) + Int32.Parse(s2);
            return temp.ToString();
        }

        public static String SubtractText(String s1, String s2)
        {
            int temp = Int32.Parse(s1) - Int32.Parse(s2);
            return temp.ToString();
        }

        public static String[] EvenOutSection(String section, String[] cnts, String[] ten, SerialPort serialPort)
        {
            int DIFF_THRESH = 30;
            int[] tensions = new int[ten.Length];
            int[] counts = new int[cnts.Length];
            int average = 0; int modMax = 12;
            int maxDiff = Int32.MaxValue;
            int[] tenSection = new int[3];
            int[] cntSection = new int[3];
            int[] cntMods = new int[3];
            float temp;
            String[] finished = new String[1];
            finished[0] = "Done";
            for (int i = 0; i < ten.Length; i++)
            {
                tensions[i] = Int32.Parse(ten[i]);
                counts[i] = Int32.Parse(cnts[i]);
            }
            if (section == "Middle Section")
            {
                for (int i = 3; i < 6; i++)
                {
                    tenSection[i - 3] = tensions[i];
                    cntSection[i - 3] = counts[i];
                }
                average = (int)tenSection.Average();
                maxDiff = tenSection.Max() - tenSection.Min();
                System.Console.WriteLine("maxDiff = " + maxDiff.ToString());
                if (maxDiff <= DIFF_THRESH) return finished;
                for (int i = 0; i < cntSection.Length; i++)
                {
                    temp = (average - tenSection[i]);
                    if (temp > 0) cntMods[i] = (int)(modMax * (temp / maxDiff));
                    else cntMods[i] = 4 * (int)(modMax * (temp / maxDiff));
                    counts[i + 3] = counts[i + 3] + cntMods[i];
                    System.Console.Write(cntMods[i].ToString() + " ");
                    cnts[i + 3] = counts[i + 3].ToString();
                }
                System.Console.WriteLine("");
            }
            else if (section == "Tip Section")
            {
                for (int i = 6; i < 9; i++)
                {
                    tenSection[i - 6] = tensions[i];
                    cntSection[i - 6] = counts[i];
                }
                average = (int)tenSection.Average();
                maxDiff = tenSection.Max() - tenSection.Min();
                System.Console.WriteLine("maxDiff = " + maxDiff.ToString());
                if (maxDiff <= DIFF_THRESH) return finished;
                for (int i = 0; i < cntSection.Length; i++)
                {
                    temp = (average - tenSection[i]);
                    if (temp < 0) cntMods[i] = (int)(modMax * (temp / maxDiff));
                    else cntMods[i] = 4 * (int)(modMax * (temp / maxDiff));
                    counts[i + 6] = counts[i + 6] + cntMods[i];
                    System.Console.Write(cntMods[i].ToString() + " ");
                    cnts[i + 6] = counts[i + 6].ToString();
                }
                System.Console.WriteLine("");

            }
            else
            {
                System.Console.WriteLine("Error with section choice.");
            }
            serialPort.WriteLine("Read," + cnts);
            return cnts;

        }
        public static Matrix4 CreateMatrixJones(double s, double k, double phi)
        {
            Matrix4 Transform = new Matrix4(
                    new Vector4((float)Math.Cos(phi), (float)(-Math.Sin(phi) * Math.Cos(s * k)), (float)(Math.Sin(phi) * Math.Sin(s * k)), (float)((1 / k) * (Math.Sin(phi) * (1 - Math.Cos(s * k)))) ),
                    new Vector4((float)Math.Sin(phi), (float)(Math.Cos(phi) * Math.Cos(s * k)), (float)(-Math.Cos(phi) * Math.Sin(s * k)), (float)(-(1 / k) * (Math.Cos(phi) * (1 - Math.Cos(s * k))))),
                    new Vector4(0, (float)Math.Sin(s * k), (float)Math.Cos(s * k), (float)((1 / k) * Math.Sin(s * k))),
                    new Vector4( 0, 0, 0, 1)
                );
            //Console.WriteLine(Transform.ToString());
            return Transform;
        }

    }
}
