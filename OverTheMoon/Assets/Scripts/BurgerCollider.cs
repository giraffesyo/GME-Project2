using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurgerCollider : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Colliding " + !other.gameObject.GetComponent<BurgerComponent>().isManipulated);
        GameObject BurgerComponentObject = gameObject.transform.parent.gameObject;

        if (other.gameObject.tag == "BurgerComponent"
            && (!other.gameObject.GetComponent<BurgerComponent>().isManipulated)
            && BurgerComponentObject.GetComponent<BurgerComponent>().Burger != null)
        {
            Debug.Log("Burger Collision");


            if (BurgerComponentObject.GetComponent<BurgerComponent>().isTop)
            {
                other.gameObject.GetComponent<BurgerComponent>().Burger = BurgerComponentObject.GetComponent<BurgerComponent>().Burger;
                BurgerComponentObject.GetComponent<BurgerComponent>().Burger.GetComponent<Burger>().Top = other.gameObject;
                BurgerComponentObject.GetComponent<BurgerComponent>().Burger.GetComponent<Burger>().BurgerComponents.Add(other.gameObject);

                BurgerComponentObject.GetComponent<BurgerComponent>().isTop = false;
                other.gameObject.GetComponent<BurgerComponent>().isTop = true;
                //other.gameObject.transform.parent = BurgerComponentObject.GetComponent<BurgerComponent>().Burger;
                other.gameObject.transform.parent = BurgerComponentObject.transform;
                other.gameObject.transform.localPosition = new Vector3(0, 1, 0);
                Destroy(other.gameObject.GetComponent<Rigidbody>());

            }
        }
    }
}
