using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
[RequireComponent(typeof(CanvasGroup))]

public class DragObject : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{


    [Header("setUp")]
    private CanvasGroup label;
    private RectTransform _rect;
    public Transform _targetParent;


    [Header("events")]
    [SerializeField] private UnityEvent<GameObject, GameObject> _startOnGrab;
    [SerializeField] private UnityEvent<GameObject, GameObject> _startOnEndGrab;

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("start drag");
        label.blocksRaycasts = false;
        _targetParent = _rect.parent;

        _rect.SetParent(GetComponentInParent<DragContiner>().Rect);
        this.gameObject.GetComponentInParent<DragPlacement>().LiveObject();

    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag");
        if (eventData.pointerEnter == null || eventData.pointerEnter.transform as RectTransform == null)
        {
            return;
        }
        RectTransform plane = eventData.pointerEnter.transform as RectTransform;

        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(plane, eventData.position, eventData.pressEventCamera, out Vector3 globalMousePos))
        {
            _rect.position = globalMousePos;
            _rect.rotation = plane.rotation;
        }

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _rect.SetParent(_targetParent);
        label.blocksRaycasts = true;

        Debug.Log("end Drag");
    }

}
