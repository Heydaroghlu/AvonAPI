using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avon.Application.Exceptions.BadRequestExceptions.NotFoundExceptions
{
    public class BasketNotFoundException : NotFoundException
    {
        public BasketNotFoundException(string message) : base("Basket Items Not Found")
        {
        }
    }
}
