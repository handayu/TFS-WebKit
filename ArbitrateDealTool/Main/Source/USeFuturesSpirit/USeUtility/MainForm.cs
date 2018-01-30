using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace USeUtility
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.txtAssemblePath.Text = @"E:\USeProgram\ArbitrateDealTool\Main\bin\USe.TradeDriver.Common.dll";
        }

        private void btnLoadAssemble_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtAssemblePath.Text))
            {
                MessageBox.Show("请输入文件路径");
                return;
            }

            try
            {
                string assemblyPath = this.txtAssemblePath.Text;
                Assembly assembly = Assembly.LoadFile(assemblyPath);
                Type[] classTypes = assembly.GetExportedTypes();
                if (classTypes != null)
                {
                    foreach (Type typeItem in classTypes)
                    {
                        this.cbxClassType.Items.Add(typeItem.FullName);
                    }
                }

                this.cbxClassType.Text = "USe.TradeDriver.Common.USeMarketData";
            }
            catch (Exception ex)
            {
                MessageBox.Show("加载程序集信息失败");
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                this.txtCodeResult.Text = string.Empty;
                string assemblyPath = this.txtAssemblePath.Text;
                string classFullName = this.cbxClassType.Text;

                FileInfo assemblyFileInfo = new FileInfo(assemblyPath);
                string assemblyExtension = assemblyFileInfo.Extension;
                string remarkXmlPath = assemblyFileInfo.FullName.Substring(0, assemblyFileInfo.FullName.Length - assemblyExtension.Length) + ".XML";
                DataViewModelCodeGenerator generator = new DataViewModelCodeGenerator();
                string codeText = generator.CreateViewModelCode(assemblyPath, classFullName, remarkXmlPath);
                this.txtCodeResult.Text = codeText;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}
