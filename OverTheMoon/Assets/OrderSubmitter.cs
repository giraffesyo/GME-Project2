using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OrderSubmitter : MonoBehaviour
{
    [SerializeField] private GameObject OrderQueue; 
    private void OnCollisionEnter(Collision other)
    {
        ExecuteEvents.Execute<IOrderQueueHandler>(OrderQueue, null, ((handler, data) => handler.SubmitOrder(other.collider.name)));
    }
}
