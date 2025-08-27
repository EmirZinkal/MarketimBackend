using Business.Abstract;
using Business.Constants;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using Core.Utilities.Security.Hashing;
using Core.Utilities.Security.Jwt;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using Entities.Dtos;
using Entities.Dtos.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class AuthManager : IAuthService
    {
        private readonly IUserService _userService;
        private readonly ITokenHelper _tokenHelper;
        private readonly IPasswordResetTokenDal _passwordResetTokenDal;
        private readonly IUserOperationClaimDal _userOperationClaimDal;


        public AuthManager(IUserService userService, ITokenHelper tokenHelper, IPasswordResetTokenDal passwordResetTokenDal, IUserOperationClaimDal userOperationClaimDal)
        {
            _userService = userService;
            _tokenHelper = tokenHelper;
            _passwordResetTokenDal = passwordResetTokenDal;
            _userOperationClaimDal = userOperationClaimDal;
        }

        public IDataResult<User> Register(UserForRegisterDto userForRegisterDto, string password)
        {
            // Şifreyi hashle
            HashingHelper.CreatePasswordHash(password, out byte[] passwordHash);

            var user = new User
            {
                Email = userForRegisterDto.Email,
                FullName = $"{userForRegisterDto.FirstName} {userForRegisterDto.LastName}",
                PasswordHash = Convert.ToBase64String(passwordHash),
                CreatedAt = DateTime.Now
            };

            _userService.Add(user);

            var memberRoleId = 2; // OperationClaims tablosunda Member rolünün ID'si
            _userOperationClaimDal.Add(new UserOperationClaim
            {
                UserId = user.Id,
                RoleId = memberRoleId
            });
            return new SuccessDataResult<User>(user, Messages.UserRegistered);
        }

        public IDataResult<User> Login(UserForLoginDto userForLoginDto)
        {
            var userToCheck = _userService.GetByMail(userForLoginDto.Email);
            if (userToCheck.Data == null)
                return new ErrorDataResult<User>(Messages.UserNotFound);

            var passwordHash = Convert.FromBase64String(userToCheck.Data.PasswordHash);
            var passwordMatch = HashingHelper.VerifyPasswordHash(userForLoginDto.Password, passwordHash);

            if (!passwordMatch)
                return new ErrorDataResult<User>(Messages.PasswordError);

            return new SuccessDataResult<User>(userToCheck.Data, Messages.SuccessfulLogin);
        }

        public IResult UserExists(string email)
        {
            var user = _userService.GetByMail(email);
            if (user.Data != null)
                return new ErrorResult(Messages.UserAlreadyExists);

            return new SuccessResult();
        }

        public IDataResult<AccessToken> CreateAccessToken(User user)
        {
            var claims = _userService.GetClaims(user).Data;
            var accessToken = _tokenHelper.CreateToken(user, claims);
            return new SuccessDataResult<AccessToken>(accessToken, Messages.AccessTokenCreated);
        }

        public IResult SendPasswordResetEmail(ForgotPasswordRequestDto request)
        {
            var userResult = _userService.GetByMail(request.Email);
            if (!userResult.Success || userResult.Data == null)
                return new ErrorResult(Messages.UserNotFound);

            // Token üret
            var token = Guid.NewGuid().ToString("N");

            var resetToken = new PasswordResetToken
            {
                UserId = userResult.Data.Id,
                Token = token,
                ExpireDate = DateTime.Now.AddMinutes(15),
                IsUsed = false
            };

            _passwordResetTokenDal.Add(resetToken);

            // Link oluştur
            var resetLink = $"https://seninfrontendadresin.com/reset-password?token={token}";

            // Mail gönder
            SendEmail(request.Email, "Şifre Sıfırlama",
                $"Merhaba {userResult.Data.FullName},\n\n" +
                $"Şifrenizi sıfırlamak için aşağıdaki bağlantıya tıklayın:\n{resetLink}\n\n" +
                $"Bu bağlantı 15 dakika boyunca geçerlidir.");

            return new SuccessResult(Messages.PasswordResetEmailSent);
        }

        public IResult ResetPassword(ResetPasswordRequestDto request)
        {
            var tokenEntity = _passwordResetTokenDal
                .Get(t => t.Token == request.Token && !t.IsUsed && t.ExpireDate > DateTime.Now);

            if (tokenEntity == null)
                return new ErrorResult(Messages.InvalidOrExpiredToken);

            var userResult = _userService.GetById(tokenEntity.UserId);
            if (!userResult.Success || userResult.Data == null)
                return new ErrorResult(Messages.UserNotFound);

            // Şifre hashle (senin helper’a göre)
            HashingHelper.CreatePasswordHash(request.NewPassword, out byte[] passwordHash);
            userResult.Data.PasswordHash = Convert.ToBase64String(passwordHash);

            _userService.Update(userResult.Data);

            // Token’i kullanıldı olarak işaretle
            tokenEntity.IsUsed = true;
            _passwordResetTokenDal.Update(tokenEntity);

            return new SuccessResult(Messages.PasswordChanged);
        }

        private void SendEmail(string to, string subject, string body)
        {
            using (var client = new SmtpClient("smtp.gmail.com", 587))
            {
                client.Credentials = new System.Net.NetworkCredential(
                    "emir.zinkal.34@gmail.com",
                    "şifre"
                );
                client.EnableSsl = true;
                var mailMessage = new MailMessage("emir.zinkal.34@gmail.com", to, subject, body);
                client.Send(mailMessage);
            }
        }
    }
}
