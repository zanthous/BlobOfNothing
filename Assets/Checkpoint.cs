using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private Player player;
    private bool activated = false;
    private SpriteRenderer sprite;

    [SerializeField]
    private Sprite activeSprite;
    [SerializeField]
    private AudioSource checkpoint;
    
    private void Start()
    {
        player = FindObjectOfType<Player>();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(activated)
            return;
        if(collision.tag == "Player")
        {
            player.LastCheckpoint = gameObject;
            activated = true;
            sprite.sprite = activeSprite;
            if(checkpoint!=null)
                checkpoint.Play();
            player.NStars++;
        }
    }
}
