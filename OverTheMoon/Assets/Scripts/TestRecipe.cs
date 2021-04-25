using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRecipe : MonoBehaviour, IRecipe
{
    [SerializeField] private List<GameObject> Ingredients;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerable<GameObject> GetIngredients()
    {
        return new List<GameObject>(Ingredients);
    }
}
