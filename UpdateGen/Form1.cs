using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using System.Threading;
using System.Diagnostics;

namespace UpdateGen
{
    public partial class FormUpdate : Form
    {
        protected Task<bool> copy_task;
        protected Task<bool> gen_md5_task;
        protected Task<string> out_package_task;
        protected StringBuilder m_log = new StringBuilder();
        protected Process proc_compile = new Process();
        delegate void AppendLogCallback(string log);
        private static FormUpdate instance;
        public FormUpdate()
        {
            InitializeComponent();
            if(instance == null)
            {
                instance = this;
            }
            lbl_declare.Text += Config.getVersion();
            cb_compile_code.Checked = true;
            loadConfig();
        }

        protected void loadConfig()
        {
            Config.LoadConfigFile();
            Dictionary<String, Config.Product>.KeyCollection keyCollections = Config.GetProductKeys();
            foreach(string s in keyCollections)
            {
                cmb_product.Items.Add(s);
            }
        }

        public static FormUpdate GetInstance()
        {
            return instance;
        }

        private void btn_sel_bk_dir_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog P_File_Folder = new FolderBrowserDialog();
            if(!String.IsNullOrEmpty(txt_bk_dir.Text))
            {
                P_File_Folder.SelectedPath = txt_bk_dir.Text;
            }
            DialogResult result = P_File_Folder.ShowDialog();
            if(result == System.Windows.Forms.DialogResult.OK)
            {
                txt_bk_dir.Text = P_File_Folder.SelectedPath;
            }
        }

        private void btn_diff_gen_Click(object sender, EventArgs e)
        {
            Config.LoadUpdateConfig(txt_hotupdate_conf.Text);
            //拷贝资源到目标目录
            copy_task = new Task<bool>(n => CopyResSrc((FormUpdate)n), this);
            copy_task.Start();
            gen_md5_task = copy_task.ContinueWith(task =>
                {
                    return GenMd5(this);
                });
            if (!string.IsNullOrEmpty(txt_online_ver.Text))
            {
                //输出版本差异
                out_package_task = gen_md5_task.ContinueWith(task =>
                {
                    return OutPackage(this);
                });
                out_package_task.ContinueWith(task =>
                {
                    string file_path = out_package_task.Result;
                    string res_md5 = Utils.GetMd5(file_path);
                    string file_name = file_path.Substring(file_path.LastIndexOf(Path.DirectorySeparatorChar) + 1);
                    Config.UpdateConfigItem(res_md5, file_name, Config.getResKeyPre() + this.GetOutVer(), this.GetOutVer(), this.getCoreVersion());
                    Config.saveUpdateConfig();
                });
            }
        }

        public String getCoreVersion()
        {
            return txt_core_ver.Text;
        }

        public String getProjectDir()
        {
            return txt_proj_dir.Text;
        }

        public String getBKDir()
        {
            return Path.Combine(txt_bk_dir.Text, txt_product_name.Text + "_" + txt_out_ver.Text);
        }

        public String getBaseDir()
        {
            return Path.Combine(txt_bk_dir.Text, txt_product_name.Text + "_" + txt_online_ver.Text);
        }

        public bool isCompileSource()
        {
            return cb_compile_code.Checked;
        }

        public void appendLog(String log)
        {
            if(this.txt_log.InvokeRequired)
            {
                AppendLogCallback cb = new AppendLogCallback(appendLog);
                this.Invoke(cb, new object[] { log });
            }
            else
            {
                this.txt_log.Text += log;
                this.txt_log.SelectionStart = this.txt_log.Text.Length;
                this.txt_log.ScrollToCaret();
            }
        }

        protected static bool CopyResSrc(FormUpdate update_form)
        {
            update_form.appendLog("资源拷贝中");
            Utils.CopyResSrc(update_form.getProjectDir(), update_form.getBKDir(), update_form.isCompileSource());
            return true;
        }

        protected static bool GenMd5(FormUpdate update_form)
        {
            bool ret = true;
            FileStream fs;
            string md5File = Path.Combine(update_form.getBKDir(), Config.getMd5File());
            if (!File.Exists(md5File))
            {
                fs = File.Create(md5File);
            }
            else
            {
                fs = new FileStream(md5File, FileMode.Truncate);
            }
            StreamWriter sw = new StreamWriter(fs);
            Utils.GenResMd5(update_form.getBKDir(), sw, update_form.getBKDir());
            sw.Flush();
            sw.Close();
            return ret;
        }

        public string GetOutVer()
        {
            return txt_out_ver.Text;
        }

        protected static string OutPackage(FormUpdate update_form)
        {
            return Utils.OutDiff(update_form.getBaseDir(), update_form.getBKDir(), update_form.getPackageOutDir(), update_form.GetOutVer());
        }        

        public void RunProgram(string programName, string cmd)
        {
            Process proc = new Process();
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.FileName = programName;
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardError = true;
            proc.StartInfo.RedirectStandardInput = true;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.Start();
            if (cmd.Length != 0)
            {
                proc.StandardInput.WriteLine(cmd);
            }
            proc.Close();
        }
        public static string RunCmd(string cmd)
        {
            Process proc_test = new Process();
            proc_test.StartInfo.CreateNoWindow = true;
            proc_test.StartInfo.FileName = "cmd.exe";
            proc_test.StartInfo.UseShellExecute = false;
            proc_test.StartInfo.RedirectStandardError = true;
            proc_test.StartInfo.RedirectStandardInput = true;
            proc_test.StartInfo.RedirectStandardOutput = true;
            proc_test.Start();
            proc_test.StandardInput.WriteLine(cmd);
            proc_test.StandardInput.WriteLine("exit");
            //string outStr = proc_test.StandardOutput.ReadToEnd();
            proc_test.Close();
            return "";
        }

        public string getPackageOutDir()
        {
            return txt_res_dir.Text;
        }

        private void btn_open_res_Click(object sender, EventArgs e)
        {
            RunCmd("explorer " + getPackageOutDir());
        }

        private void btn_open_conf_Click(object sender, EventArgs e)
        {
            RunCmd("explorer " + getConfigDir());
        }
        
        public string getConfigDir()
        {
            return txt_hotupdate_conf.Text;
        }

        private void cmb_product_SelectedIndexChanged(object sender, EventArgs e)
        {
            string product_name = cmb_product.SelectedItem.ToString();
            Config.Product product = Config.GetProductByName(product_name);
            txt_hotupdate_conf.Text = product.UpdateConfDir;
            txt_bk_dir.Text = product.BackDir;
            txt_online_ver.Text = product.BaseVer;
            txt_product_name.Text = product_name;
            txt_proj_dir.Text = product.ProjectDir;
            txt_res_dir.Text = product.ResourceDir;
        }

        private void btn_sel_proj_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog P_File_Folder = new FolderBrowserDialog();
            if (!String.IsNullOrEmpty(txt_bk_dir.Text))
            {
                P_File_Folder.SelectedPath = txt_proj_dir.Text;
            }
            DialogResult result = P_File_Folder.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                txt_proj_dir.Text = P_File_Folder.SelectedPath;
            }
        }

        private void btn_sel_res_dir_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog P_File_Folder = new FolderBrowserDialog();
            if (!String.IsNullOrEmpty(txt_bk_dir.Text))
            {
                P_File_Folder.SelectedPath = txt_res_dir.Text;
            }
            DialogResult result = P_File_Folder.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                txt_res_dir.Text = P_File_Folder.SelectedPath;
            }
        }

        private void btn_sel_hotupdate_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog P_File_Folder = new FolderBrowserDialog();
            if (!String.IsNullOrEmpty(txt_bk_dir.Text))
            {
                P_File_Folder.SelectedPath = txt_hotupdate_conf.Text;
            }
            DialogResult result = P_File_Folder.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                txt_hotupdate_conf.Text = P_File_Folder.SelectedPath;
            }
        }

        private void btn_save_ver_Click(object sender, EventArgs e)
        {
            Config.Product product = new Config.Product();
            product.ProductName = txt_product_name.Text;
            product.ProjectDir = txt_proj_dir.Text;
            product.ResourceDir = txt_res_dir.Text;
            product.UpdateConfDir = txt_hotupdate_conf.Text;
            product.BackDir = txt_bk_dir.Text;
            product.BaseVer = txt_online_ver.Text;
            Config.AddProduct(product);
            Config.SaveToFile();
        }
    }
}
