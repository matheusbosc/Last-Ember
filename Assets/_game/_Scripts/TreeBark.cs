using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class TreeBark : MonoBehaviour
{
    Rigidbody2D rb;
    public float speed;
    public float damage = 7.5f;

    private void Start()
    {
        rb  = GetComponent<Rigidbody2D>();
        var size = Random.Range(0.08830507f, 0.1366f);
        var rotation = Random.Range(0, 360);
        
        transform.localScale = new Vector3(size, size, size);
        transform.rotation = Quaternion.Euler(0, 0, rotation);
    }

    private void Update()
    {
        bool isPaused = GameManager.instance.isPaused;
        
        rb.linearVelocity = isPaused ? Vector2.zero : new Vector2(1 * speed, 0);

        if (transform.position.x >= Random.Range(70,100))
        {
            ResourceManager.instance.RemoveBark();
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Campfire"))
        {
            SoundManager.instance.PickupItem();
            other.gameObject.GetComponent<Fire>().fireHealth -= damage;
            ResourceManager.instance.RemoveBark();
            Destroy(gameObject);
        }
    }
}
