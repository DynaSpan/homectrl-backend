using System;

namespace HomeCTRL.Backend.Core.Exceptions
{
    public class InputException : Exception
    {
        public InputException(string msg): base(msg)
        {
            
        }
    }
}