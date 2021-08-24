using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FullProject.DiskSchedulingAlgorithms_2.Algorithms
{
    class SSTF_Algorithm : ScheduleAlgorithm
    {
        public const int PRIOR = -1;
        public string GetName()
        {
            return "SSTF";
        }

        public int ReadNextRequest(List<int> readRequest, int previousRead, ref bool direction)
        {
            int closest2Previous = int.MaxValue;
            int closestPosition = PRIOR;
            foreach (var request in readRequest)
            {
                int distance = Math.Abs(previousRead - request);
                if (closest2Previous > distance)
                {
                    closest2Previous = distance;
                    closestPosition = request;
                }
            }
            return closestPosition;
        }
    }
}
