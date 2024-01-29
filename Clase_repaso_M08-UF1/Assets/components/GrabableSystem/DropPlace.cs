using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class DropPlace : MonoBehaviour
{
    [System.Flags]
    public enum CheckMode
    {
        CheckObject = 1,
        CheckGrabbableType = 2
    }
    
    [Header("SetUp")]
    [SerializeField] private List<Grabbable> _validGrabbables = new();
    [SerializeField] private Grabbable.GrabbableType _validGrabbablesType = new();
    [SerializeField] private CheckMode _checkMode;

    [Header("Event")]
    public UnityEvent<GameObject> OnObjectDropped;
    public UnityEvent<GameObject> OnObjectGrabbed;

    public bool isValid(Grabbable grabbable)
    {
        bool IsValid = true;


        if (_checkMode.HasFlag(CheckMode.CheckObject)){
            if(_validGrabbables.Contains(grabbable))
            {
                return true;
            }
        }

        if (_checkMode.HasFlag(CheckMode.CheckGrabbableType))
        {
            if (_validGrabbablesType.HasFlag(grabbable._grabbableType)){
                IsValid = false;
            }
        }

        return IsValid;
    }

    public void OnDrop(Grabbable grabbable)
    {
        OnObjectDropped.Invoke(grabbable.gameObject);
        grabbable.OnStartGrab.AddListener(OnGrab);
    }

 
    private void OnGrab(GameObject grabbableObject, GameObject parent)
    {
        if(grabbableObject.TryGetComponent(out Grabbable grabbable)) { 
            grabbable.OnStartGrab.RemoveListener(OnGrab);
        }
        OnObjectGrabbed.Invoke(grabbableObject.gameObject);
    }
}
