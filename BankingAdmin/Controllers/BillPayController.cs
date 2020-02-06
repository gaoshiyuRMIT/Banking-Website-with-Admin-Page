using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using BankingLib.Models;
using BankingAdmin.Models.Repository;

namespace BankingAdmin.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BillPayController : Controller
    {
        private readonly IAsyncRepository<BillPay, int> _repo;

        public BillPayController(IAsyncRepository<BillPay, int> repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IEnumerable<BillPay>> Get()
        {
            return await _repo.GetAllAsync();
        }

        [HttpGet("{billPayId}")]
        public async Task<BillPay> Get(int billPayId)
        {
            return await _repo.GetAsync(billPayId);
        }


        [HttpPut("{billPayId}/block")]
        public async Task<BillPay> PutBlock(int billPayId)
        {
            var billPay = await _repo.GetAsync(billPayId);
            if (billPay == null)
                return null;

            billPay.Block();
            await _repo.UpdateAsync(billPayId, billPay);
            return billPay;
        }


        [HttpPut("{billPayId}/unblock")]
        public async Task<BillPay> PutUnblock(int billPayId)
        {
            var billPay = await _repo.GetAsync(billPayId);
            if (billPay == null)
                return null;

            if (billPay.Status == BillPayStatus.Blocked)
            { 
                billPay.Unblock();
                await _repo.UpdateAsync(billPayId, billPay);
            }
            return billPay;
        }

    }
}
