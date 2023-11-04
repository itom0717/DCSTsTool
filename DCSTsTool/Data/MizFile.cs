using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;
using System.Diagnostics;
using System.IO;
using System.Collections;
using System.Text.RegularExpressions;

namespace DCSTsTool.Data
{
    internal class MizFile
    {
        /// <summary>
        /// mizファイル内の英語の場所
        /// </summary>
        private const string MizPathDefault = @"l10n/DEFAULT/";

        /// <summary>
        /// mizファイル内の英語の場所(バックアップ)
        /// </summary>
        private const string MizPathDefaultOrg = @"l10n/DEFAULT_ORG/";

        /// <summary>
        /// テキスト取得
        /// </summary>
        public TextData GetText(string tgtPath)
        {
            if (string.IsNullOrWhiteSpace(tgtPath) || !Directory.Exists(tgtPath))
            {
                return null;
            }

            if (tgtPath.Substring(tgtPath.Length - 1, 1) != Path.DirectorySeparatorChar.ToString())
            {
                tgtPath = tgtPath + Path.DirectorySeparatorChar.ToString();
            }


            //ファイルを列挙
            var list = this.GetMizFileList(tgtPath, true);
            var textData = new TextData();

            foreach (var f in list)
            {
                string rf = f.Substring(tgtPath.Length);

                Debug.WriteLine($"---------------------------------------------");
                Debug.WriteLine($"{rf}");
                Debug.WriteLine($"---------------------------------------------");

                // mizファイル内から dictionary の内容を取得す
                string dictionaryText = this.GetDictionaryText(f, MizPathDefault, MizPathDefaultOrg);

                //Debug.WriteLine(dictionaryText);

                this.GetTextData(dictionaryText, f, textData);
            }

            return textData;
        }

        /// <summary>
        /// mizファイルを列挙
        /// </summary>
        /// <param name="tgtPath"></param>
        /// <returns></returns>
        private string[] GetMizFileList(string tgtPath, bool isSubDir)
        {
            string[] list = Directory.GetFiles(tgtPath, "*.miz", (isSubDir ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly));
            return list;
        }

        /// <summary>
        /// mizファイル内から dictionary の内容を取得する
        /// </summary>
        /// <param name="tgtFilename"></param>
        /// <returns></returns>
        private string GetDictionaryText(string tgtFilename, string tgtZipPath, string tgtZipPathOrg)
        {
            string dictionaryText = "";

            using (ZipArchive a = ZipFile.Open(tgtFilename, ZipArchiveMode.Read, System.Text.Encoding.UTF8))
            {
                foreach (ZipArchiveEntry e in a.Entries)
                {
                    //バックアップがあればそちらが優先
                    if (e.FullName.ToLower() == $"{tgtZipPathOrg}dictionary".ToLower())
                    {
                        using (StreamReader sr = new StreamReader(e.Open(), System.Text.Encoding.UTF8))
                        {
                            //すべて読み込む
                            dictionaryText = sr.ReadToEnd();
                            break;
                        }
                    }
                    else if (e.FullName.ToLower() == $"{tgtZipPath}dictionary".ToLower())
                    {
                        using (StreamReader sr = new StreamReader(e.Open(), System.Text.Encoding.UTF8))
                        {
                            //すべて読み込む
                            dictionaryText = sr.ReadToEnd();
                            break;
                        }
                    }
                }
            }

            return dictionaryText;
        }

        /// <summary>
        /// テキストデータ解析＆データ取得
        /// </summary>
        /// <param name="dictionaryText"></param>
        /// <param name="filename"></param>
        /// <param name="textData"></param>
        private void GetTextData(string dictionaryText, string filename, TextData textData)
        {
            string tmp = dictionaryText;

            const string CRLFDummy = @"@@@BR@@@";

            //\改行を置換
            tmp = tmp.Replace("\\\n", CRLFDummy);

            //正規表現で取得
            const string pattern1 = @"^\s*\[""(.+)""\]\s*=\s*""(.*)""\s*,\s*\n";
            const string pattern2 = @"^.+Text_\d+$";

            var mc = Regex.Matches(tmp, pattern1, RegexOptions.Multiline);
            foreach (System.Text.RegularExpressions.Match m in mc)
            {
                //Debug.WriteLine(m.Value);

                string ID = m.Groups[1].Value.Trim();
                string text = m.Groups[2].Value.Trim();
                if (string.IsNullOrWhiteSpace(text))
                {
                    continue;
                }

                if (!Regex.IsMatch(ID, pattern2))
                {
                    continue;
                }

                text = text.Replace(CRLFDummy, "\n");

                Debug.WriteLine($"{ID} = {text}");


                var t = new TextData.Text();
                t.Path = Path.GetDirectoryName(filename);
                t.Filename = Path.GetFileName(filename);
                t.ID = ID;
                t.SrcText = text;
                textData.AddTextList(t);

            }

        }



        /// <summary>
        /// テキスト変更
        /// </summary>
        /// <param name="tgtPath"></param>
        /// <returns></returns>
        public void ChangeText(string tgtPath, Data.TextData textData)
        {
            if (string.IsNullOrWhiteSpace(tgtPath) || !Directory.Exists(tgtPath))
            {
                return;
            }

            if (tgtPath.Substring(tgtPath.Length - 1, 1) != Path.DirectorySeparatorChar.ToString())
            {
                tgtPath = tgtPath + Path.DirectorySeparatorChar.ToString();
            }


            //ファイルを列挙
            var list = this.GetMizFileList(tgtPath, true);

            foreach (var f in list)
            {
                string rf = f.Substring(tgtPath.Length);

                Debug.WriteLine($"---------------------------------------------");
                Debug.WriteLine($"{rf}");
                Debug.WriteLine($"---------------------------------------------");

                // mizファイル内から dictionary の内容を取得す
                string dictionaryText = this.GetDictionaryText(f, MizPathDefault, MizPathDefaultOrg);

                //Debug.WriteLine(dictionaryText);
                string newDictionaryText = this.ChangeTextData(dictionaryText, rf, textData);


                //保存処理
                string path = @"C:\Repository\社内Tool\Tool\TestData\TEST";

                using (var sw = new StreamWriter(Path.Combine(path, rf + "_eng.txt"), false, Encoding.UTF8))
                {
                    sw.Write(dictionaryText);
                }
                using (var sw = new StreamWriter(Path.Combine(path, rf + "_jpn.txt"), false, Encoding.UTF8))
                {
                    sw.Write(newDictionaryText);
                }

            }

        }



        /// <summary>
        /// テキストデータ解析＆データ書き換え
        /// </summary>
        /// <param name="dictionaryText"></param>
        /// <param name="filename"></param>
        /// <param name="textData"></param>
        private string ChangeTextData(string dictionaryText, string filename, TextData textData)
        {
            string newDictionaryText = "";

            string tmp = dictionaryText;

            const string CRLFDummy = @"@@@BR@@@";

            //\改行を置換
            tmp = tmp.Replace("\\\n", CRLFDummy);
            var split = tmp.Split('\n');

            //正規表現で取得
            const string pattern1 = @"^\s*\[""(.+)""\]\s*=\s*""(.*)""\s*,\s*$";
            const string pattern2 = @"^.+Text_\d+$";


            for (int i = 0; i < split.Length;i++)
            {
                var mc = Regex.Matches(split[i], pattern1, RegexOptions.Singleline);
                if (mc.Count != 0)
                {
                    //対象のデータ
                    var m = mc[0];

                    string ID = m.Groups[1].Value.Trim();
                    string text = m.Groups[2].Value.Trim();
                    if (string.IsNullOrWhiteSpace(text))
                    {
                        continue;
                    }

                    if (!Regex.IsMatch(ID, pattern2))
                    {
                        continue;
                    }

                    //データが存在するか？
                    var newText = textData.GetText(filename, ID);
                    if (newText == null)
                    {
                        continue;
                    }

                    if(string.IsNullOrWhiteSpace(newText.TranslatedText))
                    {
                        continue;
                    }

                    //データ書き換え
                    string tmpText = newText.TranslatedText;
                    tmpText = tmpText.Replace("\n", CRLFDummy);


                    string t = split[i].Substring(0, m.Groups[2].Index);
                    t += tmpText;
                    t += split[i].Substring(m.Groups[2].Index + m.Groups[2].Length);


                    split[i] = t;

                }
            }
            tmp = string.Join("\n", split);
            tmp = tmp.Replace( CRLFDummy, "\\\n");
            newDictionaryText = tmp;

            return newDictionaryText;
        }



















    public void Test()
    {


        //ZIP書庫を開く
        string miz = @"C:\Repository\社内Tool\Tool\TestData\DCS Mission\FA-18C\Missions\Training\Lesson 2-Cold Start.miz";

        //zipファイルを開く
        using (ZipArchive a = ZipFile.Open(miz, ZipArchiveMode.Update, System.Text.Encoding.UTF8))
        {

            ZipArchiveEntry entryDictionaryEN = null;
            ZipArchiveEntry entryDictionaryJA = null;

            //各ファイルを調査
            foreach (ZipArchiveEntry e in a.Entries)
            {
                if (e.FullName.ToLower() == "l10n/DEFAULT/dictionary".ToLower())
                {
                    //英語データ
                    entryDictionaryEN = e;
                }
                else if (e.FullName.ToLower() == "l10n/JA/dictionary".ToLower())
                {
                    //日本語データ
                    entryDictionaryJA = e;
                }
            }

            if (entryDictionaryEN == null)
            {
                //データがない
                return;
            }

            Debug.WriteLine($"EN-フルパス   : {entryDictionaryEN.FullName}");

            if (entryDictionaryJA != null)
            {
                Debug.WriteLine($"JA-フルパス   : {entryDictionaryJA.FullName}");
            }


            //ENファイルの内容を取得
            //ファイルの内容を取得
            string dictionaryEN = "";
            string dictionaryJA = "";
            using (StreamReader sr = new StreamReader(entryDictionaryEN.Open(), System.Text.Encoding.UTF8))
            {
                //すべて読み込む
                dictionaryEN = sr.ReadToEnd();
                dictionaryJA = dictionaryEN;
                Debug.Write(dictionaryEN);
            }

            if (string.IsNullOrEmpty(dictionaryEN))
            {
                //データがない
                return;
            }


            //ファイルの内容を解析&書き換え



            //ファイルを一時保存
            string tmpFile = @"C:\\Repository\\社内Tool\\Tool\\TestData\dictionary";
            if (System.IO.File.Exists(tmpFile))
            {
                System.IO.File.Delete(tmpFile);
            }
            using (StreamWriter sw = new StreamWriter(tmpFile, false, System.Text.Encoding.UTF8))
            {
                sw.Write(dictionaryJA);
                sw.Close();
            }

            //zipファイル内のファイルを削除
            if (entryDictionaryJA != null)
            {
                entryDictionaryJA.Delete();
            }


            //zipファイルに保存
            ZipArchiveEntry newZipArchiveEntry = a.CreateEntryFromFile(tmpFile, @"l10n/JA/dictionary");

            if (System.IO.File.Exists(tmpFile))
            {
                // System.IO.File.Delete(tmpFile);
            }

        }


    }


}
}
