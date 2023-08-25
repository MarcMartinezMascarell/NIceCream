using System;
using System.Collections;
using System.Collections.Generic;
using DPUtils.Systems.DateTime;
using TMPro;
using UnityEngine;
using DateTime = DPUtils.Systems.DateTime.DateTime;

public class ClockManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _dateText, _timeText, _seasonText, _yearText;

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
    }
}
