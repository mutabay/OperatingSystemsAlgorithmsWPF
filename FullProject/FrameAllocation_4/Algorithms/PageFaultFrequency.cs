using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FullProject.FrameAllocation_4.Algorithms
{
    public class Integer
    {
        public int Value { get; set; }


        public Integer()
        {
        }

        public Integer(int value)
        {
            Value = value;
        }


        // Custom cast from "int":
        public static implicit operator Integer(Int32 x)
        {
            return new Integer(x);
        }

        // Custom cast to "int":
        public static implicit operator Int32(Integer x)
        {
            return x.Value;
        }


        public override string ToString()
        {
            return string.Format("Integer({0})", Value);
        }
    }

    class PageFaultFrequency : BasicAlgorithm
    {
        private const double PAGE_FAULT_UPPERLIMIT = 0.6;
        private const double PAGE_FAULT_LOWERLIMIT = 0.1;

        private Dictionary<Process, Integer> numberOfFaults = new Dictionary<Process, Integer>();
        private Dictionary<Process, Integer> numberOfOperations = new Dictionary<Process, Integer>();


        public PageFaultFrequency(string title, int numberOfFrames) : base(title, numberOfFrames)
        {
            for (int i = 0; i < numberOfFrames; i++)
            {
                frames.Add(new Frame(i), null);
            }
        }

        public virtual void simulate(LinkedList<Process> listOfProcesses)
        {
            Reset();

            //Ensure at least 1 frame for each process
            foreach (Process p in listOfProcesses)
            {
                increaseNumberOfFrames(p);
            }

            //Populate number of faults
            for (int i = 0; i < listOfProcesses.Count; i++)
            {
                numberOfFaults.Add(listOfProcesses.ElementAt(i), 0);
                numberOfOperations.Add(listOfProcesses.ElementAt(i), 0);
            }

            //Local copy
            LinkedList<Process> localList = new LinkedList<Process>();
            foreach (Process p in listOfProcesses)
            {
                localList.AddLast(Process.Copy(p));
            }

            System.Random rand;

            while (localList.Any())
            {
                rand = new System.Random();
                //I choose the process at random because I dont want to go 1 by 1
                Process randomProcess = localList.ElementAt(rand.Next(localList.Count));
                Page page = randomProcess.NextPage();

                //Increase number of operations for that process
                numberOfOperations.Add(randomProcess, numberOfOperations[randomProcess] + 1);

                //control if the page exists already
                bool pageWasFound = false;
                foreach (Frame f in frames.Keys)
                {
                    if (randomProcess.equals(frames[f]) && f.getPage().equals(page))
                    {
                        //page hit
                        f.resetTimeSinceLastAccess();
                        pageWasFound = true;
                    }
                }

                //If pageWasFound, then it was a page hit and we dont need to do anything

                //If the page was not found
                if (!pageWasFound)
                {
                    numberOfErrors++; //This is the global counter for faults


                    //Increase the number of faults of this process
                    numberOfFaults.Add(randomProcess, numberOfFaults[randomProcess] + 1);
                    //THIS IS WHERE THE IMPORTANT STUFF IN THIS METHOD HAPPENS
                    double faultFreq = numberOfFaults[randomProcess];
                    faultFreq = faultFreq / numberOfOperations[randomProcess];
                    //System.out.println(faultFreq);
                    if (faultFreq >= PAGE_FAULT_UPPERLIMIT)
                    {
                        increaseNumberOfFrames(randomProcess);
                    }
                    else if (faultFreq <= PAGE_FAULT_LOWERLIMIT)
                    {
                        decreaseNumberOfFrames(randomProcess);
                    }


                    //This method implements LRU page replacement
                    Frame frameToReplace = GetFrameToReplace(randomProcess);

                    frameToReplace.setPage(page);
                    frameToReplace.resetTimeSinceLastAccess();
                }


                //We increase the counter for number of operations
                numberOfOperations.Add(randomProcess, numberOfOperations[randomProcess] + 1);

                //Puts the current state of the frames into the log
                Log(CreateMemoryStateLog());

                //Increase the time for all frames (this is for LRU)
                foreach (Frame f in frames.Keys)
                    f.increaseTimeSinceLastAccess();

                if (randomProcess.IsFinished())
                    localList.Remove(randomProcess);

            }

            Finish();
        }


        public override void AllocateFrames(LinkedList<Process> listOfProcess)
        {
            // This method is never used for dynamic allocation
            // Instead, its functionality is inside simulate()        
        }
    }
}
