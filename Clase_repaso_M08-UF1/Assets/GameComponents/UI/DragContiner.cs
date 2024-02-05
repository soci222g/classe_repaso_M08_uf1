using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragContiner : MonoBehaviour
{
    [NonSerialized] public RectTransform Rect;

    private void Start()
    {
        Rect = GetComponent<RectTransform>();
    }
}
