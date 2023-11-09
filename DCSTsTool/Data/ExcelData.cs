using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;


namespace DCSTsTool.Data
{
    /// <summary>
    /// ExcelData
    /// </summary>
    internal class ExcelData
    {


        /// <summary>
        ///出力
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="textData"></param>
        public void ExportExcelData(string filename, Data.TextData textData, int lang)
        {
            if(!File.Exists(filename))
            {
                string templateFile = Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "TextData.xlsx");
                File.Copy(templateFile, filename, true);
            }



            // Excelファイルを開く
            using (var wb = new XLWorkbook(filename))
            {
                //一番左のシートを取得
                var ws = wb.Worksheet(1);

                int rowIndex = 2;

                foreach(var t in textData.TextList)
                {

                    ws.Cell(rowIndex, 1).Value = t.Path;
                    ws.Cell(rowIndex, 2).Value = Path.GetFileNameWithoutExtension(t.Filename);
                    ws.Cell(rowIndex, 3).Value = t.ID;
                    if(lang == 1)
                    {
                        ws.Cell(rowIndex, 4).Value = t.SrcText;
                        ws.Cell(rowIndex, 5).Value = "";
                    }
                    else if (lang == 2)
                    {
                        ws.Cell(rowIndex, 4).Value = "";
                        ws.Cell(rowIndex, 5).Value = t.SrcText;
                    }


                    rowIndex++;
                }


                wb.Save();
            }
        }

        /// <summary>
        /// Excelからデータ取得
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public Data.TextData ImportExcelData(string filename)
        {
            var textData = new Data.TextData();


            // Excelファイルを開く
            using (var wb = new XLWorkbook(filename))
            {
                //一番左のシートを取得
                var ws = wb.Worksheet(1);

                int lastRowIndex = ws.LastRowUsed().RowNumber();
                for (int rowIndex = 2; rowIndex <= lastRowIndex; rowIndex++)
                {
                    IXLCell cell;
                    if (ws.Row(rowIndex).IsHidden)
                    {
                        continue;
                    }


                    var text = new TextData.Text();


                    //Filename
                    cell = ws.Cell(rowIndex, 2);
                    text.Filename = cell.Value.ToString().Trim();
                    if (string.IsNullOrWhiteSpace(text.Filename))
                    {
                        continue;
                    }

                    //ID
                    cell = ws.Cell(rowIndex, 3);
                    text.ID = cell.Value.ToString().Trim();
                    if (string.IsNullOrWhiteSpace(text.ID))
                    {
                        continue;
                    }

                    //EngText
                    cell = ws.Cell(rowIndex, 4);
                    text.SrcText = cell.Value.ToString();

                    //JpnText
                    cell = ws.Cell(rowIndex, 5);
                    text.TranslatedText = cell.Value.ToString();
                    text.TranslatedText = text.SrcText;

                    //追加
                    textData.AddTextList(text);
                }
               

            }

            return textData;
        }

    }
}
