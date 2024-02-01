using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Grabbable : MonoBehaviour
{
    public enum GrabMode { FollowParentTeleport, followParentSmooth, SetOnParent, SenOnParentZeroPosition}
    

    [Header("Setup")]
    [SerializeField] private GrabMode _grabMode = GrabMode.FollowParentTeleport;
    [SerializeField, Min(0)] private float _smoothTime = 0.25f;   
    [SerializeField] public List<ObjectType> objectTypes = new();

    [Header("Event")]
    public UnityEvent<GameObject, GameObject> OnStartGrab;
    public UnityEvent<GameObject, GameObject> OnEndGrab;

    private IEnumerator _followCorutine;
    private Vector3 _currentVelocity = Vector3.zero;   

    private bool _isGrabbed=false;

    public void GrabSwitch(GameObject parent)
    {
        if (_isGrabbed)
        {
            EndGrab(parent);
        }
        else
        {
            StartGrab(parent);
        }
    }
    public void StartGrabHolder(GameObject parent) {
        StartGrab(parent);
    }

    private void StartGrab(GameObject parent)
    {
        _isGrabbed = true;  
        if(_followCorutine != null)
           {
             StopCoroutine(_followCorutine);
             _followCorutine = null;
           }
        switch (_grabMode)
        {
            case GrabMode.FollowParentTeleport: //usamos el mismo codigo para los dos casos
            case GrabMode.followParentSmooth:
                transform.parent = null;

                _followCorutine = FollowCorutine(parent);
                StartCoroutine(_followCorutine);
                break;
            case GrabMode.SetOnParent:
                transform.parent = parent.transform;
                break;
            case GrabMode.SenOnParentZeroPosition:
                transform.parent = parent.transform;
                transform.localPosition = Vector3.zero;
                break;
        }

        OnStartGrab.Invoke(this.gameObject, parent);
    }

    public void EndGrab(GameObject parent)
    {
        _isGrabbed = false;
        if (_followCorutine != null)
        {
            StopCoroutine(_followCorutine);
            _followCorutine = null;
        }

        transform.parent = null;

        TryDrop(parent);

        OnEndGrab.Invoke(this.gameObject, parent); 
    }

    private IEnumerator FollowCorutine(GameObject parent)
    {
        while (true)
        {

            switch (_grabMode)
            {
                case GrabMode.FollowParentTeleport:
                    transform.position = parent.transform.position;
                    break;
                case GrabMode.followParentSmooth:
                    transform.position = Vector3.SmoothDamp(transform.position,parent.transform.position,ref _currentVelocity, _smoothTime); 
                    break;
                case GrabMode.SetOnParent:
                case GrabMode.SenOnParentZeroPosition:
                    break;
            }
            yield return null;
        }
    }

    private void TryDrop(GameObject parent)
    {
              Interact interctorParent = parent.GetComponent<Interact>();
        if (interctorParent == null)
        {
            return;
        }

        Vector3 position = parent.transform.position;
        int inversetMask = ~(parent.layer);



        List<Collider> colliders = Physics.OverlapSphere(position, interctorParent.RadiudRange, inversetMask, QueryTriggerInteraction.Collide).ToList();

        colliders.Sort((a, b) => {

            float dA = Vector3.Distance(a.ClosestPoint(position), position);
            float dB = Vector3.Distance(b.ClosestPoint(position), position);
            return dA.CompareTo(dB);

        });
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.TryGetComponent(out DropPlace DropPlace))
            {
                if (DropPlace.isValid(this))
                {
                    DropPlace.OnDrop(this);
                }

                return; //early Exit
            }
        }

    }
}
