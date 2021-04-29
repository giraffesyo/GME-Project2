using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum FridgeItemType
{
    MEAT,
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
    private Transform MeatSpawnPoint;
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

    [SerializeField] private float RespawnTime = 5f;
    private List<Transform> SpawnPoints => new List<Transform>()
    {
        TopBunSpawnPoint,  BottomBunSpawnPoint, LettuceSpawnPoint, MeatSpawnPoint, TomatoSpawnPoint, CheeseSpawnPoint
    };

    private List<GameObject> PrefabsToSpawn => new List<GameObject>()
    {
        TopBunPrefab, BottomBunPrefab, LettucePrefab, MeatPrefab, TomatoPrefab, CheesePrefab
    };

    public List<GameObject> SpawnedFood = new List<GameObject>();
    
    // spawn all the food items at once
    private void SpawnAll()
    {
        foreach (var foodToSpawn in PrefabsToSpawn)
        {
            Spawn(foodToSpawn);
        }
    }

    private void Spawn(GameObject food)
    {
        var spawnPoint = SpawnPoints.FirstOrDefault(x => x.name == food.name);
        if (spawnPoint == null) return;
        
        GameObject spawnedFood = Instantiate(food, spawnPoint);
        spawnedFood.transform.parent = null;
        spawnedFood.name = food.name;
        spawnedFood.transform.eulerAngles = food.transform.eulerAngles;
        SpawnedFood.Add(spawnedFood);
    }

    private void RespawnAsNeeded()
    {
        var foodsToSpawn = new List<GameObject>();
        foreach (var food in SpawnedFood)
        {
            if (food.TryGetComponent<BurgerComponent>(out var burgerComponent) && burgerComponent.HasBeenManipulated)
            {
                foodsToSpawn.Add(food);
                continue;
            }
            
            // for the burger, since it doesnt have a burgercomponent script on the object
            burgerComponent = food.GetComponentInChildren<BurgerComponent>();
            if (burgerComponent != null && burgerComponent.isManipulated)
            {
                foodsToSpawn.Add(food);
            }
        }

        var foodPrefabs = foodsToSpawn.Select(x => PrefabsToSpawn.FirstOrDefault(y => x.name == y.name))
            .Where(x => x != null);
        SpawnedFood = SpawnedFood.Where(x => !foodsToSpawn.Contains(x)).ToList();
        foreach (var foodToSpawn in foodPrefabs)
        {
            Spawn(foodToSpawn);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        SpawnAll();
        InvokeRepeating(nameof(RespawnAsNeeded), RespawnTime, RespawnTime);
    }
}
