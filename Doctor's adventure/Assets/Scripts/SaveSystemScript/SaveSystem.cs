using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem 
{
    /// <summary>
    /// Átalakítja a játékos adatait a PlayerData osztály alapján egy fájlba
    /// </summary>
    public static void SavePlayer(string slot)
    {
        BinaryFormatter formatter = new BinaryFormatter();  //A kódolás kiválasztása
        string path = Application.persistentDataPath + "/player" + slot + ".sav";   //Mentési utvonal meghatározása
        FileStream stream = new FileStream(path, FileMode.Create);  //Új fájl készítése
        PlayerData data = new PlayerData(GameObject.Find("Player").GetComponent<PlayerController>());   //A mentési adatok felvétele

        formatter.Serialize(stream, data);  //Fájlba faló másolás
        stream.Close();
    }

    /// <summary>
    /// Betölti a játékos adatait a mentett fájlból
    /// </summary>
    public static PlayerData LoadPlayer(string slot)
    {
        string path = Application.persistentDataPath + "/player" + slot + ".sav";   //Utvonal meghatározása
        if (File.Exists(path))  //Ha létezik a fájl, akkor annak megnyitása majd az adatok visszatöltése
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();

            return data;
        }
        else   //Ha nem találja a file-t, akkor visszatér null értékkel
        {
            return null;
        }
    }
}
