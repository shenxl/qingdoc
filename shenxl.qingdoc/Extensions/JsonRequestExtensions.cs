﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace shenxl.qingdoc.Extensions
{
    public static class JsonRequestExtensions
    {
        public static bool IsJsonRequest(this HttpRequestBase request)
        {
            return string.Equals(request["format"], "json");
        }
    }
}