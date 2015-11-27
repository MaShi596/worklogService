using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using worklogService.DBoperate;
using worklogService.Models;

namespace worklogService.CommonMethod
{
    public class ImgBase64
    {

        public string ImgToBase64String(string Imagefilename)
        {
            try
            {
                Bitmap bmp = new Bitmap(Imagefilename);
                //this.pictureBox1.Image = bmp;
                //FileStream fs = new FileStream(Imagefilename + ".txt", FileMode.Create);
                //StreamWriter sw = new StreamWriter(fs);

                MemoryStream ms = new MemoryStream();
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] arr = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(arr, 0, (int)ms.Length);
                ms.Close();
                String strbaser64 = Convert.ToBase64String(arr);
                return "转换成功";
                
                //sw.Write(strbaser64);

                //sw.Close();
                //fs.Close();
                // MessageBox.Show("转换成功!");
            }
            catch (Exception ex)
            {
                return "ImgToBase64String 转换失败\nException:" + ex.Message;
                //MessageBox.Show("ImgToBase64String 转换失败\nException:" + ex.Message);
            }
        }



        public string Base64StringToImage(string base64Imgstring,int id)
        {

            string res;
            BaseService baseservice = new BaseService();
            String inputStr = "";
            string url = "";
            string md5code = "";
            try
            {
                                //FileStream ifs = new FileStream(txtFileName, FileMode.Open, FileAccess.Read);
                                //StreamReader sr = new StreamReader(ifs);
                                //byte[] buffer = Convert.FromBase64String(base64Imgstring);
                                //String textBase64 = Base64.encodeToString(textByte, Base64.DEFAULT);
                inputStr = base64Imgstring.Substring(22);
                byte[] arr = Convert.FromBase64String(inputStr);
                MemoryStream ms = new MemoryStream(arr);
                Bitmap bmp = new Bitmap(ms);

                                //bmp.Save(txtFileName + ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                                //bmp.Save(txtFileName + ".bmp", ImageFormat.Bmp);
                                //bmp.Save(txtFileName + ".gif", ImageFormat.Gif);
                                //bmp.Save(txtFileName + ".png", ImageFormat.Png);
                
                ms.Close();

                url  = HttpContext.Current.Server.MapPath("/") +
                         @"\\Images\\Icons\\" + id + ".png";


                if (File.Exists(url))
                {
                 File.Delete(url);
                }

                bmp.Save(url,ImageFormat.Png);

                if (File.Exists(url))
                {
                    md5code =   GetMD5HashFromFile(url);
                   
                   WkTUser w = new WkTUser();
                   w = (WkTUser)baseservice.loadEntity(w, id);

                   w.ImgMD5Code = md5code;
                   w.Base64Img = base64Imgstring;

                     
                   baseservice.SaveOrUpdateEntity(w);
                   
                }

                //sr.Close();
                //ifs.Close();
                //this.pictureBox2.Image = bmp;
                //if (File.Exists(txtFileName))
                //{
                    //File.Delete(txtFileName);
                //}
                res = "转换成功"; //+base64Imgstring.Length + "    " + md5code;
                //MessageBox.Show("转换成功！");
            }
            catch (Exception ex)
            {
                res = "Base64StringToImage 转换失败\nException：" + ex.Message; //+ "      00000" + md5code + inputStr;
                //MessageBox.Show("Base64StringToImage 转换失败\nException：" + ex.Message);
            }
            return res;
        }

         public string GetMD5HashFromFile(string fileName)  
        {  
            try  
            {  
                FileStream file = new FileStream(fileName, System.IO.FileMode.Open);  
                MD5 md5 = new MD5CryptoServiceProvider();  
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


    }
}