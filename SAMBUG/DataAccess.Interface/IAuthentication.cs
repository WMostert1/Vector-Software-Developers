﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Interface.DTOModels;

namespace DataAccess.Interface
{
    public interface IAuthentication
    {
        LoginResponse Login(LoginRequest loginRequest);
    }
}