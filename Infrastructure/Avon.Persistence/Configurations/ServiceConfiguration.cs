using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avon.Persistence.Configurations
{
    public class ServiceConfiguration
    {

        public static string MSSQL
        {
            get
            {
                ConfigurationManager configurationManager = ServiceConfiguration.SqlConfiguration();
				return configurationManager.GetConnectionString("MSSQL");
            }
        }

		public static string MySQL
		{
			get
			{
				ConfigurationManager configurationManager = ServiceConfiguration.SqlConfiguration();
				return configurationManager.GetConnectionString("MySQL");
			}
		}

		public static ConfigurationManager SqlConfiguration()
        {
			ConfigurationManager configurationManager = new ConfigurationManager();
			configurationManager.SetBasePath(Path.Combine(Directory.GetCurrentDirectory()));
			configurationManager.AddJsonFile("appsettings.json");

            return configurationManager;
		}

    }
}
