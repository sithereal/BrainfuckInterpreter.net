using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainfuckInterpreter.VM.Library
{
    class InvalidOperandException : Exception
    {
        public InvalidOperandException(string message) : base(message)
        {
        }
    }
}
