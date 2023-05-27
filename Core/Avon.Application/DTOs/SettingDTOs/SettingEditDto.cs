﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avon.Application.DTOs.SettingDTOs
{
    public class SettingEditDto
    {
        public string? Value { get; set; }
        public IFormFile? FormFile { get; set; }
    }
}
