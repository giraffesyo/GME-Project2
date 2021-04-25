using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class OrderSubmitter : MonoBehaviour
{
    [SerializeField] private GameObject OrdersManager; 
    private void OnCollisionEnter(Collision other)
    {
        var preparedOrder = other.gameObject.GetComponents<MonoBehaviour>().OfType<IRecipe>().FirstOrDefault();
        if (preparedOrder == null) return;
        
        var submittedOrder = OrdersManager.GetComponent<OrderManger>().SubmitOrder(preparedOrder);
        Debug.Log($"Submitting prepared order {other.collider.name}");
        if (submittedOrder.HasValue)
        {
            AcceptSubmission(submittedOrder.Value, other.gameObject);
        }
    }

    private void AcceptSubmission(OrderSubmission orderDetails, GameObject preparedOrder)
    {
        Debug.Log($"Accepted {orderDetails.Meal}");
        Destroy(preparedOrder);
    }
}
