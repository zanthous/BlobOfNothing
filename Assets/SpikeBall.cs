using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeBall : MonoBehaviour
{
    [SerializeField] private float reboundForce;
    private float damage = 34.0f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("hit");
        var other = collision.gameObject;
        var health = other.GetComponent<Health>();
        var rb = other.GetComponent<Rigidbody2D>();
        
        if( health!=null && rb != null)
        {
            rb.AddForce((other.transform.position - transform.position).normalized*reboundForce, ForceMode2D.Impulse);
            health.ChangeHealth(-damage);
        }
    }
}
