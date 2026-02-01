using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PRN232.LaptopShop.Repository.Entity;
using PRN232.LaptopShop.Service;
using PRN232.LaptopShop.Service.DTO;

namespace PRN232.LaptopShop.API.Controllers
{
    [Route("api/v1/accounts")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AccountService accountService;
        public AccountController(AccountService accountService)
        {
            this.accountService = accountService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAccounts(int pageIndex = 1, int pageSize = 10)
        {
            var accounts = await accountService.GetAllAccount(pageIndex, pageSize);
            return Ok(accounts);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAccountById(int id)
        {
            var account = await accountService.GetAccountById(id);
            if (account == null)
            {
                return NotFound();
            }
            return Ok(account);
        }
        [HttpPost]
        public async Task<IActionResult> AddAccount([FromBody] AccountRequest account)
        {
            var newAccount = await accountService.CreateAccount(account);
            return CreatedAtAction(nameof(GetAccountById), new { id = account.Email }, newAccount);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAccount(int id, [FromBody] AccountRequest account)
        {
            await accountService.UpdateAccount(id, account);
            return Ok(account);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            await accountService.DeleteAccount(id);            
            return NoContent();
        }
    }
}
