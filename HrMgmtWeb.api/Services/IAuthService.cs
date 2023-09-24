﻿using HrMgmtWeb.Data.Models;

namespace HrMgmtWeb.api.Services
{
    public interface IAuthService
    {
        Task<(int, string)> Registeration(RegistrationModel model, string role);
        Task<(int, string)> Login(LoginModel model);
    }
}
