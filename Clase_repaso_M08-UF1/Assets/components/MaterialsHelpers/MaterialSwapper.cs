using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class MaterialSwapper : MonoBehaviour
{
    private MeshRenderer _renderes;

    [SerializeField] private Material _materialToSwap;
    private Material _InitialMaterial;


    private void Start()
    {
        _renderes = GetComponent<MeshRenderer>();
        _InitialMaterial = _renderes.material;
    }

    public void SetSecondaryMaterial()
    {

       

        if( _materialToSwap != null )
        {
           _renderes.material = _materialToSwap;
        }
       
    }

    public void SetPrimaryMaterial()
    {
        if( _InitialMaterial != null)
        {
            _renderes.material = _InitialMaterial;
        }
       
    }
}
