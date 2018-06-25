using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using PPt = Microsoft.Office.Interop.PowerPoint;


namespace PresentationApp
{
    class Powerpoint : OfficeWindow
    {
        private static Logger logger = NLog.LogManager.GetCurrentClassLogger();

        PPt.Application pptApplication;
        PPt.Presentation presentation;
        PPt.Slides slides;
        PPt.Slide slide;

        // Slide count
        int slidescount;
        // slide index
        int slideIndex;

        public bool checkActivateWindow()
        {
            try
            {
                // Get Running PowerPoint Application object
                pptApplication = Marshal.GetActiveObject("PowerPoint.Application") as PPt.Application;
                if (pptApplication != null)
                {
                    // Get Presentation Object
                    presentation = pptApplication.ActivePresentation;
                    // Get Slide collection object
                    slides = presentation.Slides;
                    // Get Slide count
                    slidescount = slides.Count;
                    // Get current selected slide 
                    try
                    {
                        // Get selected slide object in normal view
                        slide = slides[pptApplication.ActiveWindow.Selection.SlideRange.SlideNumber];
                    }
                    catch
                    {
                        // Get selected slide object in reading view
                        slide = pptApplication.SlideShowWindows[1].View.Slide;
                    }
                }
                return true;
            }
            catch
            {
                logger.Error("Please Run PowerPoint Firstly");
            }
            return false;
        }

        public int currentPage()
        {
            return slide.SlideIndex;
        }

        public void first()
        {
            try
            {
                // Call Select method to select first slide in normal view
                slides[1].Select();
                slide = slides[1];
            }
            catch
            {
                // Transform to first page in reading view
                pptApplication.SlideShowWindows[1].View.First();
                slide = pptApplication.SlideShowWindows[1].View.Slide;
            }
        }

        public int getPages()
        {
            return slidescount;
        }

        public OFFICE_TYPE getType()
        {
            return OFFICE_TYPE.POWERPOINT;
        }

        public void last()
        {
            try
            {
                slides[slidescount].Select();
                slide = slides[slidescount];
            }
            catch
            {
                pptApplication.SlideShowWindows[1].View.Last();
                slide = pptApplication.SlideShowWindows[1].View.Slide;
            }
        }

        public void next()
        {
            slideIndex = slide.SlideIndex + 1;
            if (slideIndex > slidescount)
            {
                logger.Warn("it's last page");
            }
            else
            {
                try
                {
                    slide = slides[slideIndex];
                    slides[slideIndex].Select();
                }
                catch
                {
                    pptApplication.SlideShowWindows[1].View.Next();
                    slide = pptApplication.SlideShowWindows[1].View.Slide;
                }
            }
        }

        public void previous()
        {
            slideIndex = slide.SlideIndex - 1;
            if (slideIndex >= 1)
            {
                try
                {
                    slide = slides[slideIndex];
                    slides[slideIndex].Select();
                }
                catch
                {
                    pptApplication.SlideShowWindows[1].View.Previous();
                    slide = pptApplication.SlideShowWindows[1].View.Slide;
                }
            }
            else
            {
                logger.Warn("it's first page");

            }
        }

        public void play()
        {
            int index = slide.SlideIndex;
            logger.Debug(">>>play: current page = " + index);
            presentation.SlideShowSettings.Run();
            goToPage(index);
        }

        public void goToPage(int page)
        {
            slideIndex = page;
            if (slideIndex > slidescount)
            {
                logger.Warn("it's last page");
            }
            else
            {
                try
                {
                    slide = slides[slideIndex];
                    slides[slideIndex].Select();
                }
                catch
                {
                    pptApplication.SlideShowWindows[1].View.Next();
                    slide = pptApplication.SlideShowWindows[1].View.Slide;
                }
            }
        }
    }
}
