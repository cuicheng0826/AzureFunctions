using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileFunctions.Models
{
    public class FuncException : Exception
    {
        public FuncException(string msg) : base(msg) { }
    }
}
