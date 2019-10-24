using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Needle : MonoBehaviour
{
    private GameObject owner;
    private Part part;
    private float shootForce = 15.0f;
    private float shootRange = 4.0f;
    private bool hasShot = false;
    private float damage = 40.0f;
    private float knockBackForce = 10.0f;
    
    private void Awake()
    {
        part = GetComponent<Part>();
    }

    private void Update()
    {
        if(!part.OwnedByPlayer && part.Attached)
        {
            int mask = ~(1 << 8);
            RaycastHit2D hit;
            Debug.DrawRay(transform.position, transform.up * 5.0f,Color.red);
            if(hit = Physics2D.Raycast(transform.position, transform.up, 5.0f,mask))
            {
                if(hit.transform.tag == "Player")
                {
                    owner = transform.parent.gameObject;
                    Shoot();
                }
            }
        }
        else if( part.OwnedByPlayer && part.Attached)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                owner = transform.parent.gameObject;
                Shoot();
            }
        }
    }

    private void Shoot()
    {
        hasShot = true;
        var r = gameObject.AddComponent<Rigidbody2D>();
        r.constraints = RigidbodyConstraints2D.FreezeRotation;
        r.drag = 0.0f;
        r.gravityScale = 0.0f;
        part.ShootFromTarget();
        //move forward so it doesn't get stuck?
        transform.position += transform.up * .3f;
        r.AddForce(transform.up * shootForce, ForceMode2D.Impulse);
        Debug.Log(r.drag);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(!hasShot)
            return;
        //Entity that shot this should not get hit by it
        if(collision.gameObject == owner)
            return;

        Collider2D c = collision.contacts[0].collider;

        //Hit body
        var h = c.gameObject.GetComponent<Health>();
        var rb = c.gameObject.GetComponent<Rigidbody2D>();

        if(h != null && rb != null)
        {
            h.ChangeHealth(-damage);
            rb.AddForce((c.transform.position - transform.position).normalized * knockBackForce, ForceMode2D.Impulse);
        }
        //TODO
        //else
        //{
        //    var p = c.gameObject.GetComponent<Part>();
        //    if(p!=null)
        //        p.DetachFromTarget();
        //}

        hasShot = false;
        Destroy(gameObject.GetComponent<Rigidbody2D>());
        StartCoroutine(ReEnablePart());
    }

    private IEnumerator ReEnablePart()
    {
        yield return new WaitForSeconds(.2f);
        part.ShootCleanup();
    }
}
