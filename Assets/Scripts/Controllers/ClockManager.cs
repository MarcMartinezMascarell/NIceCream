using System;
using System.Collections;
using System.Collections.Generic;
using DPUtils.Systems.DateTime;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using DateTime = DPUtils.Systems.DateTime.DateTime;

public class ClockManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _dateText, _timeText, _seasonText, _yearText;
    [SerializeField] private Light2D _sunLight;

    public AnimationCurve dayNightCurve;
    private void OnEnable()
    {
        TimeManager.OnDateTimeChanged += UpdateClock;
    }
    
    private void OnDisable()
    {
        TimeManager.OnDateTimeChanged -= UpdateClock;
    }
    
    private void UpdateClock(DateTime dateTime)
    {
        _dateText.text = dateTime.DateToString();
        _timeText.text = dateTime.TimeToString();
        _seasonText.text = dateTime.Season.ToString();
        _yearText.text = $"Y{dateTime.Year.ToString()}";
        //Set intensity of sun light
        float dayNightT = dayNightCurve.Evaluate((float)dateTime.Hour / 24f);
        _sunLight.intensity = Mathf.Lerp(0.1f, 1f, dayNightT);
    }
}
