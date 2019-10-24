using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Player player;
    
    [SerializeField] float baseMoveSpeed = 800.0f;
    private float moveSpeedModifier = 1.0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GetComponent<Player>();
    }
    
    void FixedUpdate()
    {
        if(!player.Dead)
        {
            rb.AddForce(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized
                * Time.deltaTime * baseMoveSpeed * (1.0f + player.Stats.BonusMoveSpeed)
                , ForceMode2D.Force);
        }
    }
}
