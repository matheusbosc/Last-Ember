using System;
using UnityEngine;

public class DynamicYSort : MonoBehaviour
{
    private int _baseSortingOrder;
    [SerializeField] private SortableSprite[] _sortableSprites;
    

    private void Update()
    {
        _baseSortingOrder = -(int)((transform.position.y) * 100);
        foreach (var sprite in _sortableSprites)
        {
            sprite.spriteRenderer.sortingOrder = _baseSortingOrder + sprite.relativeOrder;
        }
    }

    [Serializable]
    public struct SortableSprite
    {
        public SpriteRenderer spriteRenderer;
        public int relativeOrder;
    }
}
