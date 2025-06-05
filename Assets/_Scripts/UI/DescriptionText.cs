using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DescriptionText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] string description;
    [SerializeField] TMP_Text text;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void OnPointerEnter(PointerEventData eventData)
    {
        text.text = description;
    }
    // Update is called once per frame
    public void OnPointerExit(PointerEventData eventData)
    {
        
    }
}
