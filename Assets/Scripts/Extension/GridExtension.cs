using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridExtension : GridLayoutGroup
{
    RectTransform rect;

    public void Init()
    {
        rect = GetComponent<RectTransform>();
    }
    public void Resizer()
    {
        if(constraint == Constraint.FixedRowCount)
        {
            //if (rect.sizeDelta.x >)
                rect.sizeDelta = new Vector2(cellSize.x * gameObject.transform.childCount, cellSize.y * 2);
        }
        else if(constraint == Constraint.FixedColumnCount)
        {
            rect.sizeDelta = new Vector2(cellSize.x * gameObject.transform.childCount, cellSize.y * 2);
        }
    }
}
