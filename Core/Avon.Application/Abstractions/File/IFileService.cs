using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avon.Application.Abstractions.File
{
    public interface IFileService
    {
        public void CheckFileType(IFormFile file, List<string> contentTypes);
    }
}
