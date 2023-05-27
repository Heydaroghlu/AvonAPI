using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Avon.Application.HelperManager
{
    public sealed class SlugManager
    {
        public static string GenerateSlug(string phrase) { 
            
            string str=phrase.ToLower();
            str = Regex.Replace(str, @"[^a-z0-9\s-]", ""); //ancaq hərf və rəqəmi saxlayır
            str = Regex.Replace(str, @"\s+", " ").Trim(); //boşluq uzunluqlarını tək eləyir
            str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim(); //slug uzunluğu maks 45 ola bilər
            str = Regex.Replace(str, @"\s", "-"); //boşluqları tire ilə əvəz edir
            return str;
        }
    }
}
