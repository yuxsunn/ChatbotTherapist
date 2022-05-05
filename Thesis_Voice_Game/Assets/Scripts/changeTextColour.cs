using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//a simple script to change the colour of button text when button is selected
public class changeTextColour : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    void ISelectHandler.OnSelect(BaseEventData eventData)
    {
        gameObject.GetComponentInChildren<Text>().color = new Color(255, 255, 255, 255);

    }

    public void OnDeselect(BaseEventData eventData)
    {
        gameObject.GetComponentInChildren<Text>().color = new Color(0, 0, 0, 255);

    }
}
