using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FullProject.DiskSchedulingAlgorithms_2.Algorithms
{
    class SCAN_Algorithm : ScheduleAlgorithm
    {
        public string GetName()
        {
            return "SCAN";
        }

        public int ReadNextRequest(List<int> readRequest, int previousRead, ref bool direction)
        {
            bool tempDirection = direction;
            int readValue = direction ? DiskScheduling.RIGHT_BORDER : DiskScheduling.LEFT_BORDER;

            List<int> sortedRequests = readRequest.OrderBy(n => tempDirection ? n : -n).ToList();

            foreach (var request in sortedRequests)
            {
                // Direction shows right | direction = 1 
                if (direction)
                {
                    if (request >= previousRead)
                    {
                        readValue = request;
                        break;
                    }
                }
                else
                {
                    if (request <= previousRead)
                    {
                        readValue = request;
                        break;
                    }
                }
            }

            if (DiskScheduling.RIGHT_BORDER == readValue || readValue == DiskScheduling.LEFT_BORDER)
                direction = !direction;
            return readValue;

        }
    }

}
