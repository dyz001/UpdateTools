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
        }

        protected void loadConfig()
        {

        }

        public static FormUpdate GetInstance()
        {
            return instance;
        }

        private void btn_sel_bk_dir_Click(object sender, EventArgs e)
        {

        }

        private void btn_diff_gen_Click(object sender, EventArgs e)
        {
            //拷贝资源到目标目录
            copy_task = new Task<bool>(n => CopyResSrc((FormUpdate)n), this);
            copy_task.Start();
            gen_md5_task = copy_task.ContinueWith(task =>
                {
                    return GenMd5(this);
                });
        }

        public String getProjectDir()
        {
            return txt_proj_dir.Text;
        }

        public String getBKDir()
        {
            return Path.Combine(txt_bk_dir.Text, txt_product_name.Text + txt_out_ver.Text);
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
            if(!File.Exists(Config.getMd5File()))
            {
                File.Create(Config.getMd5File());
            }
            StreamWriter sw = new StreamWriter(Config.getMd5File());
            Utils.GenResMd5(update_form.getBKDir(), sw, update_form.getBKDir());
            return ret;
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
            return "";
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
            return "";
        }
    }
}
