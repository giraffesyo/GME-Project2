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

public abstract class Recipe : MonoBehaviour
{
    protected abstract IEnumerable<GameObject> GetIngredients();

    public bool Matches(Recipe otherRecipe)
    {
        return GetIngredients()
            .Select(x => x.name)
            .SequenceEqual(
                otherRecipe.GetIngredients()
                .Select(x => x.name)
            );
    }
}

public class Order : Recipe
{
    [SerializeField] public List<GameObject> OrderedRecipeIngredients;
    [SerializeField] public float SecondsUntilExpiration;
    [SerializeField] public int CompletionValue;
    [SerializeField] private TextMeshProUGUI DisplayText;
    [SerializeField] private int Bonus;

    private int BonusTimeOffset = 10; 
    private Timer ExpirationTimer;
    private int CurrentValue;

    private string Ingredients => string.Join("\n", OrderedRecipeIngredients.Select((x, i) => $"{i + 1}. {x.name}"));
    private string CookedMealName => gameObject.name;

    private void Awake()
    {
        ExpirationTimer =  gameObject.AddComponent<Timer>();
        ExpirationTimer.TimeRemaining = SecondsUntilExpiration;
    }

    private void Update()
    {
        UpdateOrder();
    }

    private int CalculateCurrentValue()
    {
        var bonus = Mathf.Clamp((int) Math.Ceiling(Bonus * (ExpirationTimer.TimeRemaining - BonusTimeOffset) / (SecondsUntilExpiration - BonusTimeOffset)), 0, Bonus);

        return CompletionValue + bonus;
    }

    private void UpdateOrder()
    {
        CurrentValue = CalculateCurrentValue();
        var timeDisplay = ExpirationTimer.GetTimeDisplay();
        DisplayText.text = $"<size=2.5em><uppercase>{CookedMealName}</uppercase>  <color=#00FF00>${CurrentValue}</color> - <color=#FF0000>{timeDisplay}</color>\n<size=1.75em>{Ingredients}";

        if (ExpirationTimer.HasExpired())
        {
            GetComponentInParent<OrderManger>().RemoveOrder(this);
            Destroy(gameObject);
        }
    }

    public OrderSubmission GetSubmissionInfo()
    {
        return new OrderSubmission()
        {
            Meal = CookedMealName,
            Value = CurrentValue
        };
    }

    private void OnDestroy()
    {
        Debug.Log($"{CookedMealName} has been destroyed");
    }

    protected override IEnumerable<GameObject> GetIngredients()
    {
        return new List<GameObject>(OrderedRecipeIngredients);
    }
}