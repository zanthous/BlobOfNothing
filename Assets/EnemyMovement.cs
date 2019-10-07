using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour, IHaveSpeedModifier
{
    private GameObject playerRef;
    [SerializeField]
    private float chaseRange = 7.0f;
    private EnemyState state = EnemyState.sleep;

    private float timer = 0.0f;
    private float dirChangeTime = 3.0f;

    private Vector2 direction;

    [SerializeField]
    private float baseMoveSpeed = 800.0f;
    private float moveSpeedModifier = 1.0f;

    private Rigidbody2D rb;

    private GameObject targetItem;

    enum EnemyState
    {
        sleep,
        patrol,
        chase,
        gettingItem
        //avoid
    }
    // Start is called before the first frame update
    void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
    }

    public void UpdateMoveSpeedModifier()
    {
        var speedMods = GetComponentsInChildren<SpeedModifier>();
        moveSpeedModifier = 1.0f;
        if(speedMods.Length == 0)
        {
            return;
        }
        for(int i = 0; i < speedMods.Length; i++)
        {
            moveSpeedModifier += speedMods[i].Amount;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float distanceToItem = float.MaxValue;
        int partIndex = -1;
        float result;
        float distanceToPlayer;
        switch(state)
        {
            case EnemyState.sleep:
                direction = Vector2.zero;
                for(int i = 0; i < PartManager.Instance.freeParts.Count; i++)
                {
                    result = Vector2.Distance(PartManager.Instance.freeParts[i].transform.position, transform.position);
                    if(result < distanceToItem)
                    {
                        distanceToItem = result;
                        partIndex = i;
                    }
                }
                distanceToPlayer = Vector2.Distance(playerRef.transform.position, transform.position);

                if(distanceToPlayer < distanceToItem && distanceToPlayer < chaseRange)
                {
                    state = EnemyState.chase;
                }
                else if(distanceToItem < distanceToPlayer && distanceToItem < chaseRange)
                {
                    state = EnemyState.gettingItem;
                    targetItem = PartManager.Instance.freeParts[partIndex];
                }
                break;
            //case EnemyState.patrol:
            //    //find closest item
            //    for(int i = 0; i < PartManager.Instance.freeParts.Count; i++)
            //    {
            //        result = Vector2.Distance(PartManager.Instance.freeParts[i].transform.position, transform.position);
            //        if(result < distanceToItem)
            //        {
            //            distanceToItem = result;
            //            partIndex = i;
            //        }
            //    }
            //     distanceToPlayer = Vector2.Distance(playerRef.transform.position, transform.position);

            //    if(distanceToPlayer<distanceToItem && distanceToPlayer < chaseRange)
            //    {
            //        state = EnemyState.chase;
            //    }
            //    else if(distanceToItem < distanceToPlayer && distanceToItem < chaseRange)
            //    {
            //        state = EnemyState.gettingItem;
            //        targetItem = PartManager.Instance.freeParts[partIndex];
            //    }
            //    else
            //    {
            //        if(timer>dirChangeTime)
            //        {
            //            direction = UnityEngine.Random.insideUnitCircle.normalized;
            //            //TODO avoid spikes
            //            timer = 0.0f;
            //        }
            //        timer += Time.deltaTime;
            //    }
            //    break;
            case EnemyState.chase:
                if(Vector2.Distance(playerRef.transform.position, transform.position) >= chaseRange)
                {
                    state = EnemyState.sleep;
                }
                else
                {
                    //chase code
                    direction = (playerRef.transform.position - transform.position).normalized;
                }
                break;
            case EnemyState.gettingItem:
                if(targetItem.GetComponent<Part>().Free == false)
                {
                    state = EnemyState.sleep;
                    timer = dirChangeTime;
                }
                else
                {
                    direction = (targetItem.transform.position - transform.position).normalized;
                }
                break;
                //if( vector)
        }
        rb.AddForce(direction  * Time.deltaTime * baseMoveSpeed * moveSpeedModifier, ForceMode2D.Force);
    }

}
