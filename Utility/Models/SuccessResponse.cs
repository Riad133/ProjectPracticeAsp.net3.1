using System;
using System.Collections.Generic;
using System.Text;

namespace Utility.Models
{
    public   class SuccessResponse
    {
        public string Message { get; set; }
        public string DeveloperMessage { get; set; }
        public int statusCode { get; set; }
    }
}
