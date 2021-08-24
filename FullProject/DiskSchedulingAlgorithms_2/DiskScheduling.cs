using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FullProject.DiskSchedulingAlgorithms_2.Algorithms;

namespace FullProject.DiskSchedulingAlgorithms_2
{
    class DiskScheduling
    {
        public const int RIGHT_BORDER = 200;
        public const int LEFT_BORDER = 0;

        private bool direction;
        private int previousRead;
        public int head { get; set; }

        public List<int> AlreadyRead { get; set; }
        public List<int> ReadRequests { get; set; }

        private ScheduleAlgorithm scheduleStrategy;

        public DiskScheduling()
        {
            this.ReadRequests = new List<int>();
            this.AlreadyRead = new List<int>();
            this.previousRead = 0;
            this.direction = true;
            this.head = 0;
        }

        public void Reset()
        {
            this.ReadRequests.Clear();
            this.AlreadyRead.Clear();
            this.previousRead = 0;
            this.direction = true;
        }

        public void SetStartHeader(int header)
        {
            this.AlreadyRead.Add(header);
        }

        public void SetPreviousRead(int read)
        {
            this.previousRead = read;
        }

        public void AddScheduleStrategy(ScheduleAlgorithm algorithmStrategy)
        {
            this.scheduleStrategy = algorithmStrategy;
        }

        public void AddReadRequest(int id)
        {
            this.ReadRequests.Add(id);
        }

        public void ReadNext()
        {
            this.previousRead = this.scheduleStrategy.ReadNextRequest(this.ReadRequests, this.previousRead, ref this.direction);
            this.ReadRequests.Remove(this.previousRead);
            this.AlreadyRead.Add(this.previousRead);
        }
    }
}
