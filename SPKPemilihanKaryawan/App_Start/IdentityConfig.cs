using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using SPKPemilihanKaryawan.Models;
using SistemPendukungKeputusan.DAL;
using SistemPendukungKeputusan.Models.Security;

namespace SPKPemilihanKaryawan
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your email service here to send an email.
            return Task.FromResult(0);
        }
    }

    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }

    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    public class SistemPendukungKeputusanUserManager : UserManager<SistemPendukungKeputusanApplicationUser>
    {
        public SistemPendukungKeputusanUserManager(IUserStore<SistemPendukungKeputusanApplicationUser> store)
            : base(store)
        {
        }

        public static SistemPendukungKeputusanUserManager Create(IdentityFactoryOptions<SistemPendukungKeputusanUserManager> options, IOwinContext context) 
        {
           var manager = new SistemPendukungKeputusanUserManager(new UserStore<SistemPendukungKeputusanApplicationUser>(context.Get<SPKContext>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<SistemPendukungKeputusanApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<SistemPendukungKeputusanApplicationUser>
            {
                MessageFormat = "Your login security PIN is {0}"
            });
            manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<SistemPendukungKeputusanApplicationUser>
            {
                Subject = "Security Code",
                BodyFormat = "Your login security PIN is {0}"
            });

            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();

            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<SistemPendukungKeputusanApplicationUser>(dataProtectionProvider.Create("SistemPendukungKeputusanApplicationUser"));
            }
            return manager;
        }
    }

    // Configure the application sign-in manager which is used in this application.
    public class SistemPendukungKeputusanSignInManager : SignInManager<SistemPendukungKeputusanApplicationUser, string>
    {
        public SistemPendukungKeputusanSignInManager(SistemPendukungKeputusanUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(SistemPendukungKeputusanApplicationUser user)
        {
            return user.GenerateUserIdentityAsync((SistemPendukungKeputusanUserManager)UserManager);
        }

        public static SistemPendukungKeputusanSignInManager Create(IdentityFactoryOptions<SistemPendukungKeputusanSignInManager> options, IOwinContext context)
        {
            return new SistemPendukungKeputusanSignInManager(context.GetUserManager<SistemPendukungKeputusanUserManager>(), context.Authentication);
        }
    }
}
