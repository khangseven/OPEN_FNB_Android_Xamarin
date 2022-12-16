using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
namespace OPEN_FNB_Android.Client
{
    [Serializable]
    internal class Packet
    {
        public string name { get; set; }
        public int type { get; set; }
        public Object mainObject { get; set; }
        public Object object1 { get; set; }
        public Object object2 { get; set; }
        public Object object3 { get; set; }

        public Packet(string name, int type, object mainObject, object object1, object object2, object object3)
        {
            this.name = name;
            this.type = type;
            this.mainObject = mainObject;
            this.object1 = object1;
            this.object2 = object2;
            this.object3 = object3;
        }

        public Packet(string name, int type, object mainObject)
        {
            this.name = name;
            this.type = type;
            this.mainObject = mainObject;
        }

        public byte[] toBytes()
        {
            /*var bytes = new byte[1000];
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, this);
                return ms.ToArray();
                
            }*/
            var json = JsonConvert.SerializeObject(this);
            return System.Text.Encoding.UTF8.GetBytes(json);
        }

        public static Packet fromBytes(byte[] arr)
        {
            /*BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream(arr))
            {
                bf.Deserialize(ms);
                return (Packet)bf.Deserialize(ms);
            }*/

            var json = System.Text.Encoding.UTF8.GetString(arr);
            return JsonConvert.DeserializeObject<Packet>(json);
        }
    }
}
