using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class LoadAdresableOnPress : MonoBehaviour
{

    [SerializeField] AssetReference prefabReference;
    public void OnPress()
    {
        prefabReference.InstantiateAsync();
    }

    private void OnEnable()
    {
        UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<GameObject> loadAsyncOperation = prefabReference.LoadAssetAsync<GameObject>();

        loadAsyncOperation.Completed += OnLoadCcomplered;
    }

    private void OnLoadCcomplered(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<GameObject> obj)
    {

    }
    private void OnDisable()
    {
        prefabReference.ReleaseAsset();
    }
}
