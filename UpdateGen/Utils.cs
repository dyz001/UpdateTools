using System;
using System.Security.Cryptography; 
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using ICSharpCode.SharpZipLib.Zip;

namespace UpdateGen
{
    public class Utils
    {
        public static bool CopyResSrc(String proj_dir, String target_dir, bool isCompileSrc)
        {
            bool ret = true;
            //copy res
            String res_src = Path.Combine(proj_dir, "res");
            String res_target = Path.Combine(target_dir, "res");
            String src_src = Path.Combine(proj_dir, "src");
            String src_target = Path.Combine(target_dir, "src");
            CopyDir(res_src, res_target);
            Process proc_test = new Process();
            if (isCompileSrc)
            {
                proc_test.StartInfo.CreateNoWindow = true;
                proc_test.StartInfo.FileName = "cmd.exe";
                proc_test.StartInfo.UseShellExecute = false;
                proc_test.StartInfo.RedirectStandardError = true;
                proc_test.StartInfo.RedirectStandardInput = true;
                proc_test.StartInfo.RedirectStandardOutput = true;
                proc_test.Start();
                proc_test.StandardInput.WriteLine("cocos jscompile -s " + src_src + " -d " + src_target);
                proc_test.StandardInput.WriteLine("exit");
                string line = null;
                line = proc_test.StandardOutput.ReadLine();
                while (line != null)
                {
                    Log(line);
                }
                proc_test.Close();
            }
            else
            {
                CopyDir(src_src, src_target);
            }
            return ret;
        }

        public static void CopyDir(String src_dir, String target_dir)
        {
            if (!Directory.Exists(target_dir)) 
            {
                Directory.CreateDirectory(target_dir);
            }

            if(!Directory.Exists(src_dir))
            {
                Log("Cant find src dir:" + src_dir);
                return;
            }

            String[] file_paths = Directory.GetFiles(src_dir);

            foreach(String path in file_paths)
            {
                String target_path = Path.Combine(target_dir, path.Substring(path.LastIndexOf(Path.PathSeparator) + 1));
                if (File.Exists(target_path))
                {
                    Log("Target File Exist, jump file:" + target_path);
                    continue;
                }
                File.Copy(path, Path.Combine(target_dir, path.Substring(path.LastIndexOf(Path.PathSeparator) + 1)));
            }

            String[] dir_infos = Directory.GetDirectories(src_dir);
            for (int i = 0; i < dir_infos.Length; ++i) 
            {   
                CopyDir(dir_infos[i], System.IO.Path.Combine(target_dir,
                    dir_infos[i].Substring(dir_infos[i].LastIndexOf(System.IO.Path.DirectorySeparatorChar) + 1)));
            }
        }

        public static void GenResMd5(String package_dir, StreamWriter md5_out_file, String parent)
        {
            String[] directories = Directory.GetDirectories(package_dir);

            String[] files = Directory.GetFiles(package_dir);
            foreach(String source in files)
            {
                MD5 md5 = MD5.Create();
                byte[] result = md5.ComputeHash(File.ReadAllBytes(source));
                StringBuilder strbul = new StringBuilder(40);
                for (int i = 0; i < result.Length; i++)
                {
                    strbul.Append(result[i].ToString("x2"));//加密结果"x2"结果为32位,"x3"结果为48位,"x4"结果为64位

                }
                md5_out_file.WriteLine(source.Substring(source.IndexOf(parent) + parent.Length + 1) + ":" + strbul.ToString());
            }

            foreach(String dir in directories)
            {
                GenResMd5(dir, md5_out_file, parent);
            }
        }

        public static void OutDiff(string base_dir, string cur_dir, string package_out_dir, string ver)
        {
            string base_md5_file = Path.Combine(base_dir, Config.getMd5File());
            string cur_md5_file = Path.Combine(cur_dir, Config.getMd5File());
            if(!File.Exists(base_md5_file))
            {
                FormUpdate.GetInstance().appendLog("base md5 not found, begin gen");
                File.Create(base_md5_file);
                StreamWriter sw = new StreamWriter(base_md5_file);
                GenResMd5(base_dir, sw, base_dir);
                sw.Close();
            }

            if (!File.Exists(cur_md5_file))
            {
                FormUpdate.GetInstance().appendLog("base md5 not found, begin gen");
                File.Create(cur_md5_file);
                StreamWriter sw = new StreamWriter(cur_md5_file);
                GenResMd5(cur_md5_file, sw, cur_dir);
                sw.Close();
            }

            //读取文件
            Dictionary<String, String> base_md5_dic = new Dictionary<string, string>();
            Dictionary<String, String> cur_md5_dic = new Dictionary<string, string>();

            
            string[] line = File.ReadAllLines(base_md5_file);
            foreach(string t in line)
            {
                string[] md5_file = t.Split(':');
                if(md5_file.Length != 2)
                {
                    FormUpdate.GetInstance().appendLog("base dir get file key error:" + t);
                    continue;
                }
                base_md5_dic.Add(md5_file[0], md5_file[1]);
            }

            line = File.ReadAllLines(cur_md5_file);
            foreach (string t in line)
            {
                string[] md5_file = t.Split(':');
                if (md5_file.Length != 2)
                {
                    FormUpdate.GetInstance().appendLog("cur dir get file key error:" + t);
                    continue;
                }
                cur_md5_dic.Add(md5_file[0], md5_file[1]);
            }
            Dictionary<String, String> diff_dic = new Dictionary<string, string>();
            //get diff
            foreach(string key in cur_md5_dic.Keys)
            {
                if(!base_md5_dic.ContainsKey(key))
                {
                    diff_dic.Add(key, cur_md5_dic[key]);
                }
                if(base_md5_dic[key].CompareTo(cur_md5_dic[key]) != 0)
                {
                    diff_dic.Add(key, cur_md5_dic[key]);
                }
            }

            //copy to package out
            string tmpDir = Path.Combine(package_out_dir.Substring(0, package_out_dir.LastIndexOf(Path.PathSeparator)), "temp");
            if(Directory.Exists(tmpDir))
            {
                Directory.Delete(tmpDir, true);
            }
            Directory.CreateDirectory(tmpDir);

            foreach(String key in diff_dic.Keys)
            {
                File.Copy(Path.Combine(cur_dir, key), Path.Combine(tmpDir, key));
            }

            FastZip zip = new FastZip();
            zip.CreateZip(Path.Combine(package_out_dir, "update_" + ver + ".zip"), tmpDir, true, "");
        }

        public static void Log(String s)
        {
            FormUpdate.GetInstance().appendLog(s + "\r\n");
        }
    }
}
