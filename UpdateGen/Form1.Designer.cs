namespace UpdateGen
{
    partial class FormUpdate
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.lbl_product = new System.Windows.Forms.Label();
            this.cmb_product = new System.Windows.Forms.ComboBox();
            this.cb_compile_code = new System.Windows.Forms.CheckBox();
            this.lbl_bk_dir = new System.Windows.Forms.Label();
            this.txt_bk_dir = new System.Windows.Forms.TextBox();
            this.btn_sel_bk_dir = new System.Windows.Forms.Button();
            this.lbl_proj_dir = new System.Windows.Forms.Label();
            this.txt_proj_dir = new System.Windows.Forms.TextBox();
            this.btn_sel_proj = new System.Windows.Forms.Button();
            this.lbl_res_dir = new System.Windows.Forms.Label();
            this.txt_res_dir = new System.Windows.Forms.TextBox();
            this.btn_sel_res_dir = new System.Windows.Forms.Button();
            this.btn_sel_hotupdate = new System.Windows.Forms.Button();
            this.txt_hotupdate_conf = new System.Windows.Forms.TextBox();
            this.lbl_hotupdate_conf = new System.Windows.Forms.Label();
            this.lbl_product_name = new System.Windows.Forms.Label();
            this.txt_product_name = new System.Windows.Forms.TextBox();
            this.lbl_online_ver = new System.Windows.Forms.Label();
            this.txt_online_ver = new System.Windows.Forms.TextBox();
            this.txt_out_ver = new System.Windows.Forms.TextBox();
            this.lbl_out_ver = new System.Windows.Forms.Label();
            this.btn_save_ver = new System.Windows.Forms.Button();
            this.txt_log = new System.Windows.Forms.TextBox();
            this.btn_diff_gen = new System.Windows.Forms.Button();
            this.btn_open_res = new System.Windows.Forms.Button();
            this.btn_open_conf = new System.Windows.Forms.Button();
            this.lbl_declare = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbl_product
            // 
            this.lbl_product.AutoSize = true;
            this.lbl_product.Location = new System.Drawing.Point(5, 10);
            this.lbl_product.Name = "lbl_product";
            this.lbl_product.Size = new System.Drawing.Size(65, 12);
            this.lbl_product.TabIndex = 0;
            this.lbl_product.Text = "产品列表：";
            // 
            // cmb_product
            // 
            this.cmb_product.FormattingEnabled = true;
            this.cmb_product.Location = new System.Drawing.Point(88, 6);
            this.cmb_product.Name = "cmb_product";
            this.cmb_product.Size = new System.Drawing.Size(121, 20);
            this.cmb_product.TabIndex = 1;
            // 
            // cb_compile_code
            // 
            this.cb_compile_code.AutoSize = true;
            this.cb_compile_code.Location = new System.Drawing.Point(238, 8);
            this.cb_compile_code.Name = "cb_compile_code";
            this.cb_compile_code.Size = new System.Drawing.Size(72, 16);
            this.cb_compile_code.TabIndex = 2;
            this.cb_compile_code.Text = "编译代码";
            this.cb_compile_code.UseVisualStyleBackColor = true;
            // 
            // lbl_bk_dir
            // 
            this.lbl_bk_dir.AutoSize = true;
            this.lbl_bk_dir.Location = new System.Drawing.Point(7, 43);
            this.lbl_bk_dir.Name = "lbl_bk_dir";
            this.lbl_bk_dir.Size = new System.Drawing.Size(65, 12);
            this.lbl_bk_dir.TabIndex = 3;
            this.lbl_bk_dir.Text = "备份目录：";
            // 
            // txt_bk_dir
            // 
            this.txt_bk_dir.Location = new System.Drawing.Point(88, 36);
            this.txt_bk_dir.Name = "txt_bk_dir";
            this.txt_bk_dir.Size = new System.Drawing.Size(402, 21);
            this.txt_bk_dir.TabIndex = 4;
            // 
            // btn_sel_bk_dir
            // 
            this.btn_sel_bk_dir.Location = new System.Drawing.Point(495, 34);
            this.btn_sel_bk_dir.Name = "btn_sel_bk_dir";
            this.btn_sel_bk_dir.Size = new System.Drawing.Size(75, 23);
            this.btn_sel_bk_dir.TabIndex = 5;
            this.btn_sel_bk_dir.Text = "选择";
            this.btn_sel_bk_dir.UseVisualStyleBackColor = true;
            this.btn_sel_bk_dir.Click += new System.EventHandler(this.btn_sel_bk_dir_Click);
            // 
            // lbl_proj_dir
            // 
            this.lbl_proj_dir.AutoSize = true;
            this.lbl_proj_dir.Location = new System.Drawing.Point(7, 78);
            this.lbl_proj_dir.Name = "lbl_proj_dir";
            this.lbl_proj_dir.Size = new System.Drawing.Size(65, 12);
            this.lbl_proj_dir.TabIndex = 6;
            this.lbl_proj_dir.Text = "工程目录：";
            // 
            // txt_proj_dir
            // 
            this.txt_proj_dir.Location = new System.Drawing.Point(88, 75);
            this.txt_proj_dir.Name = "txt_proj_dir";
            this.txt_proj_dir.Size = new System.Drawing.Size(402, 21);
            this.txt_proj_dir.TabIndex = 7;
            // 
            // btn_sel_proj
            // 
            this.btn_sel_proj.Location = new System.Drawing.Point(495, 71);
            this.btn_sel_proj.Name = "btn_sel_proj";
            this.btn_sel_proj.Size = new System.Drawing.Size(75, 23);
            this.btn_sel_proj.TabIndex = 8;
            this.btn_sel_proj.Text = "选择";
            this.btn_sel_proj.UseVisualStyleBackColor = true;
            // 
            // lbl_res_dir
            // 
            this.lbl_res_dir.AutoSize = true;
            this.lbl_res_dir.Location = new System.Drawing.Point(8, 117);
            this.lbl_res_dir.Name = "lbl_res_dir";
            this.lbl_res_dir.Size = new System.Drawing.Size(65, 12);
            this.lbl_res_dir.TabIndex = 9;
            this.lbl_res_dir.Text = "资源路径：";
            // 
            // txt_res_dir
            // 
            this.txt_res_dir.Location = new System.Drawing.Point(88, 114);
            this.txt_res_dir.Name = "txt_res_dir";
            this.txt_res_dir.Size = new System.Drawing.Size(402, 21);
            this.txt_res_dir.TabIndex = 10;
            // 
            // btn_sel_res_dir
            // 
            this.btn_sel_res_dir.Location = new System.Drawing.Point(495, 113);
            this.btn_sel_res_dir.Name = "btn_sel_res_dir";
            this.btn_sel_res_dir.Size = new System.Drawing.Size(75, 23);
            this.btn_sel_res_dir.TabIndex = 11;
            this.btn_sel_res_dir.Text = "选择";
            this.btn_sel_res_dir.UseVisualStyleBackColor = true;
            // 
            // btn_sel_hotupdate
            // 
            this.btn_sel_hotupdate.Location = new System.Drawing.Point(495, 154);
            this.btn_sel_hotupdate.Name = "btn_sel_hotupdate";
            this.btn_sel_hotupdate.Size = new System.Drawing.Size(75, 23);
            this.btn_sel_hotupdate.TabIndex = 14;
            this.btn_sel_hotupdate.Text = "选择";
            this.btn_sel_hotupdate.UseVisualStyleBackColor = true;
            // 
            // txt_hotupdate_conf
            // 
            this.txt_hotupdate_conf.Location = new System.Drawing.Point(88, 153);
            this.txt_hotupdate_conf.Name = "txt_hotupdate_conf";
            this.txt_hotupdate_conf.Size = new System.Drawing.Size(402, 21);
            this.txt_hotupdate_conf.TabIndex = 13;
            // 
            // lbl_hotupdate_conf
            // 
            this.lbl_hotupdate_conf.AutoSize = true;
            this.lbl_hotupdate_conf.Location = new System.Drawing.Point(9, 156);
            this.lbl_hotupdate_conf.Name = "lbl_hotupdate_conf";
            this.lbl_hotupdate_conf.Size = new System.Drawing.Size(65, 12);
            this.lbl_hotupdate_conf.TabIndex = 12;
            this.lbl_hotupdate_conf.Text = "热更配置：";
            // 
            // lbl_product_name
            // 
            this.lbl_product_name.AutoSize = true;
            this.lbl_product_name.Location = new System.Drawing.Point(11, 196);
            this.lbl_product_name.Name = "lbl_product_name";
            this.lbl_product_name.Size = new System.Drawing.Size(59, 12);
            this.lbl_product_name.TabIndex = 15;
            this.lbl_product_name.Text = "产品名称:";
            // 
            // txt_product_name
            // 
            this.txt_product_name.Location = new System.Drawing.Point(70, 192);
            this.txt_product_name.Name = "txt_product_name";
            this.txt_product_name.Size = new System.Drawing.Size(100, 21);
            this.txt_product_name.TabIndex = 16;
            // 
            // lbl_online_ver
            // 
            this.lbl_online_ver.AutoSize = true;
            this.lbl_online_ver.Location = new System.Drawing.Point(173, 197);
            this.lbl_online_ver.Name = "lbl_online_ver";
            this.lbl_online_ver.Size = new System.Drawing.Size(59, 12);
            this.lbl_online_ver.TabIndex = 17;
            this.lbl_online_ver.Text = "线上版本:";
            // 
            // txt_online_ver
            // 
            this.txt_online_ver.Location = new System.Drawing.Point(228, 193);
            this.txt_online_ver.Name = "txt_online_ver";
            this.txt_online_ver.Size = new System.Drawing.Size(100, 21);
            this.txt_online_ver.TabIndex = 18;
            // 
            // txt_out_ver
            // 
            this.txt_out_ver.Location = new System.Drawing.Point(388, 194);
            this.txt_out_ver.Name = "txt_out_ver";
            this.txt_out_ver.Size = new System.Drawing.Size(100, 21);
            this.txt_out_ver.TabIndex = 20;
            // 
            // lbl_out_ver
            // 
            this.lbl_out_ver.AutoSize = true;
            this.lbl_out_ver.Location = new System.Drawing.Point(333, 197);
            this.lbl_out_ver.Name = "lbl_out_ver";
            this.lbl_out_ver.Size = new System.Drawing.Size(59, 12);
            this.lbl_out_ver.TabIndex = 19;
            this.lbl_out_ver.Text = "输出版本:";
            // 
            // btn_save_ver
            // 
            this.btn_save_ver.Location = new System.Drawing.Point(495, 194);
            this.btn_save_ver.Name = "btn_save_ver";
            this.btn_save_ver.Size = new System.Drawing.Size(75, 23);
            this.btn_save_ver.TabIndex = 21;
            this.btn_save_ver.Text = "保存配置";
            this.btn_save_ver.UseVisualStyleBackColor = true;
            // 
            // txt_log
            // 
            this.txt_log.Location = new System.Drawing.Point(13, 236);
            this.txt_log.Multiline = true;
            this.txt_log.Name = "txt_log";
            this.txt_log.Size = new System.Drawing.Size(471, 330);
            this.txt_log.TabIndex = 22;
            // 
            // btn_diff_gen
            // 
            this.btn_diff_gen.Location = new System.Drawing.Point(504, 236);
            this.btn_diff_gen.Name = "btn_diff_gen";
            this.btn_diff_gen.Size = new System.Drawing.Size(75, 72);
            this.btn_diff_gen.TabIndex = 23;
            this.btn_diff_gen.Text = "差异生成";
            this.btn_diff_gen.UseVisualStyleBackColor = true;
            this.btn_diff_gen.Click += new System.EventHandler(this.btn_diff_gen_Click);
            // 
            // btn_open_res
            // 
            this.btn_open_res.Location = new System.Drawing.Point(504, 324);
            this.btn_open_res.Name = "btn_open_res";
            this.btn_open_res.Size = new System.Drawing.Size(75, 72);
            this.btn_open_res.TabIndex = 24;
            this.btn_open_res.Text = "打开资源";
            this.btn_open_res.UseVisualStyleBackColor = true;
            this.btn_open_res.Click += new System.EventHandler(this.btn_open_res_Click);
            // 
            // btn_open_conf
            // 
            this.btn_open_conf.Location = new System.Drawing.Point(504, 416);
            this.btn_open_conf.Name = "btn_open_conf";
            this.btn_open_conf.Size = new System.Drawing.Size(75, 72);
            this.btn_open_conf.TabIndex = 25;
            this.btn_open_conf.Text = "打开配置";
            this.btn_open_conf.UseVisualStyleBackColor = true;
            this.btn_open_conf.Click += new System.EventHandler(this.btn_open_conf_Click);
            // 
            // lbl_declare
            // 
            this.lbl_declare.AutoSize = true;
            this.lbl_declare.Location = new System.Drawing.Point(395, 571);
            this.lbl_declare.Name = "lbl_declare";
            this.lbl_declare.Size = new System.Drawing.Size(101, 12);
            this.lbl_declare.TabIndex = 26;
            this.lbl_declare.Text = "designed by dou ";
            // 
            // FormUpdate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(601, 592);
            this.Controls.Add(this.lbl_declare);
            this.Controls.Add(this.btn_open_conf);
            this.Controls.Add(this.btn_open_res);
            this.Controls.Add(this.btn_diff_gen);
            this.Controls.Add(this.txt_log);
            this.Controls.Add(this.btn_save_ver);
            this.Controls.Add(this.txt_out_ver);
            this.Controls.Add(this.lbl_out_ver);
            this.Controls.Add(this.txt_online_ver);
            this.Controls.Add(this.lbl_online_ver);
            this.Controls.Add(this.txt_product_name);
            this.Controls.Add(this.lbl_product_name);
            this.Controls.Add(this.btn_sel_hotupdate);
            this.Controls.Add(this.txt_hotupdate_conf);
            this.Controls.Add(this.lbl_hotupdate_conf);
            this.Controls.Add(this.btn_sel_res_dir);
            this.Controls.Add(this.txt_res_dir);
            this.Controls.Add(this.lbl_res_dir);
            this.Controls.Add(this.btn_sel_proj);
            this.Controls.Add(this.txt_proj_dir);
            this.Controls.Add(this.lbl_proj_dir);
            this.Controls.Add(this.btn_sel_bk_dir);
            this.Controls.Add(this.txt_bk_dir);
            this.Controls.Add(this.lbl_bk_dir);
            this.Controls.Add(this.cb_compile_code);
            this.Controls.Add(this.cmb_product);
            this.Controls.Add(this.lbl_product);
            this.MaximizeBox = false;
            this.Name = "FormUpdate";
            this.Text = "棋牌更新工具";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_product;
        private System.Windows.Forms.ComboBox cmb_product;
        private System.Windows.Forms.CheckBox cb_compile_code;
        private System.Windows.Forms.Label lbl_bk_dir;
        private System.Windows.Forms.TextBox txt_bk_dir;
        private System.Windows.Forms.Button btn_sel_bk_dir;
        private System.Windows.Forms.Label lbl_proj_dir;
        private System.Windows.Forms.TextBox txt_proj_dir;
        private System.Windows.Forms.Button btn_sel_proj;
        private System.Windows.Forms.Label lbl_res_dir;
        private System.Windows.Forms.TextBox txt_res_dir;
        private System.Windows.Forms.Button btn_sel_res_dir;
        private System.Windows.Forms.Button btn_sel_hotupdate;
        private System.Windows.Forms.TextBox txt_hotupdate_conf;
        private System.Windows.Forms.Label lbl_hotupdate_conf;
        private System.Windows.Forms.Label lbl_product_name;
        private System.Windows.Forms.TextBox txt_product_name;
        private System.Windows.Forms.Label lbl_online_ver;
        private System.Windows.Forms.TextBox txt_online_ver;
        private System.Windows.Forms.TextBox txt_out_ver;
        private System.Windows.Forms.Label lbl_out_ver;
        private System.Windows.Forms.Button btn_save_ver;
        private System.Windows.Forms.TextBox txt_log;
        private System.Windows.Forms.Button btn_diff_gen;
        private System.Windows.Forms.Button btn_open_res;
        private System.Windows.Forms.Button btn_open_conf;
        private System.Windows.Forms.Label lbl_declare;
    }
}

