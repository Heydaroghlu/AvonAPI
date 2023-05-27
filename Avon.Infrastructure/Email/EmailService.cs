using Avon.Application.Abstractions.Email;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace Avon.Infrastructure.Email
{
    public class EmailService : IEmailService
    {
        IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmail(string to, string subject, string html)
        {
            MimeMessage email = new();
            email.From.Add(MailboxAddress.Parse("Wrish-Shop@yandex.com"));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body=new TextPart(TextFormat.Html) { Text=html};
            ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            {
                return true;
            };
            using SmtpClient smtp = new();
            await smtp.ConnectAsync("smtp.yandex.com", 587, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync("Wrish-Shop@yandex.com", "wrish.p201");
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }

        public async Task SendPasswordResetMailAsync(string to,string userName, string resetToken)
        {
            StringBuilder mail = new StringBuilder();
            mail.AppendLine("Salam<br>Əgər şifrənizi dəyişdirmək istəyirsinizsə, aşağıdakı linkə daxil olun.<br><strong><a target=\"_blank\" href=\"");

            var clientUrl = _configuration.GetSection("ClientUrl:Url").Value;
            mail.AppendLine(clientUrl.TrimEnd('/'));
            mail.AppendLine("/passwordreset/");
            mail.AppendLine(userName);
            mail.AppendLine("/");
            mail.AppendLine(resetToken);
            mail.AppendLine("\">Şifrənizi dəyişmək üçün daxil olun.</a></strong>");

            await SendEmail(to, "Şifrə dəyişdirmə", mail.ToString());

        }
    }
}
