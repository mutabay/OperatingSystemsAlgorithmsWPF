using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace FullProject.ProcessSchedulingAlgorithms_1
{
    public class Process
    {
        // Properties of "Process"
        public int processID { get; set; }
        public int burstTime { get; set; }
        public int arrivalTime { get; set; }
        public int waitingTime { get; set; }
        public int turnAroundTime { get; set; }
        public int status { get; set; }
        public int completionTime { get; set; }

        public static Random rand { get; set; }
        public Brush processColor;

        public const int RGB_MAX_VALUE = 256;

        public Process(int processId, int burstTime, int arrivalTime)
        {
            this.processID = processId;
            this.burstTime = burstTime;
            this.arrivalTime = arrivalTime;
            status = 0;
            turnAroundTime = 0;
            waitingTime = 0;
            completionTime = 0;
            processColor = Brushes.White;

        }
    }
}
