using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace FullProject.ProcessorSchedulingInDistributedSystems_5.Algorithms
{
    class MaxThreshold
    {
        public static List<Processor> processor = new List<Processor>();
        public static TextBlock maxThreshold(List<double> process, int N, int maxThr, int max_time)
        {
            double sum_of_means = 0;
            int sum_of_request = 0;
            int sum_of_process_migrations = 0;
            Random rand = new Random();
            TextBlock block = new TextBlock() { Text = "" };



            for (int i = 0; i < N; ++i)
            {
                processor.Add(new Processor() { starting_proc = rand.Next(0, N) });
            }


            for (int i = 0; i < max_time; i++)
            {
                foreach (Processor p in processor)
                {
                    p.starting_time -= 1;
                    if (p.starting_time == 0)
                    {
                        int max_temp = maxThr;
                        int times_repeated = 0;
                        if (processor[p.starting_proc].utilization < maxThr)
                        {
                            while (max_temp != 0)
                            {
                                times_repeated += 1;
                                int rand_processor = rand.Next(0, N);
                                sum_of_request += 1;
                                if (processor[rand_processor].calc_util() < max_temp)
                                {
                                    processor[rand_processor].processes_running.Add(p);
                                    sum_of_process_migrations += 1;
                                    break;
                                }
                                else
                                {
                                    if (times_repeated == N)
                                    {
                                        max_temp += 1;
                                        times_repeated = 0;
                                    }
                                }
                            }
                        }
                        else
                        {
                            processor[p.starting_proc].processes_running.Add(p);
                        }

                    }
                }

                foreach (Processor p2 in processor.ToList())
                {
                    foreach (Processor p3 in p2.processes_running.ToList())
                    {
                        p3.time_required -= 1;
                        if (p3.time_required == 0)
                            p2.processes_running.Remove(p3);
                    }
                    p2.update_mean_util();
                }
            }


            Console.WriteLine("Maximum Threshold processor request, average utilization of each processor:");
            for (int temp = 0; temp < processor.Count(); temp++)
                sum_of_means += processor[temp].get_mean_util();

            block.Text += "Max Threshold processor request, standard deviation of each processor:" + Environment.NewLine;
            block.Text += "    Standard Deviation: " + (sum_of_means / N) + "%" + Environment.NewLine;
            block.Text += "    Total number of load requests: " + sum_of_request + Environment.NewLine;
            block.Text += "    Total number of migrations: " + sum_of_process_migrations + Environment.NewLine;

            return block;
        }
    }
}
