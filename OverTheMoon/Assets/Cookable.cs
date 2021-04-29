using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Cookable : MonoBehaviour
{
    [SerializeField] private float CookingTime = 10f;
    [SerializeField] private float OvercookedThreshold = 5f;
    [SerializeField] private GameObject CircularProgressBarPrefab;
    [SerializeField] private Material RawMaterial;
    [SerializeField] private Material CookedMaterial;
    [SerializeField] private Material BurntMaterial;
    
    private bool IsCooking = false;
    private float TimeCooked = 0f;
    private string InitialName = "";
    private GameObject CookingProgressBar;
    
    public bool IsOvercooked => TimeCooked > CookingTime + OvercookedThreshold;
    public bool IsCooked => TimeCooked > CookingTime  && !IsOvercooked;
    
    private void Start()
    {
        InitialName = gameObject.name;
        gameObject.name = $"Raw{InitialName}";
        UpdateMaterial();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (IsCooking)
        {
            TimeCooked += Time.deltaTime;
            UpdateMaterial();
            UpdateTimeDisplay();
            return;
        }
        
        StartCoroutine(RemoveTimeDisplay());
        

        if (IsCooked)
        {
            // idk do something
            gameObject.name = $"{InitialName}";
        }
        else if (IsOvercooked)
        {
            gameObject.name = $"Burnt{InitialName}";
        }
    }

    private void UpdateMaterial()
    {
        if (IsCooked)
        {
            GetComponentInChildren<Renderer>().material = CookedMaterial;
        } 
        else if (IsOvercooked)
        {
            GetComponentInChildren<Renderer>().material = BurntMaterial;
        }
        else
        {
            GetComponentInChildren<Renderer>().material = RawMaterial;
        }
    }

    private IEnumerator RemoveTimeDisplay()
    {
        yield return new WaitForSeconds(2f);
        if (CookingProgressBar != null) Destroy(CookingProgressBar.gameObject);
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
        if (CookingProgressBar == null)
        {
            CookingProgressBar = Instantiate(CircularProgressBarPrefab, gameObject.transform);
            CookingProgressBar.transform.position += new Vector3(0, 0.01f, 0);
        }
        
        UpdateCookingBar();
        UpdateBurntBar();
    }

    private void UpdateBurntBar()
    {
        var burningBar = CookingProgressBar.transform.GetChild(1).GetComponentInChildren<Image>();
        var fill = Mathf.Clamp(((TimeCooked - CookingTime) / OvercookedThreshold), 0, 1);
        burningBar.fillAmount = fill;
    }

    private void UpdateCookingBar()
    {
        var cookingBar = CookingProgressBar.transform.GetChild(0).GetComponentInChildren<Image>();
        var fill = Mathf.Clamp((TimeCooked / CookingTime), 0, 1);
        cookingBar.fillAmount = fill;
    }
    
    
}
