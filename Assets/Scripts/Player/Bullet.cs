using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifeTime = 5f;
    private float speedBullet = 10f;

    void Awake()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.Translate(Vector2.right * speedBullet * Time.deltaTime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
