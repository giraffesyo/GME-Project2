using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = System.Random;

public interface IOrderQueueHandler : IEventSystemHandler
{
    void SubmitOrder(string meal);
    void ExpireOrder(string meal);
}

public class OrderManger : MonoBehaviour, IOrderQueueHandler
{
    // Where to spawn the tickets
    // [SerializeField] private GameObject OrderBoard;
    
    [SerializeField] private int OrderCapacity = 10;
    
    // The amount of time before new order is queued
    [SerializeField] private float TimeBetweenOrders = 10.0f;
    
    // All the recipes that randomly spawn
    [SerializeField] private List<Order> AvailableOrders;
    
    
    private List<Order> OutstandingOrders = new List<Order>();
    private Random RandomSelector = new Random();
    
    
    // Start is called before the first frame update
    private void Start()
    {
        InvokeRepeating("QueueOrders", 3f, TimeBetweenOrders);
    }

    bool ShouldCreateOrder()
    {
        return OutstandingOrders.Count < OrderCapacity;
    }

    void CreateRandomOrder()
    {
        var randomIndex = RandomSelector.Next(AvailableOrders.Count);
        var order = AvailableOrders.ElementAt(randomIndex);
        
        CreateOrder(order);
    }

    private void CreateOrder(Order order)
    {
        var newOrder = Instantiate(order, gameObject.transform, false);

        OutstandingOrders.Add(newOrder);
    }

    private Order FindOrderIndex(string meal)
    {
        return OutstandingOrders.FirstOrDefault(order => order.Meal == meal);
    }
    
    public void SubmitOrder(string meal)
    {
        var order = FindOrderIndex(meal);
        if (order == null)
        {
            return;
        }
        
        OutstandingOrders.Remove(order);
        Destroy(order.gameObject);
    }

    public void ExpireOrder(string meal)
    {
        Debug.Log($"Expiring {meal}");
        var order = FindOrderIndex(meal);
        if (order == null)
        {
            return;
        }
        
        OutstandingOrders.Remove(order);
        Destroy(order.gameObject);
    }

    void QueueOrders()
    {
        if (ShouldCreateOrder())
        {
            CreateRandomOrder();    
        }
    }

    public List<Order> GetOutstandingOrders()
    {
        return OutstandingOrders;
    }
}
