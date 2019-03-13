using Microsoft.EntityFrameworkCore;
using Rolebased_Authorization.Repository.Helpers.Interfaces;
using Rolebased_Authorization.Repository.Models;
using Rolebased_Authorization.Repository.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rolebased_Authorization.Repository.Helpers
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ApplicationDbContext _context;
        public AuthRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<User> Login(string username, string password)
        {
            var user = await _context.ApplictionUsers.Include(p => p.Role).FirstOrDefaultAsync(x => x.Username == username);

            if (user == null)
                return null;

            if (!VerifyPasswordHash(username, password))
                return null;

            return user;
        }

        private bool VerifyPasswordHash(string username, string password)
        {
            //using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            //{
            //    var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            //    for (int i = 0; i < computedHash.Length; i++)
            //    {
            //        if (computedHash[i] != passwordHash[i]) return false;
            //    }
            //}
            var user = _context.ApplictionUsers.SingleOrDefault(x => x.Username == username && x.Password == password);
            if (user != null)
                return true;

            return false;
        }

        //public async Task<User> Register(User user, string password)
        //{
        //    byte[] passwordHash, passwordSalt;
        //    CreatePasswordHash(password, out passwordHash, out passwordSalt);

        //    user.PasswordHash = passwordHash;
        //    user.PasswordSalt = passwordSalt;

        //    await _context.Users.AddAsync(user);

        //    await _context.SaveChangesAsync();

        //    return user;
        //}

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<bool> UserExists(string username)
        {
            if (await _context.ApplictionUsers.AnyAsync(x => x.Username == username))
                return true;

            return false;
        }
    }
}
