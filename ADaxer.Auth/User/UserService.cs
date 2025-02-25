﻿using Microsoft.AspNetCore.Identity;
using ADaxer.Auth.Helpers;
using System.Diagnostics;

namespace ADaxer.Auth.User;

public interface IUserService
{
  Task<(bool success, string accessToken, string refreshToken)> TryLogin(UserLoginData login);
  Task<bool> TryRegister(UserLoginData login, params string[] initialRoles);
}

public class UserService : IUserService
{
  private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ConfigureUserDbOptions _options;

    public UserService(SignInManager<ApplicationUser> signInManager, ConfigureUserDbOptions options)
  {
    _signInManager = signInManager;
        _options = options;
    }

  public async Task<(bool, string, string)> TryLogin(UserLoginData login)
  {
    try
    {
      var signInResult = await _signInManager.PasswordSignInAsync(login.UserName!, login.Password!, false, lockoutOnFailure: false);
      if (signInResult.Succeeded)
      {
        var user = await _signInManager.UserManager.FindByNameAsync(login.UserName!);
        var roles = await _signInManager.UserManager.GetRolesAsync(user!);
        var accessToken = JwtHelper.GenerateJwtToken(user!, roles, _options);
        return (true, accessToken, accessToken);
      }
    }
    catch (Exception ex)
    {
      Trace.TraceError($"Error logging in {login.UserName} / {login.Email}: {ex}");
    }
    return (false, string.Empty, string.Empty);
  }

  public async Task<bool> TryRegister(UserLoginData data, params string[] initialRoles)
  {
    var user = new ApplicationUser { UserName = string.IsNullOrEmpty(data.UserName) ? data.Email : data.UserName, Email = data.Email };
    var result = await _signInManager.UserManager.CreateAsync(user, data.Password);
    if (result.Succeeded)
    {
      await _signInManager.UserManager.AddToRolesAsync(user, initialRoles.Count() == 0 ? new List<string> { "User" } : initialRoles);
      return true;
    }
    return false;
  }
}
