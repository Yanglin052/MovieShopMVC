using ApplicationCore.Contracts.Repository;
using ApplicationCore.Contracts.Services;
using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using ApplicationCore.Exceptions;
using System.Security.Cryptography;
using ApplicationCore.Entities;

namespace Infrastructure.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUserRepository _userRepository;
        public AccountService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> RegisterUser(UserRegisterModel model)
        {
            // check if the user email already exists -- UserRepository

            var user = await _userRepository.GetUserByEmail(model.Email);

            // if the email already exists: 
            if (user != null)
            {
                throw new ConflictException("Email already exists, please try to login.");
            }

            // if the email does not exist, continue to register:
            // 1 - create a random salt
            var salt = GetRandomSalt();

            // 2 - get the password with created salt
            var hashedpassword = GetHashedPassword(model.Password,salt);

            // 3 - create User object and save using EF
            var newUser = new User
            {
                Email = model.Email,
                HashedPassword = hashedpassword,
                Salt = salt,
                FirstName = model.FirstName,
                LastName = model.LastName,
                DateOfBirth = model.DateOfBirth,
            };

            // 4 - save the new user to user table -- UserRepository
            var savedUser = await _userRepository.Add(newUser);
            if (savedUser.Id > 1)
            {
                return true; 
            }
            return false;

        }

        public async Task<UserModel> ValidateUser(string email, string password)
        {
            // go to the database and get the row by email:
            var user = await _userRepository.GetUserByEmail(email);

            if (user == null)
            {
                throw new Exception("Email does not exists.");
            }
            var hashedpassword = GetHashedPassword(password, user.Salt);

            if (hashedpassword == user.HashedPassword)
            {
                var userModel = new UserModel
                {
                    Id = user.Id,
                    DateOfBirth = user.DateOfBirth.GetValueOrDefault(),
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName
                };
                return userModel;
            }
            return null;
        }
        
        private string GetRandomSalt()
        {
            var randomBytes = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }

            return Convert.ToBase64String(randomBytes);
        }

        private string GetHashedPassword(string password, string salt)
        {
            var hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(password,
            Convert.FromBase64String(salt),
            KeyDerivationPrf.HMACSHA512,
            10000,
            256 / 8));
            return hashed;
        }
    }
}
