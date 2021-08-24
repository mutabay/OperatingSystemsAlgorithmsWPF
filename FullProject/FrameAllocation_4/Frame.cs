using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FullProject.FrameAllocation_4
{
    public class Frame
    {
        private Page page = null;
        private readonly int id;
        private int time_since_last_access = 0;

        public Frame(int id)
        {
            this.id = id;
        }



        public int getId()
        {
            return id;
        }

        public int hashCode()
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
            Frame other = (Frame)obj;
            if (id != other.id)
                return false;
            return true;
        }

        public void setPage(Page p)
        {
            page = p;
        }

        public Page getPage()
        {
            if (page == null)
                return new Page(-1);
            return page;
        }

        public void resetTimeSinceLastAccess()
        {
            time_since_last_access = 0;
        }

        public void increaseTimeSinceLastAccess()
        {
            time_since_last_access++;
        }

        public int getTimeSinceLastAccess()
        {
            return time_since_last_access;
        }
    }

}
