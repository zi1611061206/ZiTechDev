using System;
using System.Collections.Generic;
using System.Text;

namespace ZiTechDev.Business.Engines.Exceptions
{
    public class ExceptionEngines : Exception
    {
        public ExceptionEngines()
        {
        }

        public ExceptionEngines(string message)
            : base(message)
        {
        }

        public ExceptionEngines(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
