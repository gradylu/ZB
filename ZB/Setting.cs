using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZB
{
    public class Setting
    {
        public string Smtp { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

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
