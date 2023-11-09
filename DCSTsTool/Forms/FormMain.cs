using DCSTsTool.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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
        /// ベースパス
        /// </summary>
        private const string BasePath = @"C:\Repository\社内Tool\Tool\TestData";
        //private const string BasePath = @"F:\TEMP\DCSTsTool\TestData";


        /// <summary>
        /// テキスト取得
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GetOrgTextButton_Click(object sender, EventArgs e)
        {
            const int lang = 1;//英語

            var mizFile = new Data.MizFile();
            var textData = mizFile.GetText(Path.Combine(BasePath, @"DCS Mission"));


            if (textData != null)
            {
                //データ保存
                var excelData = new ExcelData();
                excelData.ExportExcelData(Path.Combine(BasePath, @"TextDataOrg.xlsx"), textData, lang);

            }

            this.Close();
        }

        /// <summary>
        /// 翻訳済テキスト取得
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GetTranslatTextButton_Click(object sender, EventArgs e)
        {

            const int lang = 2;//日本語

            var mizFile = new Data.MizFile();
            var textData = mizFile.GetTextJP(Path.Combine(BasePath, @"folder"));


            if (textData != null)
            {
                //データ保存
                var excelData = new ExcelData();
                excelData.ExportExcelData(Path.Combine(BasePath, @"TextDataJP.xlsx"), textData, lang);
            }

            this.Close();
        }



        /// <summary>
        /// テキスト書き換え
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextWriteButton_Click(object sender, EventArgs e)
        {
            //データ取得
            var excelData = new ExcelData();
            var textData = excelData.ImportExcelData(Path.Combine(BasePath, @"TextDataTr.xlsx"));

            //データ書き換え
            var mizFile = new Data.MizFile();
            mizFile.ChangeText(Path.Combine(BasePath, @"DCS Mission JPN"), textData);

            this.Close();
        }

    }
}
