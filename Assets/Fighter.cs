using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Seek))]
public class Fighter : MonoBehaviour
{
    [SerializeField] private int ammo = 7;
    [SerializeField] private float shotCD = 0.5f;
    private float shotCDRemaining;
    public GameObject bulletPrefab;
    public Base myBase;
    public List<Base> bases;
    public Transform targetTransform;
    Transform arriveTransform;
    Seek seek;
    Arrive arrive;
    Boid boid;
    bool chosen = false;
   public bool seekBool=true;
    public bool arriveBool=false;
    public bool restockBool =false;
    // Start is called before the first frame update
    void Start()
    {
        shotCDRemaining = shotCD;
        boid = GetComponent<Boid>();
        seek = GetComponent<Seek>();
        arrive = GetComponent<Arrive>();
        //get list of all other bases
        //if this base is there, remove it
        Base[]baseComponents = GameObject.FindObjectsOfType<Base>();
        for(int i= 0; i < baseComponents.Length; i++)
        {
            bases.Add(baseComponents[i]);
        }

        foreach(Base b in bases)
        {
            if(b == myBase)
            {
                bases.Remove(myBase);
                return;
            }
        }

        
      
    }

   
    // Update is called once per frame
    void Update()
    {
        if (!chosen)
        {
            ChooseTargetBase();
        }
        //targetTransform = bases[0].gameObject.transform;
        if (targetTransform != null)
        {
            //seek state
            if (seekBool)
            {
                seek.target = targetTransform.position;
                //if close enough to target
                if(Vector3.Distance(targetTransform.position,transform.position)
                    <= 15f){
                    arriveBool = true;
                    seekBool = false;
                    restockBool = false;
                    arriveTransform = transform;
                }
            }
            //arrive state
            if (arriveBool)
            {
                
                boid.force = Vector3.zero;
                boid.velocity = Vector3.zero;
                seek.target = myBase.gameObject.transform.position;
                seek.enabled = false;
                
                if(ammo > 0)
                {
                    shotCDRemaining -= Time.deltaTime;
                    if (shotCDRemaining <= 0)
                    {
                        shotCDRemaining = shotCD;
                        Shoot(targetTransform.position);
                    }
                    
                }
                else
                {
                    arriveBool = false;
                    seekBool = false;
                    restockBool = true;
                }
            }

            if (restockBool)
            {
                //seek to base
                seek.enabled = true;
                boid.SeekForce(myBase.gameObject.transform.position);
                //if close to base
                if(Vector3.Distance(myBase.gameObject.transform.position,
                    transform.position) <= 3f)
                {
                    if (myBase.tiberium >= 7)
                    {
                        //refill ammo
                        ammo = 7;
                        myBase.RemoveTibernium(7);
                        restockBool = false;
                        arriveBool = false;
                        seekBool = true;
                    }
                    else
                    {
                        boid.force = Vector3.zero;
                        boid.velocity = Vector3.zero;
                    }
                }
            }

        }
        
    }

    void ChooseTargetBase()
    {
        //select random base to be target
        targetTransform = bases[Random.Range(0,bases.Count-1)].gameObject.transform;
        chosen = true;
    }

    public void Shoot(Vector3 target)
    {
        transform.LookAt(target);
        //instantiate bullet
        GameObject bulletGO = (GameObject)Instantiate(bulletPrefab, transform.position,
            transform.rotation);
        //bullet colour
        foreach (Renderer r in bulletGO.GetComponentsInChildren<Renderer>())
        {
            r.material.color = GetComponent<Renderer>().material.color;
        }
        
        ammo -= 1;
        //take 1 away from ammo
        
    }
}
