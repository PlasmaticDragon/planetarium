using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CSVReader : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] Star starPrefab;

    public TextAsset textAssetData;
    private int offset = 50000;
    public GameObject dialog;

    [System.Serializable]
    public class StarDat
    {
        public string name;
        public double longitude;
        public double latitude;
        public double magnitude;
    }

    [System.Serializable]
    public class StarDatList
    {
        public StarDat[] star;
    }

    public StarDatList myStarDatList = new StarDatList();

    // Start is called before the first frame update
    void Start()
    {
        Screen.autorotateToPortrait = true;

        Screen.autorotateToPortraitUpsideDown = true;

        Screen.orientation = ScreenOrientation.AutoRotation;
        dialog.SetActive(true);
        ReadCSV();
    }

    int columns = 8;

    void ReadCSV()
    {
        string[] data = textAssetData.text.Split(new string[] {",","\n"}, StringSplitOptions.None);

        int tableSize = data.Length / columns - 1;

        for (int i = 0; i < tableSize; i++)
        {
            if (data[columns * i] == "") 
            {
                continue;
            }
            double longitude = double.Parse(data[columns * (i) + 1]);
            double latitude = double.Parse(data[columns * (i) + 2]);
            double apparent_magnitude = double.Parse(data[columns * (i) + 3]);

            double Rlong = (longitude * (Math.PI)) / 180;
            double Rlat = (latitude * (Math.PI)) / 180;

            double magnitude = Math.Pow((1/2.512), apparent_magnitude);

            double x = offset * magnitude * Math.Cos(Rlat) * Math.Sin(Rlong);
            double z = offset * magnitude * Math.Cos(Rlat) * Math.Cos(Rlong);
            double y = offset * magnitude * Math.Sin(Rlat);

            if(latitude == 44.38) {
                Debug.Log(Math.Cos(latitude));
                Debug.Log(Math.Cos(longitude));
                Debug.Log(magnitude);
                Debug.Log(x);
            }

            Vector3 position = new Vector3((float)-x, (float)y, (float)z);

            Star star = Instantiate<Star>(starPrefab, position, Quaternion.identity);

            star.name = data[columns * (i)];
            star.longitude = double.Parse(data[columns * (i) + 1]);
            star.latitude = double.Parse(data[columns * (i) + 2]);
            star.magnitude = double.Parse(data[columns * (i) + 3]);
        }

        dialog.SetActive(false);
    }

}
