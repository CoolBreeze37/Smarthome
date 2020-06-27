using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smarthome
{
    public class User
    {
        public string uname { get; set; }
        public string name { get; set; }
        public string IDcard { get; set; }
        public string pwd { get; set; }
        public Int64 FaceID { get; set; }
        public bool isadmin { get; set; }
    }
}
