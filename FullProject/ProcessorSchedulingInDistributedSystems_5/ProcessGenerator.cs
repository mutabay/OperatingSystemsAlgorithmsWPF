using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FullProject.ProcessorSchedulingInDistributedSystems_5
{
    class ProcessGenerator : Random
    {
        public double u1;
        public double u2;
        public double temp1;
        public double temp2;
        public int starting_time;
        public int proc_required;
        public int time_required;
        public int starting_proc;

        public ProcessGenerator(int N)
        {
            Random rand = new Random();

            starting_time = rand.Next(1, 1000);

            u1 = base.NextDouble();
            u2 = base.NextDouble();
            temp1 = Math.Sqrt(-2 * Math.Log(u1));
            temp2 = 2 * Math.PI * u2;

            proc_required = Convert.ToInt32(50 + 25 * (temp1 * Math.Cos(temp2)));

            time_required = Convert.ToInt32(100 + 100 * (temp1 * Math.Cos(temp2)));

            starting_proc = rand.Next(1, N);

        }

        public ProcessGenerator()
        {
        }

        public int getStarting_time()
        {
            return starting_time;
        }
        public int getProc_required()
        {
            return proc_required;
        }
        public int getTime_required()
        {
            return time_required;
        }
        public int getStarting_proc()
        {
            return starting_proc;
        }
    }
}
