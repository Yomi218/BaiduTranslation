using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Furion.JsonSerialization;
using Furion.RemoteRequest.Extensions;

namespace BaiduTranslation.Application
{
    /// <summary>
    /// 系统服务接口
    /// </summary>
    public class SystemAppService : IDynamicApiController
    {
        //private readonly ISystemService _systemService;

        //public SystemAppService(ISystemService systemService)
        //{
        //    _systemService = systemService;
        //}

        /// <summary>
        /// 获取系统描述
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetDescription(string q)
        {
            // 源语言
            string from = "zh";
            // 目标语言
            string to = "en";
            // 改成您的APP ID
            string appId = "20201217000649202";
            Random rd = new Random();
            string salt = rd.Next(100000).ToString();
            // 改成您的密钥
            string secretKey = "Y6YPm5WqdrpfI_muF2d6";
            string sign = EncryptString(appId + q + salt + secretKey);
            string url = "http://api.fanyi.baidu.com/api/trans/vip/translate?";
            url += "q=" + HttpUtility.UrlEncode(q);
            url += "&from=" + from;
            url += "&to=" + to;
            url += "&appid=" + appId;
            url += "&salt=" + salt;
            url += "&sign=" + sign;
            var res = await url.GetAsync();
            var str = await res.Content.ReadAsStringAsync();
            var model = JSON.Deserialize<Result>(str);
            if (!string.IsNullOrEmpty(model.error_msg))
                throw Oops.Oh(model.error_msg);
            var dst = model.trans_result[0].dst;
            var split = dst.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var result = "";
            foreach (var item in split)
            {
                result += Regex.Replace(item, @"^\w", m => m.Value.ToUpper());
            }
            return result;
        }

        // 计算MD5值
        public static string EncryptString(string str)
        {
            MD5 md5 = MD5.Create();
            // 将字符串转换成字节数组
            byte[] byteOld = Encoding.UTF8.GetBytes(str);
            // 调用加密方法
            byte[] byteNew = md5.ComputeHash(byteOld);
            // 将加密结果转换为字符串
            StringBuilder sb = new StringBuilder();
            foreach (byte b in byteNew)
            {
                // 将字节转换成16进制表示的字符串，
                sb.Append(b.ToString("x2"));
            }
            // 返回加密的字符串
            return sb.ToString();
        }

        public class Result
        {
            public List<Item> trans_result { get; set; }

            public string error_code { get; set; }

            public string error_msg { get; set; }
        }

        public class Item
        {
            public string src { get; set; }

            public string dst { get; set; }
        }
    }
}