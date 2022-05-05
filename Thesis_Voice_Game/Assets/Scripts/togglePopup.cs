using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class togglePopup : MonoBehaviour
{
    public GameObject Panel;

    public void popup()
    {
        if (Panel != null)
        {
            bool isActive = Panel.activeSelf;
            Panel.SetActive(!isActive);
        }
    }
}
