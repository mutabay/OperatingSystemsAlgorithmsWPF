using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FullProject.FrameAllocation_4.Algorithms
{
    class WorkingSet : BasicAlgorithm
    {
        private const int WORKING_SET_WINDOW_SIZE = 4;
        private const int EXECUTIONS_PER_WORKING_SET_RESET = 6; 	//BOTH OF THESE ARE ARBITRARY

        public WorkingSet(string title, int numberOfFrames) : base(title, numberOfFrames)
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

            int working_set_reset_timer = 0;

            //Local copy
            LinkedList<Process> localList = new LinkedList<Process>();
            foreach (Process p in listOfProcesses)
            {
                localList.AddLast(Process.Copy(p));
            }
            System.Random rand;

            while (localList.Any())
            {

                //Deals with the working set reset timer
                if (working_set_reset_timer % EXECUTIONS_PER_WORKING_SET_RESET == 0)
                {
                    AllocateFrames(localList);
                    working_set_reset_timer = 0;
                }
                working_set_reset_timer++;

                rand = new System.Random();

                //I choose the process at random because I dont want to go 1 by 1
                Process randomProcess = localList.ElementAt(rand.Next(localList.Count));
                Page page = randomProcess.NextPage();

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

                //If page was NOT found
                if (!pageWasFound)
                {
                    numberOfErrors++;
                    //This method implements LRU page replacement
                    Frame frameToReplace = GetFrameToReplace(randomProcess);

                    frameToReplace.setPage(page);
                    frameToReplace.resetTimeSinceLastAccess();
                }

                //Puts the current state of the frames into the log
                Log(CreateMemoryStateLog());

                //Increase the time for all frames (this is for LRU)
                foreach (Frame f in frames.Keys)
                    f.increaseTimeSinceLastAccess();

                if (randomProcess.IsFinished())
                {
                    localList.Remove(randomProcess);
                }
            }


            Finish();
        }

        //For this class this method is only called with localList (the local copy of listOfProcesses)
        //Therefore finished processes will not be accounted for
        public override void AllocateFrames(LinkedList<Process> listOfProcesses)
        {
            foreach (Process p in listOfProcesses)
            {
                //Find the number of frames that should be allocated
                int working_set_size = p.getWorkingSetSize(WORKING_SET_WINDOW_SIZE);

                //Find the current number of allocated frames
                int frames_allocated = 0;
                foreach (Frame f in frames.Keys)
                {
                    if (p.equals(frames[f])) frames_allocated++;
                }

                //Try to make those numbers equal
                while (frames_allocated > working_set_size)
                {
                    decreaseNumberOfFrames(p);
                    frames_allocated--;
                }
                while (frames_allocated < working_set_size)
                {
                    increaseNumberOfFrames(p);
                    frames_allocated++;
                }

                //If the numbers are equal, dont do anything

                //Keep in mind this algorithm is very susceptible to THRASHING
                //Meaning it might create a situation in which every page is being searched for
            }
        }
    }

}
