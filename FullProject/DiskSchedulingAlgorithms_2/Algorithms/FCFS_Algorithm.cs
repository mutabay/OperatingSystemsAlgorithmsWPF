using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FullProject.DiskSchedulingAlgorithms_2.Algorithms
{
    class FCFS_Algorithm : ScheduleAlgorithm
    {
        public string GetName()
        {
            return "FCFS";
        }

        public int ReadNextRequest(List<int> readRequest, int previousRead, ref bool direction)
        {
            return readRequest.First();
        }
    }
}
