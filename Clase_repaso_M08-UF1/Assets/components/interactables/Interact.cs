using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


public class Interact : MonoBehaviour
{
    [Header("debug Support")]
    [SerializeField] private Interactable _currentInteractable;

    [Header("Setup")]
    [SerializeField] private float _radiusRange = 0.5f;
    public float RadiudRange { get => _radiusRange; }

    private IEnumerator _chackBreackDistanceCorroutine;

    public void OnInteract(InputValue Value)
    {
        if (Value.isPressed)
        {
            TryStartInterct();
        }
        else
        {
            TryEndInteract();
        }
    }

    private void TryStartInterct()
    {
        Vector3 position = transform.position;
        int inversetMask = ~(gameObject.layer);
        List<Collider> colliders = Physics.OverlapSphere(position, _radiusRange, inversetMask, QueryTriggerInteraction.Collide).ToList();

        colliders.Sort((a, b) => {

            float dA = Vector3.Distance(a.ClosestPoint(position), position);
            float dB = Vector3.Distance(b.ClosestPoint(position), position);
            return dA.CompareTo(dB);
        
        });
        foreach (Collider collider in colliders)
        {
            if(collider.gameObject.TryGetComponent<Interactable>(out Interactable interactable))
            {

                _currentInteractable = interactable;
                _currentInteractable.StartInteract(this.gameObject);
                if (interactable.breakWhithDistance)
                {
                    _chackBreackDistanceCorroutine = checkBreackDistanceCorrutine(collider);
                    StartCoroutine(_chackBreackDistanceCorroutine);
                }

                return; //early Exit
            }
        }

    }
    private void TryEndInteract()
    {
        if(_currentInteractable != null)
        {
            _currentInteractable.EndInteraction(this.gameObject);
            _currentInteractable=null;
        }

        if( _chackBreackDistanceCorroutine != null)
        {
            StopCoroutine(_chackBreackDistanceCorroutine);
            _chackBreackDistanceCorroutine = null;
        }
    }

   private IEnumerator checkBreackDistanceCorrutine(Collider collider)
    {
        while(Vector3.Distance(collider.ClosestPoint(transform.position), transform.position) <= _radiusRange)
        {
            yield return null;  
        }

        TryEndInteract();
    }

#if UNITY_EDITOR  //se activa si esta en el editor de unity si no no se activa
    private void OnDrawGizmosSelected()
    {
                Gizmos.DrawWireSphere(transform.position, _radiusRange);
    }
#endif
}
