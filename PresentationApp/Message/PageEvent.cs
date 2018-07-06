using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationApp.Message
{
    public class PageEvent
    {

        public const String intent = "event_page_changed";
        public int page { get; set; }
    }
}
