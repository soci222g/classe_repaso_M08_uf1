using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class TextUITable : UITable<TextUICell>, IDropHandler
{
    [SerializeField] private List<string> _allText = new();
    public override int TotalCellCount => _allText.Count;

  
    public override void SetupCell(TextUICell cell)
    {
        cell.label.text = cell._Index.ToString() + " " + _allText[cell._Index];
    } 
    
    
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("object droped over " + name);
        GameObject droped = eventData.pointerDrag;
        if(droped.TryGetComponent(out TextUICell cell))
        {
            cell._targetParent = _scrollRect.content;
        }
    }

}
