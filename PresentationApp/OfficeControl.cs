using NLog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace PresentationApp
{
    class OfficeControl
    {
        private static Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private ArrayList controls = new ArrayList();

        public OfficeControl()
        {
            controls.Add(new Powerpoint());
        }

        private OfficeWindow getWindow(OFFICE_TYPE type)
        {
            foreach (OfficeWindow window in controls)
            {
                if (window.getType() == type)
                {
                    return window;
                }
            }
            throw new NullReferenceException();
        }

        public bool checkPowerpoint()
        {
            try
            {
                OfficeWindow window = getWindow(OFFICE_TYPE.POWERPOINT);
                return window.checkActivateWindow();
            }
            catch(Exception ex)
            {
                logger.Warn(ex.ToString());
            }
            return false;
        }

        public int getPages(OFFICE_TYPE type)
        {
            try
            {
                OfficeWindow window = getWindow(type);
                return window.getPages();
            }
            catch
            {
                throw;
            }
        }

        public void next(OFFICE_TYPE type)
        {
            try
            {
                OfficeWindow window = getWindow(type);
                window.next();
            }
            catch
            {
                throw;
            }
        }
        public void previous(OFFICE_TYPE type)
        {
            try
            {
                OfficeWindow window = getWindow(type);
                window.previous();
            }
            catch
            {
                throw;
            }
        }

        public void first(OFFICE_TYPE type)
        {
            try
            {
                OfficeWindow window = getWindow(type);
                window.first();
            }
            catch
            {
                throw;
            }
        }

        public void last(OFFICE_TYPE type)
        {
            try
            {
                OfficeWindow window = getWindow(type);
                window.last();
            }
            catch
            {
                throw;
            }
        }

        public int currentPage(OFFICE_TYPE type)
        {
            try
            {
                OfficeWindow window = getWindow(type);
                return window.currentPage();
            }
            catch
            {
                throw;
            }
        }

        public void play(OFFICE_TYPE type)
        {
            try
            {
                OfficeWindow window = getWindow(type);
                window.play();
            }
            catch
            {
                throw;
            }
        }

        public void stopRun(OFFICE_TYPE type)
        {
            try
            {
                OfficeWindow window = getWindow(type);
                window.stop();
            }
            catch
            {
                throw;
            }
        }
    }
}
