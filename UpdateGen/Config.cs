using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdateGen
{
    public class Config
    {
        protected static string md5_filename = "out_md5.txt";
        protected static string product_name = "";
        protected static string base_ver = "";
        protected static string back_dir = "";
        protected static string project_dir = "";
        protected static string resource_dir = "";
        public static string getMd5File()
        {
            return md5_filename;
        }
    }
}
