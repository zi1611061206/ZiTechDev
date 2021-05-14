using System;
using System.Collections.Generic;
using System.Text;

namespace ZiTechDev.Business.Engines.CustomResult
{
    public class Failed<T> : ApiResult<T>
    {
        public Failed(string message)
        {
            IsSuccessed = false;
            Message = message;
        }
    }
}
