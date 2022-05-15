using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveSystem : MonoBehaviour
{
    public static bool SaveExists(string saveName)
    {
        string path = Application.persistentDataPath + "/" + saveName + ".dot";
        return File.Exists(path);
    }

    public static void Save(string saveName, object data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/" + saveName + ".dot";
        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static object Load(string saveName)
    {
        string path = Application.persistentDataPath + "/" + saveName + ".dot";
        if (!File.Exists(path))
        {
            Debug.Log("File not found in " + path);
            return null;
        }
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Open);
        object data = formatter.Deserialize(stream);
        stream.Close();
        return data;
    }
}
