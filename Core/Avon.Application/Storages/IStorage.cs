﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avon.Application.Storages
{
	public interface IStorage
	{
        Task<List<(string fileName, string pathOrContainerName)>> UploadRangeAsync(string pathOrContainerName, List<IFormFile> files);
		Task<(string fileName, string pathOrContainerName)> UploadAsync(string pathOrContainerName, IFormFile file);
		Task<bool> DeleteAsync(string pathOrContainerName, string fileName);
		bool HasFile(string pathOrContainerName, string fileName);
		List<string> FileNames(string path, string ceoFriendlyName);
		string GetUrl(string pathOrContainerName, string fileName);
    }
}
