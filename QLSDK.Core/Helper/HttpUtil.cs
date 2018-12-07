using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace QLSDK.Core
{
    internal static class HttpUtil<T>
    {
        public static readonly ILog logger = LogUtil.GetLoger("QLSDK.HttpUtil");

        #region 发送Post请求
        public static HttpResullt<T> Post(string url, object data)
        {
            if (url.StartsWith("https://"))
            {
                return HttpsPost(url, data);
            }
            else
            {
                return HttpPost(url, data);
            }
        }
        #endregion

        #region 发送Http请求

        public static HttpResullt<T> HttpPost(string url, object data)
        {
            try
            {
                logger.Debug(string.Format("HttpPost({0},{1})", url, SerializerUtil.SerializeJson(data)));

                Encoding myEncode = Encoding.GetEncoding("UTF-8");
                HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
                req.Method = "POST";
                req.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";

                var message = string.Empty;
                if (null != data)
                {
                    message = SerializerUtil.SerializeJson(data);
                }
                byte[] sendBytes = Encoding.GetEncoding("utf-8").GetBytes(message);
                req.ContentLength = sendBytes.Length;
                using (var stream = req.GetRequestStream())
                {
                    stream.Write(sendBytes, 0, sendBytes.Length);
                }
                using (WebResponse res = req.GetResponse())
                {
                    using (StreamReader sr = new StreamReader(res.GetResponseStream(), myEncode))
                    {
                        string strResult = sr.ReadToEnd();
                        logger.Info(string.Format("HttpPost End: {0}", strResult));
                        return SerializerUtil.DeSerializeJson<HttpResullt<T>>(strResult);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("HttpsPost异常，ex:" + ex.Message);
                throw ex;
            }
        }
        #endregion

        #region 发送HttpsPost请求
        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; //总是接受     
        }

        private static HttpResullt<T> HttpsPost(string uri, object data)
        {
            try
            {
                logger.Debug(string.Format("HttpsPost({0},{1})", uri, SerializerUtil.SerializeJson(data)));
                //HTTPSQ请求  
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
                var req = WebRequest.Create(uri) as HttpWebRequest;
                req.Method = "POST";
                req.ContentType = "application/x-www-form-urlencoded";

                var message = string.Empty;
                if (null != data)
                {
                    message = SerializerUtil.SerializeJson(data);
                }
                byte[] sendBytes = Encoding.GetEncoding("utf-8").GetBytes(message);
                req.ContentLength = sendBytes.Length;
                using (var stream = req.GetRequestStream())
                {
                    stream.Write(sendBytes, 0, sendBytes.Length);
                }

                using (var respStream = req.GetResponse().GetResponseStream())
                {
                    using (var reader = new StreamReader(respStream))
                    {
                        var responseData = reader.ReadToEnd();
                        logger.Info(string.Format("HttpsPost End: {0}", responseData));
                        return SerializerUtil.DeSerializeJson<HttpResullt<T>>(responseData);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("HttpsPost异常，ex:"+ex.Message);
                throw ex;
            }
        }
        #endregion
    }

    public class HttpResullt<T>
    {
        /// <summary>
        /// 结果代码  成功：200
        /// </summary>
        public string code;

        public bool success { get { return code == "200"; } }
        /// <summary>
        /// 结果消息 成功：success
        /// </summary>
        public string msg;
        /// <summary>
        /// 附加对象
        /// </summary>
        public T data;
    }

    internal class AuthorizeData
    {
        public string sip_addr { get; set; }
        public string account { get; set; }
        public string pass { get; set; }
        public string log_level { get; set; }
    }
}
