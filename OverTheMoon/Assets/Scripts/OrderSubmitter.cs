using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class OrderSubmitter : MonoBehaviour
{
    [SerializeField] private GameObject OrdersManagerObject;
    [SerializeField] private AudioClip SuccessSound;

    private AudioSource Audio;
    private OrderManger OrdersManager;
    
    private void Awake()
    {
        if (!TryGetComponent<AudioSource>(out Audio))
        {
            Audio = gameObject.AddComponent<AudioSource>();
        }


        if (!OrdersManagerObject.TryGetComponent<OrderManger>(out OrdersManager))
        {
            Debug.LogError("Missing the OrdersManagerObject or the OrdersManagerObject is missing the OrderManager script.");
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        var food = other.gameObject.GetComponents<MonoBehaviour>()
                            .OfType<Recipe>()
                            .FirstOrDefault();
        if (food == null)
        {
            return;
        }
        
        SubmitOrder(food);
    }

    private void SubmitOrder(Recipe preparedOrder)
    {
        if (OrdersManager.TryGetOutstandingOrder(preparedOrder, out var outstandingOrder))
        {
            AcceptSubmission(preparedOrder, outstandingOrder);
        }
    }

    private void AcceptSubmission(Recipe preparedOrder, Recipe outstandingOrder)
    {
        PlaySound(SuccessSound);
        OrdersManager.RemoveOrder(outstandingOrder);
        Destroy(preparedOrder.gameObject);
        Destroy(outstandingOrder.gameObject);
    }

    private void PlaySound(AudioClip clip)
    {
        Audio.PlayOneShot(clip);   
    }
}
