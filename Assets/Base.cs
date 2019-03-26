using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Base : MonoBehaviour
{
    public Color baseColour;
    public float tiberium = 0;
    public float tiberiumCooldown =1f;
    public TextMeshPro text;
    

    public GameObject fighterPrefab;


    // Start is called before the first frame update
    void Start()
    {
        Color randomC = Color.HSVToRGB(Random.Range(0.0f, 1.0f), 1, 1);
        baseColour = randomC;

        foreach (Renderer r in GetComponentsInChildren<Renderer>())
        {
            r.material.color = baseColour;
        }
    }

    private void OnEnable()
    {
        StartCoroutine(AddTiberium(tiberiumCooldown));
    }

    // Update is called once per frame
    void Update()
    {
        text.text = "" + tiberium;

        if(tiberium >= 10)
        {
            //spawn fighter
            SpawnFighter();
            //spend tibernium
            RemoveTibernium(10f);
        }
    }

    IEnumerator AddTiberium(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            tiberium += 1;
        }
    }

    public void RemoveTibernium(float amount)
    {
        tiberium -= amount;
    }

    void SpawnFighter()
    {
        //instantiate
        GameObject fighterGO = (GameObject)Instantiate(fighterPrefab,
            transform.position, transform.rotation);
        //colour fighter
        //assign base to fighter
        fighterGO.GetComponent<Fighter>().myBase = this;
        //assign colour to fighter
        foreach (Renderer r in fighterGO.GetComponentsInChildren<Renderer>())
        {
            r.material.color = baseColour;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "bullet")
        {
            RemoveTibernium(0.5f);
            Destroy(other.gameObject);
        }
    }
}
