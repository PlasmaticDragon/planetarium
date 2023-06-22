using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disable : MonoBehaviour
{
    public GameObject sphere;

    // Start is called before the first frame update
    void Start()
    {
        sphere.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
