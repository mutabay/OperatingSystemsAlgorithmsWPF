using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FullProject.FrameAllocation_4.Algorithms
{
    public abstract class BasicAlgorithm
    {
        LinkedList<string> logList = new LinkedList<String>();
        string currentLog;
        string title;
        public Dictionary<Frame, Process> frames = new Dictionary<Frame, Process>();
        public int numberOfErrors;
        public int numberOfFrames;

        public BasicAlgorithm(String title, int numberOfFrames)
        {
            this.title = title;
            this.numberOfFrames = numberOfFrames;
        }

        //This will ALWAYS fail if nFrames < nProcesses (assuming no useless processes)
        public abstract void AllocateFrames(LinkedList<Process> listOfProcess);

        public void Log(String s)
        {
            currentLog += s + "\n";
        }

        public void Reset()
        {
            frames.Clear();
            //foreach (Frame f in frames.Keys)
            //{
            //    frames.Add(f, null);
            //}
            currentLog = "";
            numberOfErrors = 0;
            Console.WriteLine("Starting " + title);
        }

        public string Finish()
        {
            currentLog += numberOfErrors;
            logList.AddLast(currentLog);
            return "Finished " + title + "with" + numberOfErrors + "errors";
        }

        public String GetLog(int indexOfLog)
        {
            return logList.ElementAt(indexOfLog);
        }

        public String CreateMemoryStateLog()
        {
            String result = "";
            foreach (Frame f in frames.Keys)
            {
                int processID;
                if (frames[f] == null) processID = -1;
                else processID = frames[f].GetId();

                result += processID + " " + f.getPage().toString() + "|";
            }

            return result.Trim();
        }

        public int Compare(Frame o1, Frame o2)
        {
            if (o1.getTimeSinceLastAccess() < o2.getTimeSinceLastAccess())
                return 1;
            if (o1.getTimeSinceLastAccess() > o2.getTimeSinceLastAccess())
                return -1;
            return 0;
        }

        /*
	     * Uses LRU to determine which frame to replace
	     * @param randomProcess to which the frame belongs
	     * @return frame to be replaced
	    */
        public Frame GetFrameToReplace(Process randomProcess)
        {
            //I put all frames assigned to that process in a single set (insertion ordered)
            SortedSet<Frame> framesForProcess = new SortedSet<Frame>(new Comparator());

            foreach (Frame f in frames.Keys)
            {
                if (randomProcess.Equals((frames[f])))
                    framesForProcess.Add(f);
            }

            //I find the frame in that list with the most time_since_last_access
            //System.out.println(framesForThatProcess.size() + " " +randomProcess.getId());
            return framesForProcess.First();
        }

        public String getTitle()
        {
            return title;
        }

        public void increaseNumberOfFrames(Process randomProcess)
        {

            //Try to find frames that are not allocated to anything
            foreach (Frame f in frames.Keys)
            {
                if (frames[f] == null)
                {
                    frames?.Remove(f);
                    frames.Add(f, randomProcess);
                    return;
                }
            }

            //TODO might want to try to steal frame from someone else 
            //If I were to do this, search for empty frames first
        }

        public void decreaseNumberOfFrames(Process randomProcess)
        {
            int countFrames = 0;
            foreach (Frame f in frames.Keys)
            {
                if (randomProcess.equals(frames[f])) countFrames++;
            }
            if (countFrames <= 1) return;

            Frame frameToReplace = GetFrameToReplace(randomProcess);
            frames.Add(frameToReplace, null);

        }


        public void simulate(LinkedList<Process> listOfProcesses)
        {
            Reset();


            //The algorithm is in this 2nd method!!!!!
            AllocateFrames(listOfProcesses);

            //Local copy
            LinkedList<Process> localList = new LinkedList<Process>();
            foreach (Process p in listOfProcesses)
            {
                localList.AddLast(Process.Copy(p));
            }

            Random rand;
            while (localList.Any())
            {
                rand = new Random();
                //I choose the process at random because I dont want to go 1 by 1
                Process randomProcess = localList.ElementAt(rand.Next(localList.Count));
                Page page = randomProcess.NextPage();

                //control if the page exists already
                bool pageWasFound = false;
                foreach (Frame f in frames.Keys)
                {
                    if (randomProcess.equals(frames[f]) && page.equals(f.getPage()))
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
                    localList.Remove(randomProcess);

            }

            Finish();
        }


    }

    public class Comparator : IComparer<Frame>
    {
        int IComparer<Frame>.Compare(Frame o1, Frame o2)
        {
            //System.out.println(o1.getTimeSinceLastAccess() + " " + o2.getTimeSinceLastAccess());
            if (o1.getTimeSinceLastAccess() < o2.getTimeSinceLastAccess()) return 1;
            if (o1.getTimeSinceLastAccess() > o2.getTimeSinceLastAccess()) return -1;
            return 0;
        }

    }
}
