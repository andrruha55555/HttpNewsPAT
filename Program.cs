using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Diagnostics;

namespace HttpNewsPAT
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Cookie token = SingIn("user", "user");
            GetContent(token);
            //WebRequest request = WebRequest.Create("https://news.permaviat.ru/main");
            //HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            //Console.WriteLine(response.StatusDescription);
            //using (Stream dataStream = response.GetResponseStream()) 
            //{
            //    using (StreamReader reader = new StreamReader(dataStream))
            //    {
            //        string responseFromServer = reader.ReadToEnd();
            //        Console.WriteLine(responseFromServer);
            //    }
            //}              


            ////reader.Close();
            ////dataStream.Close();
            ////response.Close();
            Console.Read();

        }
        public static Cookie SingIn(string login, string password)
        {
            Cookie token = null;
            string Url = "http://news.permaviat.ru/ajax/login.php";

            Debug.WriteLine($"Выполняем запрос: {Url}");

            HttpWebRequest Request = (HttpWebRequest)WebRequest.Create(Url);
            Request.Method = "POST";
            Request.ContentType = "application/x-www-form-urlencoded";
            Request.CookieContainer = new CookieContainer();

            byte[] Data = Encoding.ASCII.GetBytes($"Login={login}&password={password}");
            Request.ContentLength = Data.Length;

            using (Stream stream = Request.GetRequestStream())
            {
                stream.Write(Data, 0, Data.Length);
            }

            using (HttpWebResponse Response = (HttpWebResponse)Request.GetResponse())
            {
                Debug.WriteLine($"Статус выполнения: {Response.StatusCode}");

                string ResponseFromServer = new StreamReader(Response.GetResponseStream()).ReadToEnd();
                token = Response.Cookies["token"];
            }
            return token;
        }
        public static string GetContent(Cookie token) 
        {
            string Content = null;
            string Url = "http://news.permaviat.ru/main";
            Debug.WriteLine($"Выполняем запрос: {Url}");

            HttpWebRequest Request = (HttpWebRequest)WebRequest.Create(Url);
            Request.CookieContainer = new CookieContainer();
            Request.CookieContainer.Add(token);

            using (HttpWebResponse Response = (HttpWebResponse)Request.GetResponse())
            {
                Debug.WriteLine($"Статус выполнения: {Response.StatusCode}");

                Content = new StreamReader(Response.GetResponseStream()).ReadToEnd();
                
            }
            return Content;
        }

    }

}
