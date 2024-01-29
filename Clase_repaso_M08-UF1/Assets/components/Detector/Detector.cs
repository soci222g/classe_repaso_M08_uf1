using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



public class Detector : MonoBehaviour
{
    [Header("Objects Setup")]
    [SerializeField, Min(1)] private uint _RequiredAmountObjects = 1;
    [SerializeField]   private List<GameObject> _ObjectsInside = new();


    [Header("Eventos")]
    public UnityEvent OnActivate;
    public UnityEvent OnDeactivate;

    private void OnTriggerEnter(Collider other)
    {
        _ObjectsInside.Add(other.gameObject);

        if(_ObjectsInside.Count == _RequiredAmountObjects)
        {
            OnActivate.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _ObjectsInside.Remove(other.gameObject);
        if( _ObjectsInside.Count == _RequiredAmountObjects - 1)
        {
            OnDeactivate.Invoke();
        }
    }
}
