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
    OrderSubmission? CheckAndSubmitFood(IRecipe preparedFood);
    void ExpireOrder(IRecipe expiredOrder);
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

    private bool ShouldCreateOrder()
    {
        return OutstandingOrders.Count < OrderCapacity;
    }

    private void CreateRandomOrder()
    {
        var randomIndex = RandomSelector.Next(AvailableOrders.Count);
        var order = AvailableOrders.ElementAt(randomIndex);
        
        CreateOrder(order);
    }

    private void CreateOrder(Order order)
    {
        var newOrder = Instantiate(order, gameObject.transform, false);
        newOrder.name = order.name;
        OutstandingOrders.Add(newOrder);
    }
    
    private Order FindOrder(IRecipe recipe)
    {
        return OutstandingOrders.FirstOrDefault(order => DoRecipesMatch(order, recipe));
    }

    private bool DoRecipesMatch(IRecipe first, IRecipe second)
    {
        return first.GetIngredients().SequenceEqual(second.GetIngredients());
    }
    
    public OrderSubmission? CheckAndSubmitFood(IRecipe preparedFood)
    {
        var order = FindOrder(preparedFood);
        if (order == null)
        {
            return null;
        }
        
        OutstandingOrders.Remove(order);
        Destroy(order.gameObject);
        return order.GetSubmissionInfo();
    }

    public void ExpireOrder(IRecipe expiredOrder)
    {
        var order = FindOrder(expiredOrder);
        if (order == null)
        {
            return;
        }
        
        OutstandingOrders.Remove(order);
        Destroy(order.gameObject);
    }

    private void QueueOrders()
    {
        if (ShouldCreateOrder())
        {
            CreateRandomOrder();    
        }
    }
}
