using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FullProject.DiskSchedulingAlgorithms_2.Algorithms
{
    interface ScheduleAlgorithm
    {
        string GetName();
        int ReadNextRequest(List<int> readRequest, int previousRead, ref bool direction);
    }
}
