﻿using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using AspNetCore.MongoDB;
using Backend.Core.Security;
using Backend.Models.Database;
using Microsoft.CodeAnalysis.CodeActions;

namespace Backend.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IPasswordStorage _passwordStorage;
        private readonly ISecurityTokenFactory _securityTokenFactory;
        private readonly IMongoOperation<User> _operation;

        public UsersController(IPasswordStorage passwordStorage, ISecurityTokenFactory securityTokenFactory, IMongoOperation<User> operation)
        {
            _passwordStorage = passwordStorage;
            _securityTokenFactory = securityTokenFactory;
            _operation = operation;
        }

        [HttpGet("current")]
        public PrivateUserResponse Current()
        {
            return new PrivateUserResponse()
            {
                Id = Guid.NewGuid(),
                Email = "current@test.ch",
                Points = 150,
                Location = new LocationResponse()
                {
                    Latitude = 46.941060,
                    Longitude = 7.442725
                }
            };
        }

        [HttpGet]
        public PrivateUserResponse Get(Guid guid)
        {
            return new PrivateUserResponse()
            {
                Id = guid,
                Email = "user@test.ch",
                Points = 150,
                Location = new LocationResponse()
                {
                    Latitude = 46.941060,
                    Longitude = 7.442725
                }
            };
        }

        [HttpPost]
        public LoginResponse Login(string email)
        {
            User user = _operation.GetQuerableAsync().SingleOrDefault(u => u.Email == email);
            if (user == null)
            {
                string token = _securityTokenFactory.Create(new User
                {
                    Email = email
                });

                _operation.InsertOne(new User { Email = email, Password = _passwordStorage.Create("1234") });

                return new LoginResponse
                {
                    Token = token
                };
            }

            return new LoginResponse();
        }

        [HttpPost]
        public ActionResult<LoginResponse> Login(string email, string password)
        {
            User user = _operation.GetQuerableAsync().SingleOrDefault(u => u.Email == email);
            if (user == null)
            {
                return NotFound();
            }

            if (!_passwordStorage.Match(password, user.Password))
            {
                return Forbid();
            }

            string securityToken = _securityTokenFactory.Create(user);
            return new ActionResult<LoginResponse>(new LoginResponse
            {
                Token = securityToken
            });
        }
    }
}