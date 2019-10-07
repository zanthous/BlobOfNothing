using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Part : MonoBehaviour
{
    //distances have to be adjusted by height of part sprite
    private float detachForce = 10.0f;
    private float detachDistance = 1.5f;
    private float attachedDistance = .7f;
    private float startOrbitDistance = 1.0f;
    private float orbitDistance = 1.0f;
   
    private float scaleFactor = 1.0f;

    private Player player;
    private GameObject playerRef;
    //player or enemy depending on which is picking up the part
    private GameObject target;
    private PartInventory playerInv;
    //private PartInventory targetInv;
    private ParticleSystem ps;
    private SpriteRenderer sprite;
    private BoxCollider2D collider;

    private bool orbiting = false;
    private bool attached = false;
    private bool ownedByPlayer = false;

    [SerializeField]
    private bool free = true;
    public bool Free { get => free; set => free = value; }

    //Serialized so enemies can have pre-setup parts
    [SerializeField]
    private int slot = -1;
    public int Slot { get => slot; /*set => slot = value; */}
    public bool OwnedByPlayer { get => ownedByPlayer; set => ownedByPlayer = value; }
    public bool Attached { get => attached; set => attached = value; }

    //Ordered by slot index
    private float[] slotAngles = { 0,45,90,135,180,-135,-90,-45 };

    
    // Start is called before the first frame update
    void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
        playerInv = playerRef.GetComponent<PartInventory>();
        ps = GetComponent<ParticleSystem>();
        sprite = GetComponent<SpriteRenderer>();
        player = playerRef.GetComponent<Player>();
        collider = GetComponent<BoxCollider2D>();
        if(!free)
        { 
            ps.Stop();
            attached = true;
        }
        else
        {
            collider.enabled = false;
        }
        if(transform.parent != null)
            target = transform.parent.gameObject;

        scaleFactor = (sprite.sprite.rect.height / 16.0f) * .25f + .75f;
    }

    // Update is called once per frame
    void Update()
    {
        //in other scripts check to see if attached before applying bonuses
        if(attached)
        {
            MouseOver();
            return;
        }

        if(!orbiting && !attached && free)
        {
            //TODO: Check enemies, snap to random part
            //if()
            /*else*/
            for(int i = 0; i < EnemyManager.Instance.enemies.Count; i++)
            {
                if(Vector2.Distance(transform.position, EnemyManager.Instance.enemies[i].transform.position) < startOrbitDistance)
                {
                    target = EnemyManager.Instance.enemies[i];
                    var tempSlot = target.GetComponent<PartInventory>().FirstFree();
                    if(tempSlot!=-1)
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
                        return;
                    }
                    else
                    {
                        continue;
                    }
                }
            }


            if(player.PartOrbiting)
            {
                return;
            }

            //player
            if(Vector2.Distance(playerRef.transform.position, transform.position) < startOrbitDistance)
            {
                target = playerRef;
                var tempSlot = target.GetComponent<PartInventory>().FirstFree();
                if(target.GetComponent<PartInventory>().SpaceFree())
                {
                    orbiting = true;
                    free = false;
                    collider.enabled = true;
                    PartManager.Instance.RemoveFreePart(gameObject);
                    player.PartOrbiting = true;
                    ownedByPlayer = true;
                    ps.Stop();
                }
                else
                {
                    //display full (player)
                }
            }
        }
        else if(orbiting)
        {
            var toMouse =  Camera.main.ScreenToWorldPoint(Input.mousePosition) - playerRef.transform.position;
            toMouse.z = 0;
            var angleToMouseVector = Vector2.SignedAngle(Vector2.up, toMouse.normalized);
            float closestAngle = float.MaxValue;
            float difference = float.MaxValue;
            int currentSlot = -1;
            int[] freeSlots = playerInv.GetFreeSlots();
            bool slotFree = false;

            for(int i = 0; i < slotAngles.Length; i++)
            {
                slotFree = false;
                for(int j = 0; j < freeSlots.Length; j++)
                {
                    if(i==freeSlots[j])
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
            Vector2 snapVector;
            snapVector = new Vector2(Mathf.Cos((closestAngle + 90)*Mathf.PI / 180.0f), Mathf.Sin((closestAngle + 90) * Mathf.PI / 180.0f));
            transform.position = playerRef.transform.position + (Vector3)((snapVector.normalized * orbitDistance * scaleFactor));
            transform.eulerAngles = new Vector3(0, 0, closestAngle);

            if(Input.GetMouseButtonDown(0))
            {
                AttachToTarget(currentSlot, snapVector);
            }
        }
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
        attached = true;
        slot = currentSlot;
        orbiting = false;
        player.PartOrbiting = false;
        transform.parent = target.transform;
        GetComponent<ModifierBase>()?.UpdateEntityParameter();
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
        GetComponent<ModifierBase>()?.UpdateEntityParameter();
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
        GetComponent<ModifierBase>()?.UpdateEntityParameter();
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
    //private void OnMouseExit()
    //{
    //    if(attached)
    //        sprite.color = Color.white;
    //}
}
