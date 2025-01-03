﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Results
{
    public class Result : IResult
    {
        public Result(bool success, string message) : this(success)
        {
            Message = message;
            Success = success;
        }
        public Result(bool success)
        {
            Success = success;
        }

        public bool Success { get; private set; }

        public string Message { get; private set; }
    }
}
