using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TendrilInterfaceForm
{
    public class CSVReader
    {
        private int Index { get; set; }
        private StreamReader strmReader;        
        private String[] scriptRows;

        public CSVReader(Stream file)
        { 
                strmReader = new StreamReader(file);
                scriptRows = strmReader.ReadToEnd().Split('\n');
                Index = -1;
            
        }
        public String CSV_ReadPrevLine()
        {
            if (Index > 0)
            {
                String currLine = scriptRows[--Index];
                return currLine;
            }
            else return "File opened.";

        }
        public String CSV_ReadNextLine()
        {
            if (Index < (scriptRows.Length - 1))
            {
                String currLine = scriptRows[++Index];
                if (currLine == "") return "End of file.";
                return currLine;
            }
            else
            {
                return "End of file.";
            }
                
            
        }
        public void CloseCSV()
        {
            strmReader.Close();
        }
        public String ResetPosition()
        {
            Index = 0;
            String currLine = scriptRows[Index];
            return currLine;
        }
    }
}
