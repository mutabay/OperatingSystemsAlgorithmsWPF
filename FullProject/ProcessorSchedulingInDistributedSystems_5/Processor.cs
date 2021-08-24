using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FullProject.ProcessorSchedulingInDistributedSystems_5
{
    class Processor : ProcessGenerator
    {
        public double mean_util;
        public int times_called;
        public int utilization;
        public List<Processor> processes_running = new List<Processor>();
        Random rand = new Random();
        public Processor()
        {
            base.starting_time = rand.Next(1, 1000);
            base.u1 = base.NextDouble();
            base.u2 = base.NextDouble();
            base.temp1 = Math.Sqrt(-2 * Math.Log(u1));
            base.temp2 = 2 * Math.PI * u2;

            base.proc_required = Convert.ToInt32(50 + 25 * (temp1 * Math.Cos(temp2)));

            base.time_required = Convert.ToInt32(100 + 100 * (temp1 * Math.Cos(temp2)));

            base.starting_proc = rand.Next(1, 100);

            utilization = 0;
            mean_util = 0.0;
            times_called = 0;
        }

        public int calc_util()
        {
            utilization = 0;
            foreach (Processor p in processes_running)
                utilization += p.proc_required;
            return utilization;

        }

        public void update_mean_util()
        {
            times_called += 1;
            mean_util += calc_util();
        }

        public double get_mean_util()
        {
            return this.mean_util / this.times_called;
        }
    }
}
