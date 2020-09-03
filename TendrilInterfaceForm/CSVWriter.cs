using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TendrilInterfaceForm
{
    
    class CSVWriter
    {
        private int Index { get; set; }
        private StreamWriter strmWriter;
        public bool strmActive;
        public bool strmRunning;

        public CSVWriter(Stream file)
        {
            strmWriter = new StreamWriter(file);
            Index = 0;
            strmActive = true;
            strmRunning = false;
        }
        
        public void CSV_WriteLine(String s)
        {
            strmWriter.WriteLine(s);
            strmWriter.Flush();
        }
        
        public void CloseCSV()
        {
            strmWriter.Close();
            strmActive = false;
        }

        public String CSV_PrepareLog(string[] cnts, string[] tens)
        {
            String s = TendrilUtils.GetTimesstamp(DateTime.Now) + ",";

            for (int i = 0; i < cnts.Length; i++)
            {
                if (i == cnts.Length - 1)
                {
                    s += cnts[i].Remove(cnts[i].LastIndexOf('\r'), 1);
                    s += ",";
                }
                else
                {
                    s += cnts[i];
                    s += ",";
                }
            }
            for (int i = 0; i < tens.Length; i++)
            {
                s += tens[i];
                if (i < tens.Length - 1) s += ",";
            }
            return s;
        }

    }
}
