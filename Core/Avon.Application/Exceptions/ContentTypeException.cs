using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avon.Application.Exceptions
{
    public class ContentTypeException : Exception
    {

        public ContentTypeException(string msg) : base(msg)
        {

        }

    }
}
