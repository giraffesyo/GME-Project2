using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurgerComponent : MonoBehaviour
{
    public bool isTop = false;
    public Transform Burger;
    public bool isManipulated = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setManipulated()
    {
        isManipulated = !isManipulated;
        Debug.Log("Change Manipulation State " + isManipulated);
    }
}
