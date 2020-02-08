using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using BankingAdmin.Models.Repository;
using BankingLib.Models;

namespace BankingAdmin.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly IAsyncRepository<Login, string> _repo;
        public LoginController(IAsyncRepository<Login, string> repo) 
        {
            _repo = repo;
        }

        [HttpGet("admin")]
        public async Task<bool> LoginAdmin(string userName, string password)
        {
            return userName == "admin" && password == "admin";
        }

        [HttpPut("lock/customer/{customerId}")]
        public async Task<bool> LockLogin(int customerId)
        {
            var login = (await _repo.GetManyAsync(x => x.CustomerID == customerId)).First();
            login.Lock();
            await _repo.UpdateAsync(login.UserID, login);
            return login.IsLocked;
        }

        [HttpGet("lock/customer/{customerId}")]
        public async Task<bool> GetByCustomer(int customerId)
        {
            var login = (await _repo.GetManyAsync(x => x.CustomerID == customerId)).First();
            if (login.LockExpired)
            {
                login.Unlock();
                await _repo.UpdateAsync(login.UserID, login);
            }
            return login.IsLocked;
        }

        [HttpPut("unlock/customer/{customerId}")]
        public async Task<bool> UnlockLogin(int customerId)
        {
            var login = (await _repo.GetManyAsync(x => x.CustomerID == customerId)).First();
            login.Unlock();
            await _repo.UpdateAsync(login.UserID, login);
            return login.IsLocked;
        }

    }
}
