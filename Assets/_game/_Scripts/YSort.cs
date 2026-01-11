using System;
using UnityEngine;

public class YSort : MonoBehaviour
{
    
    private void Start()
    {
        var spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = -(int)((transform.position.y) * 100);
    }
}
