﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Logging.Correlation
{
    public interface ICorrelationIdGenerator
    {
        string Get();
        string Set(string correlationId);
    }
}
