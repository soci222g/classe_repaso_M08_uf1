using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public abstract class UITable<T> : MonoBehaviour where T : UICell
{
    [SerializeField] protected ScrollRect _scrollRect;
    [SerializeField] private T _baseCell;

    protected virtual void Start()
    {
       if(_scrollRect == null)
        {
            Debug.LogWarning("No have scroll rect in UITable name: " + name);
            return;
        }
        ReloadTable();

    }


    public void ReloadTable()
    {
        RectTransform parent = _scrollRect.content;
        
        int chillCount = parent.childCount;

        // llimpiamos el parent
        for (int i = 0; i < chillCount; i++)
        {
            GameObject gO = parent.GetChild(i).gameObject;
            if(gO != _baseCell.gameObject)
            {
                Destroy(gO);
            }
        }
        int cellsCount = TotalCellCount;
        _baseCell.gameObject.SetActive(true);
        // rellenar el parent
        for(int i = 0;i < cellsCount;i++)
        {
            T cell = Instantiate(_baseCell, parent);
            cell._Index = i;

            SetupCell(cell);
        }

        _baseCell.gameObject.SetActive(false);

    }

    public abstract int TotalCellCount {  get; }


    public abstract void SetupCell (T cell);
}
