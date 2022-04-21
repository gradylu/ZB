using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZB
{
    /// <summary>
    /// 配置
    /// </summary>
    public class Setting
    {
        /// <summary>
        /// 邮箱服务地址
        /// </summary>
        public string Smtp { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 授权码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 发送邮件列表
        /// </summary>
        public string EmailList { get; set; }

        public static Setting Load()
        {
            string fileName = AppDomain.CurrentDomain.BaseDirectory + "/setting.json";
            if (File.Exists(fileName))
            {
                string json = File.ReadAllText(fileName);
                Setting setting = SimpleJson.SimpleJson.DeserializeObject<Setting>(json);
                return setting;
            }
            else
            {
                Setting setting = new Setting();
                string json = SimpleJson.SimpleJson.SerializeObject(setting);
                File.WriteAllText(fileName, json);
                return setting;
            }
        }

        public static void Save(Setting setting)
        {
            string fileName = AppDomain.CurrentDomain.BaseDirectory + "/setting.json";
            string json = SimpleJson.SimpleJson.SerializeObject(setting);
            File.WriteAllText(fileName, json);
        }
    }
}
