using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avon.Application.Abstractions.Email
{
    public interface IEmailService
    {
        Task SendEmail(string to,string subject,string html);
        Task SendPasswordResetMailAsync(string to,string userName, string resetToken);
    
    }
}
