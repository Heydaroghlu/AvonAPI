﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avon.Application.HelperManager
{
	public sealed class CodeGeneratorManager
	{
		public static string Generate(string name, string surname)
		{
			StringBuilder codeBuilder = new StringBuilder();
			codeBuilder.Append(char.ToUpper(name[0]));
			codeBuilder.Append(char.ToUpper(surname[0]));

			Random random = new Random();
			string randomNumber = random.Next(1000, 9999).ToString();

			codeBuilder.Append(randomNumber);
			return codeBuilder.ToString();
		}
	}
}
