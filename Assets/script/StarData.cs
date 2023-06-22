using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StarData
{
    //Replace these example variable with your objects variables
    //that you wish to save
    public string constellation;
    public double longitude;
    public double latitude;
    public double magnitude;
    public string name;
    public float[] position;

    public StarData(Star star)
    {
        constellation = star.constellation;
        longitude = star.longitude;
        latitude = star.latitude;
        magnitude = star.magnitude;
        name = star.name;

        Vector3 starPos = star.transform.position;

        position = new float[]
        {
            starPos.x, starPos.y, starPos.z
        };
    }
}
