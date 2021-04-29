using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class OrderSubmitter : MonoBehaviour
{
    [SerializeField] private GameObject OrdersManagerObject;
    [SerializeField] private AudioClip SuccessSound;
    [SerializeField] private TextMeshPro PointsDisplay;

    private AudioSource Audio;
    private OrderManger OrdersManager;
    
    private void Awake()
    {
        PointsDisplay.gameObject.SetActive(false);
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
        StartCoroutine(ShowPoints((Order)outstandingOrder));
        Destroy(preparedOrder.gameObject);
        Destroy(outstandingOrder.gameObject);
    }

    private IEnumerator ShowPoints(Order submittedOrder)
    {
        var submission = submittedOrder.GetSubmissionInfo();
        PointsDisplay.gameObject.SetActive(true);
        PointsDisplay.text = $"+ {submission.Value}";
        PointsDisplay.gameObject.transform.LookAt(Camera.main.transform);
        // PointsDisplay.gameObject.transform.localEulerAngles = new Vector3(0, 180, 0);
        yield return new WaitForSeconds(5f);
        PointsDisplay.gameObject.SetActive(false);
    }

    private void PlaySound(AudioClip clip)
    {
        Audio.PlayOneShot(clip);   
    }
}
