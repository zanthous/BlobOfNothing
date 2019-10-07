using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWood : MonoBehaviour
{

    [SerializeField] private AudioSource open;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        Collider2D c = collision.contacts[0].collider;
        var a = c.gameObject.GetComponent<Horn>();
        if(a!=null)
        {
            a.Open.Play();
            Destroy(gameObject);
        }
    }
}
