using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] public bool breakWhithDistance = true;

    [Header("Events")]
    public UnityEvent<GameObject> OnStartInteract;
    public UnityEvent<GameObject> OnEndInteract;

    public virtual void StartInteract(GameObject Interactor)
    {
        OnStartInteract.Invoke(Interactor);
    }

    public virtual void EndInteraction(GameObject Interactor)
    {
        OnEndInteract.Invoke(Interactor);
    }
}
