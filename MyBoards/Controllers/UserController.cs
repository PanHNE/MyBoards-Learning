﻿using Microsoft.AspNetCore.Mvc;
using MyBoards.Models.Account;
using MyBoards.Services;

namespace MyBoards.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _accountService;

        public UserController(IUserService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("register")]
        public ActionResult Register([FromBody] RegisterUserDto registerAccountDto)
        {
            _accountService.RegisterUser(registerAccountDto);

            return Created("New user registered", null);
        }
    }
}