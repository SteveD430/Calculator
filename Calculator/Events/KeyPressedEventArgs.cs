using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.Events
{
    public class KeyPressedEventArgs : EventArgs
    {
        public KeyPressedEventArgs(char key)
        {
            Key = key;
        }
        public char Key { get; private set; }
    }
}
