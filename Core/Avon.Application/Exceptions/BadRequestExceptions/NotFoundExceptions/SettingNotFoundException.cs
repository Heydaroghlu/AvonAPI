using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avon.Application.Exceptions.BadRequestExceptions.NotFoundExceptions
{
    public class SettingNotFoundException : NotFoundException
    {
        public SettingNotFoundException() : base("Setting Not Found Exception")
        {
        }
    }
}
