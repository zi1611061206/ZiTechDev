﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ZiTechDev.Business.Requests.Activity
{
    public class ActivityUpdateRequest
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int FunctionId { get; set; }
    }
}