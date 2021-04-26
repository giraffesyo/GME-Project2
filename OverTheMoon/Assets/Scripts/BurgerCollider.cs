using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;

public class BurgerCollider : MonoBehaviour
{
    private float componentOffset = .05f;
    // private void OnTriggerEnter(Collider other)
    // {
    //     Debug.Log("Colliding " + !other.gameObject.GetComponent<BurgerComponent>().isManipulated);
    //     GameObject BurgerComponentObject = gameObject.transform.parent.gameObject;
    //     Vector3 originalPosition = BurgerComponentObject.transform.position;
    //     if (other.gameObject.tag == "BurgerComponent"
    //         && (!other.gameObject.GetComponent<BurgerComponent>().isManipulated)
    //         && BurgerComponentObject.GetComponent<BurgerComponent>().Burger != null)
    //     {
    //         Debug.Log("Burger Collision");


    //         if (BurgerComponentObject.GetComponent<BurgerComponent>().isTop)
    //         {
    //             other.gameObject.GetComponent<BurgerComponent>().Burger = BurgerComponentObject.GetComponent<BurgerComponent>().Burger;
    //             BurgerComponentObject.GetComponent<BurgerComponent>().Burger.GetComponent<Burger>().Top = other.gameObject;
    //             BurgerComponentObject.GetComponent<BurgerComponent>().Burger.GetComponent<Burger>().BurgerComponents.Add(other.gameObject);

    //             BurgerComponentObject.GetComponent<BurgerComponent>().isTop = false;
    //             other.gameObject.GetComponent<BurgerComponent>().isTop = true;
    //             //other.gameObject.transform.parent = BurgerComponentObject.GetComponent<BurgerComponent>().Burger;
    //             other.gameObject.transform.parent = BurgerComponentObject.transform;
    //             other.gameObject.transform.localPosition = new Vector3(0, .01f, 0);
    //             // set rotation of current component to its original position
    //             other.gameObject.GetComponent<BurgerComponent>().ResetToOriginalEulerAngles();

    //             Destroy(other.gameObject.GetComponent<Rigidbody>());
    //             BurgerComponentObject.transform.position = originalPosition;
    //         }
    //     }
    // }
    private void OnTriggerEnter(Collider other)
    {
        // the collider we want to interact with is two levels down from the "burger component" 
        BurgerComponent thisBurgerComponent = GetComponentInParent<BurgerComponent>();
        // the collider that we want to detect hitting is the burger component, that is, the 3D object, not the burger collider box colliders
        BurgerComponent otherBurgerComponent = other.GetComponentInParent<BurgerComponent>();
        // This verifies that we're dealing with burger components only
        if (!otherBurgerComponent || !thisBurgerComponent)
        {
            return;
        }
        // check if this component is part of a burger
        Burger burger = GetComponentInParent<Burger>();
        if (!burger)
        {
            // we don't want to snap two components together unless "this" component is part of a burger
            return;
        }
        // Debug.Log($"detected the collision between the components this {thisBurgerComponent.gameObject.name} and other {otherBurgerComponent.gameObject.name}");
        // we only want to snap things to the top of the burger, so ignore other collisions
        if (burger.Top != thisBurgerComponent)
        {
            return;
        }

        // only snap things together when we're not manipulating the other object (the free object)
        if (!otherBurgerComponent.isManipulated)
        {

            Rigidbody otherBurgerRb = otherBurgerComponent.GetComponent<Rigidbody>();
            // delete the components rigidbody
            Destroy(otherBurgerRb);
            // Destroy object manipulators  on the burger components
            Destroy(otherBurgerComponent.GetComponent<ObjectManipulator>());
            burger.name = "Burger (Incomplete)";
            // set the collided burger component to also be a child of the burger
            otherBurgerComponent.transform.SetParent(burger.transform, true);
            otherBurgerComponent.transform.SetAsLastSibling();
            // set the collided burger components position to some offset
            Vector3 newPosition = thisBurgerComponent.transform.localPosition;
            newPosition.y += componentOffset * burger.BurgerComponents.Count;
            otherBurgerComponent.transform.localPosition = newPosition;
            // reset the rotations on the collided component
            // otherBurgerComponent.transform.rotation = Quaternion.identity;
            otherBurgerComponent.ResetToOriginalEulerAngles();
            burger.Top = otherBurgerComponent;


            burger.BurgerComponents.Add(otherBurgerComponent.gameObject);
        }



    }
}
