using AutoMapper;
using Contracts.Models;
using Contracts.Models.Account;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.IdentityModel.Tokens;
using Services.Abstractions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Services;

public sealed class AccountService : IAccountService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly AuthenticationSettings _authenticationSettings;
    private readonly IUserContextService _userContextService;

    public AccountService(IRepositoryManager repositoryManager, IMapper mapper, IPasswordHasher<User> passwordHasher,
        AuthenticationSettings authenticationSettings, IUserContextService userContextService)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
        _passwordHasher = passwordHasher;
        _authenticationSettings = authenticationSettings;
        _userContextService = userContextService;
    }

    public async Task<int> RegisterUser(RegisterUserDto dto)
    {
        var user = _mapper.Map<User>(dto);
        user.Password = _passwordHasher.HashPassword(user, dto.Password);

        await _repositoryManager.AccountsRepository.RegisterUser(user);

        await _repositoryManager.UnitOfWork.SaveChangesAsync();

        return user.Id;
    }

    public async Task<string> GenerateJwt(LoginUserDto dto)
    {
        var user = await _repositoryManager.UsersRepository
            .GetUserByEmailAsync(dto.Email);

        if (user == null)
        {
            throw new BadRequestException("Invalid username or password");
        }

        var result = _passwordHasher.VerifyHashedPassword(user, user.Password, dto.Password);
        if (result == PasswordVerificationResult.Failed)
        {
            throw new BadRequestException("Invalid username or password");
        }

        //create claims
        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
            new Claim(ClaimTypes.Role, user.Role.Name),
            new Claim("Nickname", user.NickName),
        };
        if(user.DateOfBirth != null)
        {
            claims.Add(new Claim(JwtRegisteredClaimNames.Birthdate, user.DateOfBirth?.ToString("yyyy-MM-dd")));
        }

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

    public async Task RemoveUserById(int userId, string password)
    {
        var user = await _repositoryManager.UsersRepository.GetUserByIdAsync(userId);
        if (user is null)
        {
            throw new NotFoundException($"User with id {userId} cannot be found");
        }

        var verifyPassword = _passwordHasher.VerifyHashedPassword(user, user.Password, password);
        if (_userContextService.GetUserRole != "Admin" &&
            (_userContextService.GetUserId != user.Id || 
            (_userContextService.GetUserId == user.Id && verifyPassword == PasswordVerificationResult.Failed))
           )
        {
            throw new ForbidException();
        }

        await _repositoryManager.AccountsRepository.RemoveUserById(userId);

        await _repositoryManager.UnitOfWork.SaveChangesAsync();
    }

    public async Task RestoreUserById(int userId)
    {
        var user = await _repositoryManager.UsersRepository.GetUserByIdAsync(userId, isActive: false);
        if (user is null)
        {
            throw new NotFoundException($"Inactive user with id {userId} cannot be found");
        }

        await _repositoryManager.AccountsRepository.RestoreUserById(userId);

        await _repositoryManager.UnitOfWork.SaveChangesAsync();
    }

    public async Task UpdateAccountPatch(int userId, JsonPatchDocument user)
    {
        var foundUser = await _repositoryManager.UsersRepository.GetUserByIdAsync(userId);
        if (foundUser is null)
        {
            throw new NotFoundException($"Inactive user with id {userId} cannot be found");
        }

        user.ApplyTo(foundUser);

        await _repositoryManager.UnitOfWork.SaveChangesAsync();
    }
}
