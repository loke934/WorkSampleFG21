using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary; //turning data to binary
using System.IO;
public static class SaveSystem 
{
    public static void SaveHighscores(HighscoreData data)
    {
        BinaryFormatter bf = new BinaryFormatter();
        string path = Path.Combine(Application.persistentDataPath, "Highscores.bin");
        FileStream fs = new FileStream(path, FileMode.Create);
        bf.Serialize(fs, data);
        fs.Close();
    }

    public static HighscoreData LoadHighscores()
    {
        string path = Path.Combine(Application.persistentDataPath, "Highscores.bin");
        if (File.Exists(path))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream(path, FileMode.Open);
            HighscoreData data = bf.Deserialize(fs) as HighscoreData;
            //converts binary and read it as highscoredata
            fs.Close();
            return data;
        }
        else
        {

            Debug.Log("Save file not found in " + path);
            return null;
        }
    }
}
