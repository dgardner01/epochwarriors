using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Coffee.UIExtensions;

public class TextParticle : MonoBehaviour
{
    float lifetime;
    public float maxLifetime;
    public float gravityScale;
    public float xForce;
    public float yForce;
    public AnimationCurve scale;
    Rigidbody2D rb2D => GetComponent<Rigidbody2D>();

    private void Start()
    {
        rb2D.gravityScale = gravityScale;
        rb2D.velocity = new Vector2(Random.Range(-xForce, xForce), yForce);
    }
    private void Update()
    {
        transform.localScale = Vector3.one * scale.Evaluate(lifetime / maxLifetime);
    }
    private void FixedUpdate()
    {
        if (lifetime < maxLifetime)
        {
            lifetime += Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
