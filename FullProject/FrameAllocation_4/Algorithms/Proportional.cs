using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FullProject.FrameAllocation_4.Algorithms
{
    class Proportional : BasicAlgorithm
    {
        public Proportional(string title, int numberOfFrames) : base(title, numberOfFrames)
        {
        }

        //This might leave some unused frames which will never be used
        public override void AllocateFrames(LinkedList<Process> listOfProcesses)
        {

            //This method populates and also allocates frames

            //Populates
            for (int i = 0; i < numberOfFrames; i++)
            {
                frames.Add(new Frame(i), null);
            }

            //For each process, calculate the right number of frames
            int total_number_of_pages = 0;
            foreach (Process p in listOfProcesses) total_number_of_pages += p.getListOfPages().Count;

            foreach (Process p in listOfProcesses)
            {

                double number_of_frames_to_give = p.getListOfPages().Count;
                number_of_frames_to_give = number_of_frames_to_give / total_number_of_pages;
                number_of_frames_to_give = number_of_frames_to_give * numberOfFrames;

                //Ensure at least 1 frame for each
                if (number_of_frames_to_give < 1) number_of_frames_to_give = 1;

                //System.out.println(number_of_frames_to_give);
                while (number_of_frames_to_give >= 1)
                {
                    increaseNumberOfFrames(p);
                    number_of_frames_to_give--;
                }
            }

            foreach (Process p in listOfProcesses)
            {
                int counter = 0;
                foreach (Frame f in frames.Keys)
                {
                    if (p.equals(frames[f])) counter++;
                }
                // TODO
                // Take it to the UI
                // Console.WriteLine("Process " + p.getId() + " has " + counter);
            }
        }
    }
}
