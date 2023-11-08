using DocumentFormat.OpenXml.Office2010.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCSTsTool.Data
{
    /// <summary>
    /// テキストデータ
    /// </summary>
    internal class TextData
    {

        /// <summary>
        /// 1件分テキスト
        /// </summary>
        public class Text
        {
            /// <summary>
            /// パス
            /// </summary>
            public string Path { get; set; } = "";


            /// <summary>
            /// ファイル名
            /// </summary>
            public string Filename { get; set; } = "";

            /// <summary>
            /// ID
            /// </summary>
            public string ID { get; set; } = "";


            /// <summary>
            /// 原文テキスト
            /// </summary>
            public string SrcText { get; set; } = "";


            /// <summary>
            /// 翻訳テキスト
            /// </summary>
            public string TranslatedText { get; set; } = "";

        }


        /// <summary>
        /// テキストデータ
        /// </summary>
        public List<Text> TextList { get; } = new List<Text>();

        /// <summary>
        /// テキストデータ
        /// </summary>
        private Dictionary<string, Dictionary<string, Text>> TextListDic = new Dictionary<string, Dictionary<string, Text>>();

        /// <summary>
        /// 追加
        /// </summary>
        /// <param name="text"></param>
        public void AddTextList(Text text)
        {
            this.TextList.Add(text);


            //Dicに追加
            string keyFilename = Path.GetFileNameWithoutExtension(text.Filename.ToLower());
            if (!this.TextListDic.ContainsKey(keyFilename))
            {
                this.TextListDic.Add(keyFilename, new Dictionary<string, Text>());
            }

            string keyID = text.ID.ToLower();
            this.TextListDic[keyFilename].Add(keyID, text);
        }

        /// <summary>
        /// Text取得
        /// </summary>
        /// <returns></returns>
        public Text GetText(string keyFilename,string keyID)
        {
            keyFilename = keyFilename.ToLower();
            keyID = keyID.ToLower();
            if (!this.TextListDic.ContainsKey(keyFilename))
            {
                return null;
            }
            if (!this.TextListDic[keyFilename].ContainsKey(keyID))
            {
                return null;
            }


            return this.TextListDic[keyFilename][keyID];
        }


    }
}
