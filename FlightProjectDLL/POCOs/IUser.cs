﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightProject
{
    public interface IUser
    {
        string Username { get; }
        string Password { get; }
    }
}
