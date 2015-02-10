using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CHI_MVCCodeHelper
{
    public class Property
    {
        public string Name { get; set; }
        public string CamelName
        {
            get
            {
                return Name.First().ToString().ToUpper() + String.Join("", Name.Skip(1)).Replace("ID", "Id");

            }
            set { value = CamelName; }
        }

        public string DispayName {
            get
            {
                return Regex.Replace(CamelName, "(\\B[A-Z])", " $1");
            }
        }
        public string Type { get; set; }
        public int Size { get; set; }
        public bool IsNullable { get; set; }
        public bool IsKey { get; set; }
    }
}
