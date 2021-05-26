﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ZiTechDev.Business.Engines.CustomResult
{
    public class Successed<T> : ApiResult<T>
    {
        public Successed()
        {
            IsSuccessed = true;
        }

        public Successed(T returnedObject)
        {
            IsSuccessed = true;
            ReturnedObject = returnedObject;
        }
    }
}