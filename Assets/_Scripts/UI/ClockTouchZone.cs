using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using UnityEngine.Windows;

public class ClockTouchZone : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private Vector2 touchedPos;
    public static Vector2 toDragedPos;

    private void Start()
    {
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        touchedPos = eventData.position;
        ClockManager.instance.ClockBegin();
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        toDragedPos = eventData.position - touchedPos;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        ClockManager.instance.ClockEnd();
    }
}
