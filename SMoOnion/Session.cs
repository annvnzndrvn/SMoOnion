using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMoOnion
{
    class Session
    {
        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private int _pictureCount = 0;

        public int PictureCount
        {
            get { return _pictureCount; }
            set { _pictureCount = value; }
        }
    }
}
