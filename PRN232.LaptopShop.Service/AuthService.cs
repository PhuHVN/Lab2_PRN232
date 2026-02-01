using Microsoft.AspNetCore.Identity.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PRN232.LaptopShop.Repository;
using PRN232.LaptopShop.Repository.Entity;
using PRN232.LaptopShop.Service.DTO;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PRN232.LaptopShop.Service
{
    public class AuthService
    {
 
        private readonly IConfiguration _configuration;
        private readonly UnitOfWork _unitOfWork;
        private readonly AccountService _accountService;

        public AuthService( IConfiguration configuration, UnitOfWork unitOfWork, AccountService accountService)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _accountService = accountService;
        }
        public async Task<string> GenerateJwtToken(Account accounts)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]!);
                var account = await _unitOfWork.accountRepo.FindAsync(x => x.AccountId == accounts.AccountId );
                if (account == null)
                {
                    throw new KeyNotFoundException("Account not found.");
                }
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, accounts.AccountId.ToString()),
                        new Claim(ClaimTypes.Email, accounts.Email),
                        new Claim(ClaimTypes.Role, accounts.Role.ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat,DateTime.UtcNow.ToString()),
                        new Claim(JwtRegisteredClaimNames.Sub, accounts.AccountId.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddMinutes(60),
                    Issuer = _configuration["Jwt:Issuer"],
                    Audience = _configuration["Jwt:Audience"],
                    SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(key),
                        SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);

            }
            catch (Exception ex)
            {
                throw new Exception("Error generating JWT token", ex);
            }
        }

        public async Task<AuthResponse> LoginEmail(AuthRequest request)
        {
            var existingAccount = await _unitOfWork.accountRepo
                .FindAsync(a => a.Email == request.Email);
            if (existingAccount == null)
            {
                throw new UnauthorizedAccessException("Account not found.");
            }
            if (!BCrypt.Net.BCrypt.Verify(request.Password, existingAccount.PasswordHash))
            {
                throw new UnauthorizedAccessException("Invalid password.");
            }
            return new AuthResponse
            {
                Token = await GenerateJwtToken(existingAccount)
            };
        }
        public async Task<AccountResponse> Register(AccountRequest request)
        {
            var account = await _accountService.CreateAccount(request);
            return account;
        }

    }
}
