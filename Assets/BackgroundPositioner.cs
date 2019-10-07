using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BackgroundPositioner : MonoBehaviour
{
    private GameObject playerRef;
    [SerializeField]
    private Tilemap tilemap;
    private float xMax = 5.0f;
    private float yMax = 5.0f;
    private float tmWidth;
    private float tmHeight;
    void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
        tmWidth = tilemap.localBounds.max.x - tilemap.localBounds.min.x;
        tmHeight = tilemap.localBounds.max.y - tilemap.localBounds.min.y;
    }

    // Update is called once per frame
    void Update()
    {
        var bounds = tilemap.localBounds;
        
        gameObject.transform.position = new Vector3
        (
            (playerRef.transform.position.x  / tmWidth)* xMax,
            (playerRef.transform.position.y / tmHeight)* yMax,
            0.0f
        );
    }
}
