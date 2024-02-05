using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TextUICell : UICell, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public TextMeshProUGUI label;

    private RectTransform _rect;
    public Transform _targetParent;


    private void Start()
    {
        _rect = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("start drag");
        label.raycastTarget = false;
        _targetParent = _rect.parent;

        _rect.SetParent(GetComponentInParent<DragContiner>().Rect);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag");
        if(eventData.pointerEnter == null || eventData.pointerEnter.transform as  RectTransform == null)
        {
            return;
        }
        RectTransform plane = eventData.pointerEnter.transform as RectTransform;

        if(RectTransformUtility.ScreenPointToWorldPointInRectangle(plane,eventData.position, eventData.pressEventCamera,out Vector3 globalMousePos))
        {
            _rect.position = globalMousePos;
            _rect.rotation = plane.rotation;
        }

    }

    public void OnEndDrag(PointerEventData eventData)
    {   
        _rect.SetParent(_targetParent);
        label.raycastTarget = true;
        Debug.Log("end Drag");
    }
}
