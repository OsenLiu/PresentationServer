using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationApp
{
    class MsgResponse
    {
        public enum RESULT_CODE
        {
            SUCCESS=0,
            NOT_FOUND
        }
        public String Intent { get; set; }
        public int code { get; set; }
        public String message { get; set; }
        public int page { get; set; }
        public bool isLaser { get; set; }
    }
}
