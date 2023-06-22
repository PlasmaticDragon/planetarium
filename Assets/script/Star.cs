using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Star : MonoBehaviour
{
    [Header("Data")]
    public string constellation;
    public double longitude;
    public double latitude;
    public double magnitude;
    public string name;

    public GameObject dialog;

    public TMP_Text text;

    void Awake()
    {
        SaveSystem.stars.Add(this);
        dialog = GameObject.Find("[ Dialog UI ]");
        text = GameObject.Find("[ Dialog UI ]/Canvas/Panel/Text (TMP)").GetComponent<TMP_Text>(); 
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnDestroy()
    {
        SaveSystem.stars.Remove(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown() 
    {
        dialog.SetActive(true);
        text.text = name;
    }

}
