using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;

public class SaveSystem : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] Star starPrefab;

    public static List<Star> stars = new List<Star>();

    //Rename your strings according to what your saving
    const string STAR_SUB = "/star";
    const string STAR_COUNT_SUB = "/star.count";

    void Awake()
    {
        LoadStar();
    }

    //Use if Android
    //void OnApplicationPause(bool pause)
    //{
    //    SaveStar();
    //}

    void OnApplicationQuit()
    {
        SaveStar();
    }

    void SaveStar()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + STAR_SUB + SceneManager.GetActiveScene().buildIndex;
        string countPath = Application.persistentDataPath + STAR_COUNT_SUB + SceneManager.GetActiveScene().buildIndex;

        FileStream countStream = new FileStream(countPath, FileMode.Create);

        formatter.Serialize(countStream, stars.Count);
        countStream.Close();

        for (int i = 0; i < stars.Count; i++)
        {
            FileStream stream = new FileStream(path + i, FileMode.Create);
            StarData data = new StarData(stars[i]);

            formatter.Serialize(stream, data);
            stream.Close();
        }
    }

    void LoadStar()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + STAR_SUB + SceneManager.GetActiveScene().buildIndex;
        string countPath = Application.persistentDataPath + STAR_COUNT_SUB + SceneManager.GetActiveScene().buildIndex;
        int starCount = 0;

        if (File.Exists(countPath))
        {
            FileStream countStream = new FileStream(countPath, FileMode.Open);

            starCount = (int)formatter.Deserialize(countStream);
            countStream.Close();

        }
        else
        {
            Debug.LogError("Path not found in " + countPath);
        }

        for (int i = 0; i < starCount; i++)
        {
            if (File.Exists(path + i))
            {
                FileStream stream = new FileStream(path + i, FileMode.Open);
                StarData data = formatter.Deserialize(stream) as StarData;

                stream.Close();

                Vector3 position = new Vector3(data.position[0], data.position[1], data.position[2]);
                
                Star star = Instantiate<Star>(starPrefab, position, Quaternion.identity);

                star.constellation = data.constellation;
                star.longitude = data.longitude;
                star.latitude = data.latitude;
                star.magnitude = data.magnitude;
                star.name = data.name;
            }
            else
            {
                Debug.LogError("Path not found in " + (path + i));
            }
        }
    }
}
