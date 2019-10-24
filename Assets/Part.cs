using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Part : MonoBehaviour
{
    private bool attached = false;
    public bool Attached { get => attached; set => attached = value; }
    private bool ownedByPlayer = false;
    public bool OwnedByPlayer { get => ownedByPlayer; set => ownedByPlayer = value; }

    [SerializeField] private bool free = true;
    public bool Free { get => free; set => free = value; }

    //Serialized so enemies can have pre-setup parts
    [SerializeField] private int slot = -1;
    public int Slot { get => slot; /*set => slot = value; */}

    private float detachForce = 10.0f;
    private float detachDistance = 1.5f;
    private float attachedDistance = .7f;
    private float startOrbitDistance = 1.0f;
    private float orbitDistance = 1.0f;
    private float scaleFactor = 1.0f;
    private bool orbiting = false;
    //Ordered by slot index 0-7
    private float[] slotAngles = { 0, 45, 90, 135, 180, -135, -90, -45 };

    private Player player;
    private GameObject playerRef;
    //player or enemy depending on which is picking up the part
    private GameObject target;
    private PartInventory playerInv;
    private ParticleSystem ps;
    private SpriteRenderer sprite;
    private BoxCollider2D collider; 
    private Modifier modifier;
    private EntityStats stats;
     
    void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
        playerInv = playerRef.GetComponent<PartInventory>();
        ps = GetComponent<ParticleSystem>();
        sprite = GetComponent<SpriteRenderer>();
        player = playerRef.GetComponent<Player>();
        collider = GetComponent<BoxCollider2D>();
        modifier = GetComponent<Modifier>();

        if(!free)
        { 
            ps.Stop();
            attached = true;
            collider.enabled = true;
        }
        else
        {
            collider.enabled = false;
        }

        if(transform.parent != null)
        { 
            target = transform.parent.gameObject;
            stats = target.GetComponent<EntityBase>()?.Stats;
        }

        scaleFactor = (sprite.sprite.rect.height / 16.0f) * .25f + .75f;
    }
    
    void Update()
    {
        if(attached)
        {
            MouseOver();
            return;
        }

        if(!orbiting && !attached && free)
        {
            if(TryAttachEnemy())
                return;
            if(TryAttachPlayer())
                return;
        }
        else if(orbiting)
        {
            int currentSlot;
            Vector2 snapVector;

            SnapToSlotLocation(out currentSlot, out snapVector);

            if(Input.GetMouseButtonDown(0))
            {
                AttachToTarget(currentSlot, snapVector);
            }
        }
    }
    
    private bool TryAttachEnemy()
    {
        //enemy test
        for(int i = 0; i < EnemyManager.Instance.enemies.Count; i++)
        {
            if(Vector2.Distance(transform.position, EnemyManager.Instance.enemies[i].transform.position) < startOrbitDistance)
            {
                target = EnemyManager.Instance.enemies[i];
                stats = target.GetComponent<EntityBase>()?.Stats;
                var tempSlot = target.GetComponent<PartInventory>().FirstFree();
                if(tempSlot != -1)
                {
                    free = false;
                    collider.enabled = true;
                    PartManager.Instance.RemoveFreePart(gameObject);
                    ownedByPlayer = false;
                    ps.Stop();

                    Vector2 snapVector;
                    var closestAngle = slotAngles[tempSlot];
                    snapVector = new Vector2(Mathf.Cos((closestAngle + 90) * Mathf.PI / 180.0f), Mathf.Sin((closestAngle + 90) * Mathf.PI / 180.0f));
                    transform.eulerAngles = new Vector3(0, 0, closestAngle);
                    AttachToTarget(target.GetComponent<PartInventory>().FirstFree(), snapVector);
                    return true;
                }
                else
                {
                    continue;
                }
            }
        }
        return false;
    }
    private bool TryAttachPlayer()
    {
        //player test
        if(player.PartOrbiting)
        {
            return false;
        }

        if(Vector2.Distance(playerRef.transform.position, transform.position) < startOrbitDistance)
        {
            target = playerRef;
            stats = target.GetComponent<EntityBase>()?.Stats;
            var tempSlot = target.GetComponent<PartInventory>().FirstFree();
            if(target.GetComponent<PartInventory>().SpaceFree())
            {
                orbiting = true;
                free = false;
                collider.enabled = false;
                PartManager.Instance.RemoveFreePart(gameObject);
                player.PartOrbiting = true;
                ownedByPlayer = true;
                ps.Stop();
                return true;
            }
            else
            {
                //display full (player)
                return false;
            }
        }
        return false;
    }

    private void SnapToSlotLocation(out int currentSlot, out Vector2 snapVector)
    {
        var toMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition) - playerRef.transform.position;
        toMouse.z = 0;
        var angleToMouseVector = Vector2.SignedAngle(Vector2.up, toMouse.normalized);
        float closestAngle = float.MaxValue;
        float difference = float.MaxValue;
        currentSlot = -1;
        int[] freeSlots = playerInv.GetFreeSlots();
        bool slotFree = false;

        for(int i = 0; i < slotAngles.Length; i++)
        {
            slotFree = false;
            for(int j = 0; j < freeSlots.Length; j++)
            {
                if(i == freeSlots[j])
                {
                    slotFree = true;
                    break;
                }
            }
            //this slot is not free, check next instead
            if(!slotFree)
                continue;

            var dAngle = Mathf.Abs(Mathf.DeltaAngle(slotAngles[i], angleToMouseVector));
            if(dAngle < difference)
            {
                closestAngle = slotAngles[i];
                difference = dAngle;
                currentSlot = i;
            }
        }
        snapVector = new Vector2(Mathf.Cos((closestAngle + 90) * Mathf.PI / 180.0f), Mathf.Sin((closestAngle + 90) * Mathf.PI / 180.0f));
        transform.position = playerRef.transform.position + (Vector3) ((snapVector.normalized * orbitDistance * scaleFactor));
        transform.eulerAngles = new Vector3(0, 0, closestAngle);
    }

    private void AttachToTarget(int currentSlot, Vector2 snapVector)
    {
        transform.position = target.transform.position + (Vector3) ((snapVector.normalized * attachedDistance * scaleFactor));
        if(ownedByPlayer)
        { 
            target.GetComponent<PartInventory>().AddPart(currentSlot);
        }
        else
        {
            target.GetComponent<PartInventory>().AddPart(currentSlot);
        }
        collider.enabled = true;
        attached = true;
        slot = currentSlot;
        orbiting = false;
        player.PartOrbiting = false;
        transform.parent = target.transform;
        //foreach todo
        if(modifier!=null)
            stats.AddStat(modifier);
    }

    private void MouseOver()
    {
        if(!ownedByPlayer)
            return;
        //TODO: highlight sprite
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit;
        if(hit = Physics2D.Raycast(ray.origin, ray.direction))
        {
            if(hit.collider.transform == transform)
            {
                if(attached)
                {
                    sprite.color = new Color(255 / 255.0f, 90 / 255.0f, 90 / 255.0f);
                    if(Input.GetMouseButtonDown(1))
                    {
                        DetachFromTarget();
                    }
                }
            }
        }
        else
        {
            if(attached)
                sprite.color = Color.white;
        }

    }

    public void DetachFromTarget()
    {
        transform.parent = null;
        orbiting = false;
        attached = false;
        free = true;
        collider.enabled = false;
        PartManager.Instance.AddFreePart(gameObject);
        if(ownedByPlayer)
        {
            target.GetComponent<PartInventory>().RemovePart(slot);
        }
        else
        {
            target.GetComponent<PartInventory>().RemovePart(slot);
        }
        if(modifier != null)
            stats.RemoveStat(modifier);
        ps.Play();
        sprite.color = Color.white;
        //player
        var vectorToPart = (transform.position - target.transform.position).normalized * detachDistance * scaleFactor;
        transform.position = target.transform.position + vectorToPart;
    }

    public void ShootFromTarget()
    {
        transform.parent = null;
        orbiting = false;
        attached = false;
        free = false;
        collider.enabled = true;
        //PartManager.Instance.AddFreePart(gameObject);
        if(ownedByPlayer)
        {
            target.GetComponent<PartInventory>().RemovePart(slot);
        }
        else
        {
            target.GetComponent<PartInventory>().RemovePart(slot);
        }
        if(modifier != null)
            stats.RemoveStat(modifier);
        //ps.Play();
        sprite.color = Color.white;
        //player
        //var vectorToPart = (transform.position - target.transform.position).normalized * detachDistance * scaleFactor;
        //transform.position = target.transform.position + vectorToPart;
    }

    public void ShootCleanup()
    {
        //Return to being a free part after colliding with something
        PartManager.Instance.AddFreePart(gameObject);
        ps.Play();
        collider.enabled = false;
        free = true;
    }
}
