using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FullProject.DiskSchedulingAlgorithms_2.Algorithms
{
    class CSCAN_Algorithm : ScheduleAlgorithm
    {
        public string GetName()
        {
            return "CSCAN";
        }

        public int ReadNextRequest(List<int> readRequest, int previousRead, ref bool direction)
        {
            bool tempDirection = direction;
            int readValue = direction ? DiskScheduling.RIGHT_BORDER : DiskScheduling.LEFT_BORDER;

            List<int> sortedRequests = readRequest.OrderBy(n => tempDirection ? n : -n).ToList();

            foreach (var request in sortedRequests)
            {
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

            if (readValue == DiskScheduling.RIGHT_BORDER && previousRead == DiskScheduling.RIGHT_BORDER)
                readValue = 0;
            if (readValue == DiskScheduling.LEFT_BORDER && previousRead == DiskScheduling.LEFT_BORDER)
                readValue = 100;
            return readValue;
        }
    }
}
