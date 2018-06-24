using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationApp
{
    public enum OFFICE_TYPE
    {
        POWERPOINT
    }
    interface OfficeWindow
    {
        bool checkActivateWindow();
        int getPages();
        void next();
        void previous();
        void first();
        void last();

        OFFICE_TYPE getType();
    }
}
