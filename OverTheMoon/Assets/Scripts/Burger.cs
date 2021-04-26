using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Burger : MonoBehaviour, IRecipe
{

    public List<GameObject> BurgerComponents;
    public BurgerComponent Top;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnCollisionEnter(Collision collision)
    {
        //if(collision.)
    }

    public IEnumerable<GameObject> GetIngredients()
    {
        return new List<GameObject>(BurgerComponents);
    }
}
