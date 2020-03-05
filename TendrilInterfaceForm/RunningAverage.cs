using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TendrilInterfaceForm
{
    class RunningAverage
    {
        public float avgF;
        public int avgI;
        int maxSize;
        int actSize;
        Queue<float> values;

        public RunningAverage (int n)
        {
            maxSize = n;
            avgF = 0;
            avgI = 0;
            actSize = 0;
            values = new Queue<float>(n);
        }

        public void AddValue(float val)
        {
            if (actSize != maxSize)
            {
                values.Enqueue(val);
                actSize = values.Count;
                avgF = values.Sum() / actSize;
                avgI = (int)avgF;
            } else
            {
                values.Dequeue();
                values.Enqueue(val);
                avgF = values.Sum() / actSize;
                avgI = (int)avgF;
            }
        }

        public void Clear()
        {
            values.Clear();
        }

    }
}
