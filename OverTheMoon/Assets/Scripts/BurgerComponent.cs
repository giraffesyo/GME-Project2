using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum FoodType
{
    MEAT,
    CHEESE,
    TOMATO,
    LETTUCE,
    TOP_BUN,
    BOTTOM_BUN,
}



public class BurgerComponent : MonoBehaviour
{
    public bool HasBeenManipulated = false;
    public bool isManipulated = false;
    // Start is called before the first frame update
    public FoodType foodType;
    
    [SerializeField]
    private GameObject MeatPrefab;
    [SerializeField]
    private GameObject LettucePrefab;
    [SerializeField]
    private GameObject TopBunPrefab;
    [SerializeField]
    private GameObject BottomBunPrefab;
    [SerializeField]
    private GameObject TomatoPrefab;
    [SerializeField]
    private GameObject CheesePrefab;

    public void ResetToOriginalEulerAngles()
    {
        // Quaternion newRotation = transform.euler;
        Vector3 newRotation = transform.eulerAngles;
        switch (foodType)
        {
            case FoodType.MEAT:
                {

                    newRotation = MeatPrefab.transform.eulerAngles;
                    break;
                }

            case FoodType.BOTTOM_BUN:
                {

                    newRotation = BottomBunPrefab.transform.eulerAngles;
                    break;
                }
            case FoodType.TOP_BUN:
                {

                    newRotation = TopBunPrefab.transform.eulerAngles;
                    break;
                }
            case FoodType.LETTUCE:
                {

                    newRotation = LettucePrefab.transform.eulerAngles;
                    break;
                }
            case FoodType.CHEESE:
                {

                    newRotation = CheesePrefab.transform.eulerAngles;
                    break;
                }
            case FoodType.TOMATO:
                {

                    newRotation = TomatoPrefab.transform.eulerAngles;
                    break;
                }
            default:
                {
                    Debug.LogError("Fell to default case in Spawn food component of fridge script");
                    break;
                }
        }
        Debug.Log($"Updating rotation of  {gameObject.name} to {newRotation}");
        transform.eulerAngles = newRotation;
    }


    public void setManipulated()
    {
        isManipulated = !isManipulated;
        HasBeenManipulated = true;
        // Debug.Log("Change Manipulation State " + isManipulated);
    }
}
