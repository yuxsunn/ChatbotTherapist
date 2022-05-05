using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class nickname : MonoBehaviour
{
    public TMP_Text Title;
    public TMP_Text Name;
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("name"))
        {
            Name.text = PlayerPrefs.GetString("name");
            Title.enabled = true;
        } else
        {
            Name.text = "";
            Title.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
