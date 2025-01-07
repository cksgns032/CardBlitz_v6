using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class HorizontalExtension : HorizontalLayoutGroup
{
    public float heigth;
    public float width;
    RectTransform rect;

    public void Init()
    {
        rect = GetComponent<RectTransform>();
    }
    public void Resizer()
    {
        rect.sizeDelta = new Vector2(width * gameObject.transform.childCount, heigth);
    }
}
