using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace FullProject.PageReplacementAlgorithms_3
{
    public class PageReplacement
    {
        public List<int> pageReferenceList { get; set; }
        public List<int> pageFrameList { get; set; }
        public int pageFrameCount { get; set; }
        public int pageFaults { get; set; }
        public int hit { get; set; }

        public Label label { get; set; }


        public PageReplacement(List<int> pageReferenceList, int pageFrameCount)
        {
            this.pageReferenceList = pageReferenceList;
            this.pageFrameCount = pageFrameCount;
            pageFrameList = new List<int>(pageFrameCount);
            pageFaults = 0;
            hit = 0;
            label = new Label() { Content = "" };
        }

        public void ClearProperties()
        {
            pageFrameList.Clear();
            pageFaults = 0;
            hit = 0;
            label.Content = "";
        }


        public void ResetProperties(List<int> pageReferenceList, int pageFrameCount)
        {
            this.pageReferenceList = pageReferenceList;
            this.pageFrameCount = pageFrameCount;
            if (pageFrameList != null) pageFrameList.Clear();
            pageFaults = 0;
            hit = 0;
            label.Content = "";
        }

        public void FIFO()
        {
            ClearProperties();
            Queue<int> queue = new Queue<int>();

            // Not empty
            if (pageReferenceList.Any())
            {
                for (int i = 0; i < pageReferenceList.Count; i++)
                {
                    if (pageFrameList.Count < pageFrameCount)
                    {
                        // Insert it into set if not present already which represents page fault.
                        if (!pageFrameList.Contains(pageReferenceList[i]))
                        {
                            pageFrameList.Add(pageReferenceList[i]);
                            // increment page fault.
                            pageFaults++;

                            // Push the current page into the queue.
                            queue.Enqueue(pageReferenceList[i]);
                        }
                        else
                            hit++;
                    }

                    // If the set is full then need to remove the first page of the queue.
                    else
                    {
                        // Check if current page is not already present in the set.
                        if (!pageFrameList.Contains(pageReferenceList[i]))
                        {
                            // Peek first page from the queue.
                            int peekedPageReference = (int)queue.Peek();
                            queue.Dequeue();

                            pageFrameList.Remove(peekedPageReference);
                            // Replacing new page.
                            pageFrameList.Add(pageReferenceList[i]);
                            // Push the current page into the queue.
                            queue.Enqueue(pageReferenceList[i]);

                            pageFaults++;
                        }
                        else
                            hit++;
                    }
                    label.Content += "";
                }
            }
        }

        public void LRU()
        {
            ClearProperties();
            // Storing least recently used indexes of pages.
            Dictionary<int, int> dictionary = new Dictionary<int, int>();

            for (int i = 0; i < pageReferenceList.Count; i++)
            {
                // Checking if the set can hold more pages.
                if (pageFrameList.Count < pageFrameCount)
                {
                    // Insert it into set if not present already which represents page fault.
                    if (!pageFrameList.Contains(pageReferenceList[i]))
                    {
                        pageFrameList.Add(pageReferenceList[i]);
                        pageFaults++;
                    }
                    else
                        hit++;

                    // Store the recently used index of each page.
                    if (dictionary.ContainsKey(pageReferenceList[i]))
                        dictionary[pageReferenceList[i]] = i;
                    else
                        dictionary.Add(pageReferenceList[i], i);
                }


                // If the set is full then need to remove the least recently used page
                // and insert current page.
                else
                {
                    // Check if current page is not already present in the set.
                    if (!pageFrameList.Contains(pageReferenceList[i]))
                    {
                        // Find least recently used pages that is present in the set
                        int lru = int.MaxValue, peekedPageReference = int.MinValue;

                        foreach (var pageFrame in pageFrameList)
                        {
                            int temp = pageFrame;
                            if (dictionary[temp] < lru)
                            {
                                lru = dictionary[temp];
                                peekedPageReference = temp;
                            }
                        }

                        // Remove the indexes page
                        pageFrameList.Remove(peekedPageReference);
                        dictionary.Remove(peekedPageReference);

                        pageFrameList.Add(pageReferenceList[i]);
                        pageFaults++;
                    }
                    else
                        hit++;

                    // Update the current page index.
                    if (dictionary.ContainsKey(pageReferenceList[i]))
                        dictionary[pageReferenceList[i]] = i;
                    else
                        dictionary.Add(pageReferenceList[i], i);
                }
            }
        }

        private int Predict(int index)
        {
            // Store the index of pages which are going to be used recently in future
            int res = -1, farthest = index;
            for (int i = 0; i < pageFrameList.Count; i++)
            {
                int j;
                for (j = index; j < pageReferenceList.Count; j++)
                {
                    if (pageFrameList[i] == pageReferenceList[i])
                    {
                        if (j > farthest)
                        {
                            farthest = j;
                            res = i;
                        }
                        break;
                    }
                }

                // If a page is never referenced in future, return it
                if (j == pageReferenceList.Count)
                    return i;
            }

            return (res == -1) ? 0 : res;
        }
        public void OPT()
        {
            ClearProperties();
            pageFaults--;
            hit++;
            for (int i = 0; i < pageReferenceList.Count; i++)
            {
                if (pageFrameList.Contains(pageReferenceList[i]))
                {
                    hit++;
                    continue;
                }

                if (pageFrameList.Count < pageFrameCount)
                    pageFrameList.Add(pageReferenceList[i]);

                else
                {
                    int j = Predict(i + 1);
                    pageFrameList[j] = pageReferenceList[i];
                }
            }

            pageFaults = pageReferenceList.Count - hit;
        }

        public void ALRU()
        {
            ClearProperties();
            // TODO Control
            bool[] bitref = new bool[pageFrameCount];
            for (int i = 0; i < pageFrameCount; i++)
                bitref[i] = false;

            // Find the first element that doesn't have the bitref set to true.
            int ptr = 0;

            Queue<int> queue = new Queue<int>();
            int count = 0;
            for (int i = 0; i < pageReferenceList.Count; i++)
            {
                if (!queue.Contains(pageReferenceList[i]))
                {
                    // Queue is not filled up to capacity
                    if (count < pageFrameCount)
                    {
                        queue.Enqueue(pageReferenceList[i]);
                        count++;
                    }

                    // Queue is filled up to capacity
                    else
                    {
                        ptr = 0;

                        // Find the first value that has its bit set to 0
                        while (queue.Any())
                        {
                            if (bitref[ptr % pageFrameCount])
                                bitref[ptr % pageFrameCount] = !(bitref[ptr % pageFrameCount]);

                            // Found the bit value 0
                            else
                                break;
                            ptr++;
                        }

                        // If the queue was empty.
                        if (!(queue.Any()))
                            queue.Enqueue(pageReferenceList[i]);

                        // If queue wasn't empty.
                        else
                        {
                            int j = 0;

                            while (j < (ptr % pageFrameCount))
                            {
                                int t1 = queue.Peek();
                                queue.Dequeue();
                                queue.Enqueue(t1);
                                bool temp = bitref[0];

                                // Rotate the bitref array
                                for (int counter = 0; counter < pageFrameCount - 1; counter++)
                                    bitref[counter] = bitref[counter + 1];
                                bitref[pageFrameCount - 1] = temp;
                                j++;
                            }

                            queue.Dequeue();

                            queue.Enqueue(pageReferenceList[i]);
                        }
                    }
                    pageFaults++;
                }

                else
                {
                    Queue<int> tempQueue = queue;
                    int counter = 0;
                    while (queue.Any())
                    {
                        if (queue.Peek() == pageReferenceList[i])
                            bitref[counter] = true;
                        counter++;
                        queue.Dequeue();
                    }

                    queue = tempQueue;
                    hit++;
                }
            }
        }

        public void RAND()
        {
            ClearProperties();
            Random rand = new Random();
            int index;
            // Not empty
            if (pageReferenceList.Any())
            {
                for (int i = 0; i < pageReferenceList.Count; i++)
                {
                    index = rand.Next(pageFrameCount - 1);
                    if (pageFrameList.Count < pageFrameCount)
                    {
                        // Insert it into set if not present already which represents page fault.
                        if (!pageFrameList.Contains(pageReferenceList[i]))
                        {
                            pageFrameList.Add(pageReferenceList[i]);
                            // increment page fault.
                            pageFaults++;
                        }
                        else
                            hit++;
                    }

                    // If the set is full then need to remove the first page of the queue.
                    else
                    {
                        // Check if current page is not already present in the set.
                        if (!pageFrameList.Contains(pageReferenceList[i]))
                        {
                            pageFrameList.Remove(pageReferenceList[index]);
                            // Replacing new page.
                            pageFrameList[index] = pageReferenceList[i];
                            pageFaults++;
                        }
                        else
                            hit++;
                    }
                    label.Content += "";
                }
            }
        }
    }
}
