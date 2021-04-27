using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = System.Random;

public interface IOrderHandler : IEventSystemHandler
{
    List<Order> GetAllOutstandingOrders();

    bool TryGetOutstandingOrder(Recipe preparedOrder, out Order outstandingOrder);
    
    bool RemoveOrder(Recipe orderToRemove);
}

public class OrderManger : MonoBehaviour, IOrderHandler
{
    // Where to spawn the tickets
    [SerializeField] private GameObject ContentDisplay;
    
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

    private bool ShouldCreateOrder()
    {
        return OutstandingOrders.Count < OrderCapacity;
    }

    private Order SelectRandomAvailableOrder()
    {
        var randomIndex = RandomSelector.Next(AvailableOrders.Count);
        var order = AvailableOrders.ElementAt(randomIndex);

        return order;
    }
    
    private void CreateOrder(Order order)
    {
        var newOrder = Instantiate(order, ContentDisplay.transform, false);
        newOrder.name = order.name;
        OutstandingOrders.Add(newOrder);
    }
    
    private Order FindOrder(Recipe recipe)
    {
        return OutstandingOrders.FirstOrDefault(order => order.Matches(recipe));
    }

    private void QueueOrders()
    {
        if (!ShouldCreateOrder()) return;
        
        var nextOrder = SelectRandomAvailableOrder();
        CreateOrder(nextOrder);
    }

    public List<Order> GetAllOutstandingOrders()
    {
        return new List<Order>(OutstandingOrders);
    }

    public bool TryGetOutstandingOrder(Recipe preparedOrder, out Order outstandingOrder)
    {
        outstandingOrder = FindOrder(preparedOrder);

        return outstandingOrder != null;
    }
    
    public bool RemoveOrder(Recipe orderToRemove)
    {
        var order = FindOrder(orderToRemove);
        return OutstandingOrders.Remove(order);
    }
}
