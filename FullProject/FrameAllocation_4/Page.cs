using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FullProject.FrameAllocation_4
{
    public class Page
    {
        public int numberOfPage { get; set; }
        public Page(int num)
        {
            numberOfPage = num;
        }

        public static Page Copy(Page p)
        {
            return new Page(p.getNumberOfPage());
        }

        public int hashCode()
        {
            const int prime = 31;
            int result = 1;
            result = prime * result + numberOfPage;
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
            Page other = (Page)obj;
            if (numberOfPage != other.numberOfPage)
                return false;
            return true;
        }

        int getNumberOfPage()
        {
            return numberOfPage;
        }

        public String toString()
        {
            return "" + numberOfPage;
        }
    }

}
