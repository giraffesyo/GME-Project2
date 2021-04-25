using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class OrderSubmitter : MonoBehaviour
{
    [SerializeField] private GameObject OrdersManager;
    [SerializeField] private AudioClip SuccessSound;

    private AudioSource Audio;
    private void Awake()
    {
        if (!gameObject.TryGetComponent<AudioSource>(out Audio))
        {
            Audio = gameObject.AddComponent<AudioSource>();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.GetComponents<MonoBehaviour>().OfType<IRecipe>().Any())
        {
            return;
        }
        
        SubmitOrder(other.gameObject);
    }

    private void SubmitOrder(GameObject preparedOrder)
    {
        var food = preparedOrder.GetComponents<MonoBehaviour>().OfType<IRecipe>().First();
        var submittedOrder = OrdersManager.GetComponent<OrderManger>().CheckAndSubmitFood(food);
        if (submittedOrder.HasValue)
        {
            AcceptSubmission(submittedOrder.Value, preparedOrder);
        }
    }

    private void AcceptSubmission(OrderSubmission orderDetails, GameObject preparedOrder)
    {
        PlaySound(SuccessSound);
        StartCoroutine(DelayedFoodRemoval(preparedOrder));
    }

    private IEnumerator DelayedFoodRemoval(GameObject preparedOrder)
    {
        yield return new WaitForSeconds(1.25f);
        Destroy(preparedOrder);
    }

    private void PlaySound(AudioClip clip)
    {
        Audio.PlayOneShot(clip);   
    }
}
