using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

// I guess the Hamburger will inherit from this base class
public class Order : MonoBehaviour
{
    [SerializeField] private float SecondsUntilExpiration = 60;
    [SerializeField] private float CompletionValue = 5;
    
    // Final result
    public GameObject Result;
    
    private void Start()
    {
        StartCoroutine(ExpireAfterDelay());
    }

    IEnumerator ExpireAfterDelay()
    {
        yield return new WaitForSeconds(SecondsUntilExpiration);
        Destroy(gameObject);
    }
}

public class OrderQueuer : MonoBehaviour
{
    // Where to spawn the GUIs
    [SerializeField] private GameObject DisplayParent;
    
    // The amount of time before new order is queued
    [SerializeField] private float OrderTimer = 10.0f;
    
    // Max number of orders displayed to the player
    [SerializeField] private int MaxOrders = 5;

    // All the recipes that randomly spawn
    [SerializeField] private List<Order> AvailableOrders;

    private List<Order> OutstandingOrders;
    private Random RandomSelector;
    
    
    // Start is called before the first frame update
    private void Start()
    {
        InvokeRepeating("QueueOrders", 3f, OrderTimer);
    }

    bool ShouldCreateOrder()
    {
        return AvailableOrders.Count < MaxOrders;
    }

    void CreateRandomOrder()
    {
        var randomIndex = RandomSelector.Next(AvailableOrders.Count);
        var selectedOrderPrefab = AvailableOrders.ElementAt(randomIndex);
        
        Instantiate(selectedOrderPrefab, DisplayParent.transform, true);
    }

    void QueueOrders()
    {
        if (ShouldCreateOrder())
        {
            CreateRandomOrder();    
        }
    }
}
