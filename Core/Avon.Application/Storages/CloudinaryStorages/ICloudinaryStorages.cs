using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avon.Application.Storages.CloudinaryStorages
{
	public interface ICloudinaryStorages : IStorage
	{
        public string GetPublicId(string imageUrl);
    }
}
