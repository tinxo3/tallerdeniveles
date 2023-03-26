using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

    public static class SaveSystem
    {
        public static void SaveGame(HairyTroublesData data)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            string path = Application.persistentDataPath + "/HairyTroubles.fun";
            FileStream stream = new FileStream(path, FileMode.Create);



            formatter.Serialize(stream, data);
            stream.Close();
        }

        public static HairyTroublesData LoadGame()
        {
            string path = Application.persistentDataPath + "/HairyTroubles.fun";
            if (File.Exists(path))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Open);

                HairyTroublesData data = formatter.Deserialize(stream) as HairyTroublesData;
                stream.Close();

                return data;
            }
            else
            {
                Debug.LogError("Save file not found in" + path);
                HairyTroublesData data = new HairyTroublesData();
                return data;
            }
        }
    }