using Business.Abstract;
using Business.Constants;
using Core.Entities.Concrete;
using Core.Utilities.Helpers;
using Core.Utilities.Results;
using Core.Utilities.Security.Hashing;
using Core.Utilities.Security.JWT;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;

namespace Business.Concrete
{
    public class AuthManager : IAuthService
    {
        private IUserService _userService;
        private ITokenHelper _tokenHelper;
        private readonly IPasswordResetDal _passwordResetDal;

        public AuthManager(IUserService userService, ITokenHelper tokenHelper, IPasswordResetDal passwordResetDal)
        {
            _userService = userService;
            _tokenHelper = tokenHelper;
            _passwordResetDal = passwordResetDal;
        }

        public IDataResult<Users> Register(UserForRegisterDto userForRegisterDto, string password)
        {
            Console.WriteLine("girdi");
            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(password, out passwordHash, out passwordSalt);
            var user = new Users
            {
                Email = userForRegisterDto.Email,
                FirstName = userForRegisterDto.FirstName,
                LastName = userForRegisterDto.LastName,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                City = userForRegisterDto.City,
                District = userForRegisterDto.District,
                Adress = userForRegisterDto.Adress,
                Cinsiyet = userForRegisterDto.Cinsiyet,
                Status = true,
                Roles = [4],
            };
            _userService.Add(user);
            return new SuccessDataResult<Users>(user, Messages.UserRegistered);
        }

        public IDataResult<Users> Login(UserForLoginDto userForLoginDto)
        {
            var userToCheck = _userService.GetByMail(userForLoginDto.Email);
            if (userToCheck == null)
            {
                return new ErrorDataResult<Users>(Messages.UserNotFound);
            }
            if (userToCheck.Status == false)
            {
                return new ErrorDataResult<Users>(Messages.UserNotFound);
            }
            if (!HashingHelper.VerifyPasswordHash(userForLoginDto.Password, userToCheck.PasswordHash, userToCheck.PasswordSalt))
            {
                return new ErrorDataResult<Users>(Messages.PasswordError);
            }

            return new SuccessDataResult<Users>(userToCheck, Messages.SuccessfulLogin);
        }

        public IResult UserExists(string email)
        {
            if (_userService.GetByMail(email) != null)
            {
                return new ErrorResult(Messages.UserAlreadyExists);
            }
            return new SuccessResult();
        }

        public IDataResult<AccessToken> CreateAccessToken(Users user)
        {
            var claims = _userService.GetClaims(user);
            var accessToken = _tokenHelper.CreateToken(user, claims);
            return new SuccessDataResult<AccessToken>(accessToken, Messages.AccessTokenCreated);
        }

        public IResult RequestPasswordReset(PasswordResetRequestDto passwordResetRequestDto)
        {
            var user = _userService.GetByMail(passwordResetRequestDto.Email);
            if (user == null)
                return new ErrorResult(Messages.UserNotFound);

            var token = TokenGenerator.GenerateToken();
            var expirationTime = DateTime.Now.AddMinutes(10);

            var passwordReset = new PasswordResetRequest
            {
                UserId = user.Id,
                ResetToken = token,
                ExpirationTime = expirationTime,
                Status = false
            };

            _passwordResetDal.Add(passwordReset);

            // E-posta gönderme işlemi
            var result = EmailHelper.SendPasswordResetEmail(user.Email, token);

            return new SuccessResult(result.Message);
        }

        public IResult ResetPassword(PasswordResetDto passwordResetDto)
        {
            var resetEntry = _passwordResetDal.Get(p => p.ResetToken == passwordResetDto.ResetToken && !p.Status);

            if (resetEntry == null || resetEntry.ExpirationTime < DateTime.Now)
                return new ErrorResult(Messages.InvalidOrExpiredToken);

            var user = _userService.GetById(resetEntry.UserId);
            if (user == null)
                return new ErrorResult(Messages.UserNotFound);

            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(passwordResetDto.NewPassword, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            _userService.Update(user);

            resetEntry.Status = true;
            _passwordResetDal.Update(resetEntry);

            return new SuccessResult(Messages.PasswordResetSuccessful);
        }
    }
}