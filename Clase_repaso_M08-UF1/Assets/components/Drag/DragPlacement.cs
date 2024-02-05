using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragPlacement : MonoBehaviour, IDropHandler
{
    [Header("setup")]
    [SerializeField] private RectTransform _parentRectTransform;



    [Header("events")]
    [SerializeField] private UnityEvent<GameObject> _startOnDrop;

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("object droped over " + name);
        GameObject droped = eventData.pointerDrag;
        if (droped.TryGetComponent(out DragObject cell))
        {
            cell._targetParent = _parentRectTransform;

            _startOnDrop.Invoke(droped);
        }
    }

    public void LiveObject()
    {

    }

   

   
}
