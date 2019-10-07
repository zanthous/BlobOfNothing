using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horn : MonoBehaviour
{
    [SerializeField]
    private float damage;
    [SerializeField]
    private float knockBackForce;
    //cap on how often damage/knockback can be applied
    private float damageInterval = 0.5f;

    [SerializeField]
    private AudioSource open;

    public AudioSource Open { get => open; set => open = value; }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Collider2D c = collision.contacts[0].collider;
         
        if(transform.parent == null)
            return;
        if(c.transform == transform.parent.transform)
            return;

        //Hit body
        var h = c.gameObject.GetComponent<Health>();
        var rb = c.gameObject.GetComponent<Rigidbody2D>();

        if(h == null || rb == null)
            return;

        h.ChangeHealth(-damage);
        rb.AddForce((c.transform.position - transform.position).normalized * knockBackForce,ForceMode2D.Impulse);
        
        //hit Part? todo


    }
}
