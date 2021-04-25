using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FoodCompoonentType
{
    FIRST_MEAT,
    SECOND_MEAT,
    TOP_BUN,
    BOTTOM_BUN,
    LETTUCE,
    TOMATO,
    CHEESE
}

public class FridgeScript : MonoBehaviour
{

    [SerializeField]
    private Transform TopBunSpawnPoint;
    [SerializeField]
    private Transform BottomBunSpawnPoint;
    [SerializeField]
    private Transform LettuceSpawnPoint;
    [SerializeField]
    private Transform FirstMeatSpawnPoint;
    [SerializeField]
    private Transform SecondMeatSpawnPoint;
    [SerializeField]
    private Transform TomatoSpawnPoint;
    [SerializeField]
    private Transform CheeseSpawnPoint;

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

    // spawn all the food items at once
    void SpawnAll()
    {
        foreach (FoodCompoonentType food in System.Enum.GetValues(typeof(FoodCompoonentType)))
        {
            Spawn(food);
        }
    }

    private void Spawn(FoodCompoonentType food)
    {
        switch (food)
        {
            case FoodCompoonentType.FIRST_MEAT:
                {
                    GameObject obj = Instantiate(MeatPrefab, FirstMeatSpawnPoint.position, Quaternion.identity);
                    obj.transform.eulerAngles = MeatPrefab.transform.eulerAngles;
                    break;
                }
            case FoodCompoonentType.SECOND_MEAT:
                {
                    GameObject obj = Instantiate(MeatPrefab, SecondMeatSpawnPoint.position, Quaternion.identity);
                    obj.transform.eulerAngles = MeatPrefab.transform.eulerAngles;
                    break;
                }
            case FoodCompoonentType.BOTTOM_BUN:
                {
                    GameObject obj = Instantiate(BottomBunPrefab, BottomBunSpawnPoint.position, Quaternion.identity);
                    obj.transform.eulerAngles = BottomBunPrefab.transform.eulerAngles;
                    break;
                }
            case FoodCompoonentType.TOP_BUN:
                {
                    GameObject obj = Instantiate(TopBunPrefab, TopBunSpawnPoint.position, Quaternion.identity);
                    obj.transform.eulerAngles = TopBunPrefab.transform.eulerAngles;
                    break;
                }
            case FoodCompoonentType.LETTUCE:
                {
                    GameObject obj = Instantiate(LettucePrefab, LettuceSpawnPoint.position, Quaternion.identity);
                    obj.transform.eulerAngles = LettucePrefab.transform.eulerAngles;
                    break;
                }
            case FoodCompoonentType.CHEESE:
                {
                    GameObject obj = Instantiate(CheesePrefab, CheeseSpawnPoint.position, Quaternion.identity);
                    obj.transform.eulerAngles = CheesePrefab.transform.eulerAngles;
                    break;
                }
            case FoodCompoonentType.TOMATO:
                {
                    GameObject obj = Instantiate(TomatoPrefab, TomatoSpawnPoint.position, Quaternion.identity);
                    obj.transform.eulerAngles = TomatoPrefab.transform.eulerAngles;
                    break;
                }
            default:
                {
                    Debug.LogError("Fell to default case in Spawn food component of fridge script");
                    break;
                }
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        SpawnAll();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
