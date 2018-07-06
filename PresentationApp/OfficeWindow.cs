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
    public delegate void SlideShowNextSlideEventHandler(int index);

    interface OfficeWindow
    {
        bool checkActivateWindow();
        int getPages();
        void next();
        void previous();
        void first();
        void last();
        int currentPage();
        void play();
        void stop();
        void enableLaserPen(bool isEnable);
        void enableColorPen(bool isEnable);
        void marker();

        event SlideShowNextSlideEventHandler onSlideIndexChanged;


        OFFICE_TYPE getType();
    }
}
