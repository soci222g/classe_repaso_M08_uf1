using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class DropPlace : MonoBehaviour
{
    [System.Flags]
    public enum CheckMode
    {
        CheckObject = 1,
        CheckObjectType = 2
    }


    public enum PlaceMode
    {
        teleport,
        smuth
    }
    
    [Header("SetUp")] 
    [SerializeField] private CheckMode _checkMode;
    [SerializeField] private List<Grabbable> _validGrabbables = new();
    [SerializeField, Min(0)] private float _smoothTime = 0.25f;
    [SerializeField] private List<ObjectType> _validObjectTypes = new();
    [SerializeField] private PlaceMode _placeMode;

    private Vector3 _currentVelocity = Vector3.zero;

    [Header("Event")]
    public UnityEvent<GameObject> OnObjectDropped;
    public UnityEvent<GameObject> OnObjectGrabbed;

    public bool isValid(Grabbable grabbable)
    {
        if (_checkMode.HasFlag(CheckMode.CheckObject)){
            if(_validGrabbables.Contains(grabbable))
            {
                return true;
            }
        }
        /*
        if (_checkMode.HasFlag(CheckMode.CheckGrabbableType))
        {
            if (_validGrabbablesType.HasFlag(grabbable._grabbableType)){
                IsValid = false;
            }
        }
        */

        if (_checkMode.HasFlag(CheckMode.CheckObjectType)) { 
            foreach(ObjectType objectType in grabbable.objectTypes)
            {
                if (_validObjectTypes.Contains(objectType))
                {
                    
                    return true;
                }
            }
        }

        return false;
    }

    public void OnDrop(Grabbable grabbable)
    {
        switch (_placeMode)
        {
            case PlaceMode.teleport:
                grabbable.gameObject.transform.position = transform.position;
                break;
            case PlaceMode.smuth:
                transform.position = Vector3.SmoothDamp(grabbable.gameObject.transform.position, transform.position, ref _currentVelocity, _smoothTime);
                break;
        }

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
