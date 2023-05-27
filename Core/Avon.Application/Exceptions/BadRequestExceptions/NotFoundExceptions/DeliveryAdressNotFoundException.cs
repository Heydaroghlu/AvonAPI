using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avon.Application.Exceptions.BadRequestExceptions.NotFoundExceptions
{
    public class DeliveryAdressNotFoundException : NotFoundException
    {
        public DeliveryAdressNotFoundException() : base("Delivery Adress Not Found")
        {
        }
    }
}
