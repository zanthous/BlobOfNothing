using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField]
    private float baseMoveSpeed = 800.0f;
    private float moveSpeedModifier = 1.0f;
    private Player player;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GetComponent<Player>();
    }

    // Update is called once per frame

    void FixedUpdate()
    {
        if(!player.Dead)
        {
            rb.AddForce(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized
                * Time.deltaTime * baseMoveSpeed * (1.0f + player.Stats.BonusMoveSpeed)
                , ForceMode2D.Force);
        }
    }

    //public void UpdateMoveSpeedModifier()
    //{
    //    var speedMods = GetComponentsInChildren<SpeedModifier>();
    //    moveSpeedModifier = 1.0f;
    //    if(speedMods.Length == 0)
    //    {
    //        return;
    //    }
    //    for(int i = 0; i < speedMods.Length; i++)
    //    {
    //        moveSpeedModifier += speedMods[i].Amount;
    //    }
    //}
}
