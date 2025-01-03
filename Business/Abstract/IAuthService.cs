﻿using Core.Entities.Concrete;
using Core.Utilities.Results;
using Core.Utilities.Security.JWT;
using Entities.DTOs;

namespace Business.Abstract
{
    public interface IAuthService
    {
        IDataResult<Users> Register(UserForRegisterDto userForRegisterDto, string password);

        IDataResult<Users> Login(UserForLoginDto userForLoginDto);

        IResult UserExists(string email);

        IDataResult<AccessToken> CreateAccessToken(Users user);

        IResult RequestPasswordReset(PasswordResetRequestDto passwordResetRequestDto);

        IResult ResetPassword(PasswordResetDto passwordResetDto);
    }
}