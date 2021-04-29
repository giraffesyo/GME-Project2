using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cookable : MonoBehaviour
{
    [SerializeField] private float TimeToCook = 5f;
    [SerializeField] private float OvercookedThreshold = 3f;
    private bool IsCooking = false;
    private float TimeCooked = 0f;

    public bool IsOvercooked => TimeCooked > TimeToCook + OvercookedThreshold;
    public bool IsCooked => TimeCooked > TimeToCook  && !IsOvercooked;
    // Update is called once per frame
    void FixedUpdate()
    {
        if (IsCooking)
        {
            TimeCooked += Time.deltaTime;
        }

        if (IsCooked)
        {
            // idk do something
            Debug.Log("The meat is cooked!");
        } else if (IsOvercooked)
        {
            // do something else??
            Debug.Log("Ah, you burned the meat.");
        }
        
        UpdateTimeDisplay();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.TryGetComponent<Cooker>(out var cooker))
        {
            return;
        }
        IsCooking = true;    
    }

    private void OnCollisionExit(Collision other)
    {
        if (!other.gameObject.TryGetComponent<Cooker>(out var cooker))
        {
            return;
        }

        IsCooking = false;
    }

    private void UpdateTimeDisplay()
    {
        
    }
}
