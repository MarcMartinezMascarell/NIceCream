using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
 
public class ClickableObject : MonoBehaviour, IPointerClickHandler {
    public static UnityAction onLeftClick;
    public static UnityAction onRightClick;
    public static UnityAction onMiddleClick;
    protected Button button;
 
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            onLeftClick.Invoke();
        }
        else if (eventData.button == PointerEventData.InputButton.Middle)
            onMiddleClick.Invoke();
        else if (eventData.button == PointerEventData.InputButton.Right)
            onRightClick.Invoke();
    }
}