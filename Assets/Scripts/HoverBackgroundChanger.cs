using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HoverBackgroundChanger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image backgroundImage;
    public Sprite hoverBackground;
    public Sprite defaultBackground;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(backgroundImage != null && hoverBackground != null)
            backgroundImage.sprite = hoverBackground;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(backgroundImage != null && defaultBackground != null)
            backgroundImage.sprite = defaultBackground;
    }
}
