using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationApp
{
    class OfficeException : Exception
    {
        public OfficeException(): base("Office error")
        {
        }
    }
}
