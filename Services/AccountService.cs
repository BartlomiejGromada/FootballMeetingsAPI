using AutoMapper;
using Contracts.Models;
using Contracts.Models.Account;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Services.Abstractions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Services;

public class AccountService : IAccountService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly AuthenticationSettings _authenticationSettings;

    public AccountService(IRepositoryManager repositoryManager, IMapper mapper, IPasswordHasher<User> passwordHasher, 
        AuthenticationSettings authenticationSettings)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
        _passwordHasher = passwordHasher;
        _authenticationSettings = authenticationSettings;
    }

    public async Task<int> RegisterUserAsync(RegisterUserDto dto, CancellationToken cancellationToken = default)
    {
        var user = _mapper.Map<User>(dto);
        user.Password = _passwordHasher.HashPassword(user, dto.Password);

        await _repositoryManager.UsersRepository.RegisterUserAsync(user, cancellationToken);

        await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);

        return user.Id;
    }

    public async Task<string> GenerateJwtAsync(LoginUserDto dto, CancellationToken cancellationToken = default)
    {
        var user = await _repositoryManager.UsersRepository
            .GetUserByEmailAsync(dto.Email, cancellationToken);

        if (user == null)
        {
            throw new BadRequestException("Invalid username or password");
        }

        var result = _passwordHasher.VerifyHashedPassword(user, user.Password, dto.Password);
        if(result == PasswordVerificationResult.Failed)
        {
            throw new BadRequestException("Invalid username or password");
        }

        //create claims
        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
            new Claim(ClaimTypes.Role, user.Role.Name),
            new Claim(JwtRegisteredClaimNames.Birthdate, user.DateOfBirth?.ToString("yyyy-MM-dd")),
            new Claim("Nickname", user.NickName),
        };
        
        //create private key
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));

        //generate credentials for signing token
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        //expire date token
        var expireDate = DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays);

        //create token
        var token = new JwtSecurityToken(
            issuer: _authenticationSettings.JwtIssuer,
            audience: _authenticationSettings.JwtAudience,
            claims: claims,
            notBefore: DateTime.Now,
            expires: expireDate,
            signingCredentials: credentials);

        //crate string value from token
        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(token);
    }


}
