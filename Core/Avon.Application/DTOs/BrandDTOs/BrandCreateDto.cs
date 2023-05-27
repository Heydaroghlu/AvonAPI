﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avon.Application.DTOs.BrandDTOs
{
    public class BrandCreateDto
    {

        public string Name { get; set; }
        public IFormFile? ImageFile { get; set; }
    }
}
