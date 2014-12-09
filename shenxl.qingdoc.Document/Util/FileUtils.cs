using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shenxl.qingdoc.Document.Util
{
    public class FileUtils
    {
        /// <summary>
        /// 根据文件路径获取文件的MD5值
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetMD5HashFromFile(string fileName)
        {
            try
            {
                FileStream file = new FileStream(fileName, FileMode.Open);
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(file);
                file.Close();

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("GetMD5HashFromFile() fail,error:" + ex.Message);
            }
        }

        /// <summary>
        /// 根据文件流获取文件的MD5值
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetMD5HashFromFileStream(Stream fileStream)
        {
            try
            {
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(fileStream);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("GetMD5HashFromFile() fail,error:" + ex.Message);
            }
        }

        /// <summary>
        /// 读取生成的文件
        /// </summary>
        /// <param name="filepath">文件路径</param>
        /// <returns></returns>
        public static string ReadFile(string filepath)
        {
            StringBuilder html = new StringBuilder();
            using (StreamReader objReader = new StreamReader(filepath, Encoding.Default))
            {
                string sLine = "";

                while (sLine != null)
                {
                    sLine = objReader.ReadLine();
                    if (sLine != null && !sLine.Equals(""))
                        html.AppendLine(sLine);
                }
            }
            return html.ToString();
        }

        /// <summary>
        /// 生成样式文件
        /// </summary>
        /// <param name="style"></param>
        /// <param name="outpath"></param>
        public static void WriteStyleFile(string style, string outpath)
        {
            using (FileStream stream = new FileStream(outpath, FileMode.Create))
            {
                //获取StreamWriter
                using (StreamWriter writer = new StreamWriter(stream, Encoding.UTF8))
                {
                    writer.Write(style);
                }
            }
        }
    }
}
