﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Exceptions
{
    public class OrderNotFoundException : Exception
    {
        public OrderNotFoundException(string name, object key)
            : base($"Entity {name} - {key} is not found")
        {

        }
    }
}
