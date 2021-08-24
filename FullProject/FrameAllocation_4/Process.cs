using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FullProject.FrameAllocation_4
{
        public class Process
    {
        LinkedList<Page> listOfPages = new LinkedList<Page>();
        private readonly int id;

        public Process(int id)
        {
            this.id = id;
        }

        public void PutList(LinkedList<Page> list)
        {
            this.listOfPages = list;
        }

        public int GetId()
        {
            return id;
        }

        public Boolean IsFinished()
        {
            return listOfPages.Any();
        }

        public Page NextPage()
        {
            Page result = listOfPages.First();
            listOfPages.RemoveFirst();
            return result;
        }

        public int GetSize()
        {
            return listOfPages.Count();
        }

        public static Process Copy(Process process)
        {
            Process result = new Process(process.GetId());
            LinkedList<Page> copyOfPages = new LinkedList<Page>();
            foreach (Page p in process.listOfPages)
            {
                copyOfPages.AddLast(Page.Copy(p));
            }
            result.PutList(copyOfPages);
            return result;
        }


        public int HashCode()
        {
            const int prime = 31;
            int result = 1;
            result = prime * result + id;
            return result;
        }

        public Boolean equals(Object obj)
        {
            if (this == obj)
                return true;
            if (obj == null)
                return false;
            if (GetType() != obj.GetType())
                return false;
            Process other = (Process)obj;
            if (id != other.id)
                return false;
            return true;
        }

        public int getWorkingSetSize(int working_Set_window_size)
        {
            HashSet<Page> pages_already_counted = new HashSet<Page>();
            for (int i = 0; i < working_Set_window_size; i++)
            {
                if (i >= listOfPages.Count())
                    break;
                pages_already_counted.Add(listOfPages.ElementAt(i));
            }
            return pages_already_counted.Count();
        }

        public LinkedList<Page> getListOfPages()
        {
            return listOfPages;
        }
    }

}
