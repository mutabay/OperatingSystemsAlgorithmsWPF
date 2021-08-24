using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FullProject.FrameAllocation_4.Algorithms
{
    class Rand : BasicAlgorithm
    {
        public Rand(string title, int numberOfFrames) : base(title, numberOfFrames)
        {
        }

        public override void AllocateFrames(LinkedList<Process> listOfProcesses)
        {
            //This method both populates and allocated frames�

            //Populates
            for (int i = 0; i < numberOfFrames; i++)
            {
                frames.Add(new Frame(i), null);
            }

            //This ensures that every process has at least 1 frame
            foreach (Process p in listOfProcesses)
            {
                increaseNumberOfFrames(p);
            }

            System.Random rand;
            //Assign all the rest to random processes
            while (frames.ContainsValue(null))
            {
                rand = new System.Random();
                Process randomProcess = listOfProcesses.ElementAt(rand.Next(0, listOfProcesses.Count));
                increaseNumberOfFrames(randomProcess);
            }

            foreach (Process p in listOfProcesses)
            {
                int counter = 0;
                foreach (Frame f in frames.Keys)
                {
                    if (p.equals(frames[f])) counter++;
                }

                //TODO
                //Take it to the UI.
                //Console.WriteLine("Process " + p.getId() + " has " + counter);

            }

        }
    }

}
