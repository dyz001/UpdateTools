using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdateGen
{
    public class Config
    {
        public class Product
        {
            public String ProductName{get;set;}
            public String BaseVer{get;set;}
            public String BackDir{get;set;}
            public String ProjectDir{get;set;}
            public String ResourceDir{get;set;}
            public String UpdateConfDir { get; set; }
        }
        public class UpdateConfig
        {
            public String engineVersion { get; set; }
            public Dictionary<String, String> groupVersions { get; set; }
            public String packageUrl { get; set; }
            public String remoteManifestUrl { get; set; }
            public String remoteVersionUrl { get; set; }
            public String[] searchPaths { get; set; }
            public String version { get; set; }
        }

        public class ResouceConfig
        {
            public bool compressed { get; set; }
            public String group { get; set; }
            public String md5 { get; set; }
            public String path { get; set; }
        }

        public class UpdateConfigDetail
        {
            public Dictionary<String, ResouceConfig> assets { get; set; }
            public String engineVersion { get; set; }
            public Dictionary<String, String> groupVersions { get; set; }
            public String packageUrl { get; set; }
            public String remoteManifestUrl { get; set; }
            public String remoteVersionUrl { get; set; }
            public String[] searchPaths { get; set; }
            public String version { get; set; }
        }
        const string config_file = "product.conf";
        protected static string md5_filename = "out_md5.txt";
        protected static string product_name = "";
        protected static string base_ver = "";
        protected static string back_dir = "";
        protected static string project_dir = "";
        protected static string resource_dir = "";
        protected static Dictionary<String, Product> product_dic = new Dictionary<string, Product>();
        protected static UpdateConfigDetail androidDetail;
        protected static UpdateConfigDetail iosDetail;
        protected static UpdateConfig androidConfig;
        protected static UpdateConfig iosConfig;
        protected static string androidDetailPath;
        protected static string iosDetailPath;
        protected static string androidConfigPath;
        protected static string iosConfigPath;

        protected static string update_res_key_pre = "update_res_";

        public static string getResKeyPre()
        {
            return update_res_key_pre;
        }

        public static string getVersion()
        {
            return "v1.0.0";
        }

        public static string getMd5File()
        {
            return md5_filename;
        }

        public static bool LoadConfigFile()
        {
            if (!File.Exists(config_file))
            {
                return false;
            }
            string file_content = File.ReadAllText(config_file);
            product_dic = Utils.DeserializeObject<Dictionary<String, Product>>(file_content);
            return true;
        }

        public static bool LoadUpdateConfig(string config_dir)
        {
            string[] files = Directory.GetFiles(config_dir);
            if(files.Length < 4)
            {
                return false;
            }
            foreach(string s in files)
            {
                int idx = s.LastIndexOf(Path.DirectorySeparatorChar);
                string file_name = s.Substring(idx);
                string file_content = File.ReadAllText(s);
                if(file_name.Contains("android"))
                {
                    if(file_name.Contains("detail"))
                    {
                        androidDetail = Utils.DeserializeObject<UpdateConfigDetail>(file_content);
                        androidDetailPath = s;
                    }
                    else
                    {
                        androidConfig = Utils.DeserializeObject<UpdateConfig>(file_content);
                        androidConfigPath = s;
                    }
                }
                else if(file_name.Contains("ios"))
                {
                    if (file_name.Contains("detail"))
                    {
                        iosDetail = Utils.DeserializeObject<UpdateConfigDetail>(file_content);
                        iosDetailPath = s;
                    }
                    else
                    {
                        iosConfig = Utils.DeserializeObject<UpdateConfig>(file_content);
                        iosConfigPath = s;
                    }
                }
            }
            return true;
        }

        public static void SaveToFile()
        {
            string obj_str = Utils.SerializeObject(product_dic);
            File.WriteAllText(config_file, obj_str);
        }

        /// <summary>
        /// 更新配置文件，返回最大版本号对应的组Id
        /// </summary>
        /// <param name="resMd5"></param>
        /// <param name="path"></param>
        /// <param name="resName"></param>
        /// <returns></returns>
        public static int UpdateConfigItem(string resMd5, string path, string resName, string version_no, string core_version)
        {
            FormUpdate.GetInstance().appendLog("save config " + path + ", version_no:" + version_no);
            int default_group = 1;
            foreach(string key in androidConfig.groupVersions.Keys)
            {
                if(int.Parse(key) > default_group)
                {
                    default_group = int.Parse(key);
                }
            }
            default_group += 1;
            ResouceConfig resConf = new ResouceConfig();
            resConf.compressed = true;
            resConf.group = (default_group.ToString());
            resConf.md5 = resMd5;
            resConf.path = path;

            //android update
            FormUpdate.GetInstance().appendLog("android update");
            androidConfig.groupVersions.Add(default_group.ToString(), version_no);
            androidConfig.version = String.IsNullOrEmpty(core_version) ? androidConfig.version : core_version;
            androidDetail.assets.Clear();
            androidDetail.assets.Add(resName, resConf);
            if (androidDetail.groupVersions.ContainsKey(default_group.ToString()))
            {
                FormUpdate.GetInstance().appendLog("android detail version error, key exist:" + default_group);
            }
            else
            {
                androidDetail.groupVersions.Add(default_group.ToString(), version_no);
            }

            //ios update
            FormUpdate.GetInstance().appendLog("ios update");
            if (iosConfig.groupVersions.ContainsKey(default_group.ToString()))
            {
                FormUpdate.GetInstance().appendLog("key exist in ios:" + default_group.ToString() + "\r\n");
            }
            else
            {
                iosConfig.groupVersions.Add(default_group.ToString(), version_no);
            }
            iosConfig.version = String.IsNullOrEmpty(core_version) ? androidConfig.version : core_version;
            iosDetail.assets.Clear();
            iosDetail.assets.Add(resName, resConf);
            if (!iosDetail.groupVersions.ContainsKey(default_group.ToString()))
            {
                iosDetail.groupVersions.Add(default_group.ToString(), version_no);
            }
            else
            {
                FormUpdate.GetInstance().appendLog("ios detail info error, key exist:" + default_group);
            }
            return default_group;
        }

        public static void saveUpdateConfig()
        {
            FormUpdate.GetInstance().appendLog("saveUpdateConfig:" + Utils.SerializeObject(androidConfig));
            if(!string.IsNullOrEmpty(androidConfigPath))
            {
                File.WriteAllText(androidConfigPath, Utils.SerializeObject(androidConfig));
            }
            if(!string.IsNullOrEmpty(androidDetailPath))
            {
                File.WriteAllText(androidDetailPath, Utils.SerializeObject(androidDetail));
            }
            if(!string.IsNullOrEmpty(iosConfigPath))
            {
                File.WriteAllText(iosConfigPath, Utils.SerializeObject(iosConfig));
            }
            if(!string.IsNullOrEmpty(iosDetailPath))
            {
                File.WriteAllText(iosDetailPath, Utils.SerializeObject(iosDetail));
            }
        }

        public static void AddProduct(Product p)
        {
            if (p == null) return;
            if(product_dic.ContainsKey(p.ProductName))
            {
                product_dic[p.ProductName] = p;
            }
            else
            {
                product_dic.Add(p.ProductName, p);
            }
        }

        public static Product GetProductByName(String name)
        {
            if (product_dic.ContainsKey(name))
            {
                return product_dic[name];
            }   
            else return null;
        }

        public static Dictionary<String, Product>.KeyCollection GetProductKeys()
        {
            return product_dic.Keys;
        }
    }
}
