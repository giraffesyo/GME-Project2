using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.MixedReality.Toolkit.Editor;
using TMPro;
using UnityEngine;

public struct OrderSubmission
{
    public string Meal;
    public int Value;
}

public interface IRecipe
{
    public IEnumerable<GameObject> GetIngredients();
}

public class Order : MonoBehaviour, IRecipe
{
    [SerializeField] public List<GameObject> OrderedRecipeIngredients;
    [SerializeField] public float SecondsUntilExpiration;
    [SerializeField] public int CompletionValue;
    [SerializeField] private TextMeshProUGUI DisplayText;

    private Timer ExpirationTimer;
    private int CurrentValue;
    private string Ingredients => "*" + string.Join("\n*", OrderedRecipeIngredients);
    private string Meal => gameObject.name;

    private void Awake()
    {
        ExpirationTimer =  gameObject.AddComponent<Timer>();
        ExpirationTimer.TimeRemaining = SecondsUntilExpiration;
    }

    private void Update()
    {
        UpdateOrder();
    }

    private void UpdateOrder()
    {
        CurrentValue = (int) Math.Ceiling(CompletionValue * ExpirationTimer.TimeRemaining / SecondsUntilExpiration);
        var timeDisplay = ExpirationTimer.GetTimeDisplay();
        DisplayText.text = $"<size=2.5em>{Meal} - {timeDisplay}\n <indent=15%><size=1.5em>{Ingredients}</indent>";

        if (ExpirationTimer.HasExpired())
        {
            GetComponentInParent<OrderManger>().ExpireOrder(this);
        }
    }

    public OrderSubmission GetSubmissionInfo()
    {
        return new OrderSubmission()
        {
            Meal = Meal,
            Value = CurrentValue
        };
    }

    private void OnDestroy()
    {
        Debug.Log($"{Meal} has been destroyed");
    }

    public IEnumerable<GameObject> GetIngredients()
    {
        return new List<GameObject>(OrderedRecipeIngredients);
    }
}