using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Rolebased_Authorization.Repository.Helpers;
using Rolebased_Authorization.Repository.Helpers.Interfaces;
using Rolebased_Authorization.Repository.Models;
using WebApi.Helpers;

namespace WebApi.Services
{
    public interface IUserService
    {
        //User Authenticate(string username, string password);
        IEnumerable<User> GetAll();
        User GetById(int id);
    }

    public class UserService 
    {
        private readonly AppSettings _appSettings;
        private readonly IUnitOfWork _uow;
        private readonly IRepository<User> _repo;
        private readonly IAuthRepository _authRepository;
        public UserService(IOptions<AppSettings> appSettings, IUnitOfWork unit, IRepository<User> repo, IAuthRepository authRepository)
        {
            _appSettings = appSettings.Value;
            _uow = unit;
            _repo = repo;
            _authRepository = authRepository;
        }

        public async Task<User> Authenticate(string username, string password)
        {
            var user = await _authRepository.Login(username, password);

            // return null if user not found
            if (user == null)
                return null;

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] 
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                     new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);

            // remove password before returning
            user.Password = null;

            return user;
        }

       
    }
}