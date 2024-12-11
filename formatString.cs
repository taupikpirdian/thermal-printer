using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace printerAPI
{
    class formatString 
    {
        public string formatStrng(string left, string right)
        {
            int leftWidht = 33 - right.Length;
            string formattedString = left.PadRight(leftWidht) + right;

            return formattedString;
        }
    }
}
