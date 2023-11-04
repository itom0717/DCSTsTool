using DCSTsTool.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DCSTsTool.Forms
{
    public partial class FormMain : Form
    {
         /// <summary>
         /// コンストラクト
         /// </summary>
        public FormMain()
        {
            InitializeComponent();
        }



        /// <summary>
        /// テキスト取得
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            var mizFile = new Data.MizFile();
            var textData = mizFile.GetText(@"F:\TEMP\DCSTsTool\TestData\DCS Mission");


            if(textData != null)
            {
                //データ保存
                var excelData = new ExcelData();
                excelData.ExportExcelData(@"F:\TEMP\DCSTsTool\TestData\TextData.xlsx", textData);

            }

            this.Close();
        }




        /// <summary>
        /// テキスト書き換え
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            //TEST

            //var mizFile = new Data.MizFile();
            //mizFile.Test();

            //データ取得
            var excelData = new ExcelData();
            var textData = excelData.ImportExcelData(@"F:\TEMP\DCSTsTool\TestData\TextData.xlsx");

            //データ書き換え
            var mizFile = new Data.MizFile();
            mizFile.ChangeText(@"F:\TEMP\DCSTsTool\TestData\DCS Mission JPN", textData);

            this.Close();
        }

    }
}
