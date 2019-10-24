using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private Sprite activeSprite;
    [SerializeField] private AudioSource checkpoint;

    private Player player;
    private SpriteRenderer sr;
    
    private bool activated = false;

    
    private void Start()
    {
        player = FindObjectOfType<Player>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(activated)
            return;
        if(collision.tag == "Player")
        {
            player.LastCheckpoint = gameObject;
            activated = true;
            sr.sprite = activeSprite;
            if(checkpoint!=null)
                checkpoint.Play();
            player.NStars++;
        }
    }
}
