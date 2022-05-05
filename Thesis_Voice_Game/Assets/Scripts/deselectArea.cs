using UnityEngine;
using UnityEngine.EventSystems;

public class deselectArea : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] GameObject panel;
    //Detect current clicks on the GameObject (the one with the script attached)
    public void OnPointerDown(PointerEventData pointerEventData)
    {
        panel.SetActive(false);
        Debug.Log("clicking on " + name);
    }

}
