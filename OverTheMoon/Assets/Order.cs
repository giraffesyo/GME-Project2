using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.MixedReality.Toolkit.Editor;
using TMPro;
using UnityEngine;

public class Order : MonoBehaviour
{
    [SerializeField] private string Meal;
    [SerializeField] public List<string> OrderedIngredients;
    [SerializeField] public float TimeUntilExpiration;
    [SerializeField] public int CompletionValue;
    [SerializeField] private TextMeshProUGUI DisplayText;

    private Timer ExpirationTimer;

    private string Ingredients => "*" + string.Join("\n*", OrderedIngredients);

    private void Awake()
    {
        ExpirationTimer =  gameObject.AddComponent<Timer>();
        ExpirationTimer.TimeRemaining = TimeUntilExpiration;
    }

    private void Update()
    {
        UpdateOrder();
    }

    private void UpdateOrder()
    {
        var timeDisplay = ExpirationTimer.GetTimeDisplay();
        DisplayText.text = $"<size=2.5em>{Meal} - {timeDisplay}\n <indent=15%><size=1.5em>{Ingredients}</indent>";

        if (ExpirationTimer.HasExpired())
        {
            GetComponentInParent<OrderManger>().ExpireOrder(Meal);
        }
    }

    private void OnDestroy()
    {
        Debug.Log($"{Meal} has expired");
        Destroy(gameObject);
    }
}