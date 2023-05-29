using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextSortLayer : MonoBehaviour
{
    // 将文本父物体渲染顺序赋值给当前文本
    void Start()
    {
        var parentRenderer = transform.parent.GetComponent<Renderer>();
        var render = GetComponent<Renderer>();
        render.sortingLayerName = parentRenderer.sortingLayerName;
        render.sortingOrder = parentRenderer.sortingOrder;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
