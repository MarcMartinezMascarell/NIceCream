using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NotificationsController : MonoBehaviour
{
    [SerializeField] private GameObject _notificationPanel;
    [SerializeField] private Image _notificationImage;
    [SerializeField] private TextMeshProUGUI _notificationPrimaryText;
    
    private float _notificationDisplayTime = 2f;
    private float _notificationFadeTime = 0.5f;
    private Coroutine _notificationCoroutine;
    private float _timeRemaining;

    private void Awake()
    {
        _notificationPanel.SetActive(false);
        _notificationImage.color = Color.clear;
        _notificationPrimaryText.text = "";
        //_notificationSecondaryText.text = "";
    }
    

    public void DisplayNotification(Sprite sprite, string primaryText, string secondaryText)
    {
        if (_notificationCoroutine != null)
        {
            StopCoroutine(_notificationCoroutine);
        }

        _notificationPanel.SetActive(true);
        _notificationImage.sprite = sprite;
        _notificationPrimaryText.text = primaryText;
        //_notificationSecondaryText.text = secondaryText;

        _notificationCoroutine = StartCoroutine(ManageNotification());
    }
    
    private IEnumerator ManageNotification()
    {
        _timeRemaining = _notificationDisplayTime;
        StartCoroutine(FadeInNotification());

        while (_timeRemaining > 0)
        {
            yield return null;
            _timeRemaining -= Time.deltaTime;
        }

        StartCoroutine(FadeOutNotification());
    }
    
    private IEnumerator FadeInNotification()
    {
        float elapsedTime = 0f;
        while (elapsedTime < _notificationFadeTime)
        {
            _notificationImage.color = Color.Lerp(Color.clear, Color.white, elapsedTime / _notificationFadeTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        _notificationImage.color = Color.white;
    }

    private IEnumerator FadeOutNotification()
    {
        float elapsedTime = 0f;
        while (elapsedTime < _notificationFadeTime)
        {
            _notificationImage.color = Color.Lerp(Color.white, Color.clear, elapsedTime / _notificationFadeTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        _notificationImage.color = Color.clear;
        _notificationPanel.SetActive(false);
    }
    
    

}
