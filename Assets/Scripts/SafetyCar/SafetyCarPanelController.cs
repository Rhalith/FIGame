﻿using Scripts.Events;
using Scripts.EventBus;
using TMPro;
using UnityEngine;
using DG.Tweening;

namespace Scripts.SafetyCar
{
    public class SafetyCarPanelController : MonoBehaviour
    {
        [SerializeField] private GameObject _safetyCarInfoPanel;
        [SerializeField] private GameObject _safetyCarPanel;
        [SerializeField] private TMP_Text _safetyCarText;
        [SerializeField] private float animationDuration = 0.5f;

        private void OnEnable()
        {
            EventBus<CallSafetyCarEvent>.AddListener(ShowSafetyCarPanel);
            EventBus<SendSafetyCarEvent>.AddListener(CloseSafetyCarPanel);
            EventBus<EndSafetyCarEvent>.AddListener(ChangeSafetyCarText);
        }

        private void OnDisable()
        {
            EventBus<CallSafetyCarEvent>.RemoveListener(ShowSafetyCarPanel);
            EventBus<SendSafetyCarEvent>.RemoveListener(CloseSafetyCarPanel);
            EventBus<EndSafetyCarEvent>.RemoveListener(ChangeSafetyCarText);
        }

        private void ShowSafetyCarPanel(object sender, CallSafetyCarEvent @event)
        {
            _safetyCarText.text = "STAY BEHIND\nSafety Car";
            _safetyCarPanel.SetActive(true);
            _safetyCarPanel.transform.localScale = Vector3.zero; // Start from 0 scale
            _safetyCarPanel.transform.DOScale(Vector3.one, animationDuration).SetEase(Ease.OutBack).OnComplete(() =>
            {
                _safetyCarInfoPanel.SetActive(true);
                EventBus<CheckSelectableElementEvent>.Emit(this, new CheckSelectableElementEvent { CanSelect = true });
            });
        }

        private void CloseSafetyCarPanel(object sender, SendSafetyCarEvent @event)
        {
            _safetyCarPanel.transform.DOScale(Vector3.zero, animationDuration)
                .SetEase(Ease.InBack)
                .OnComplete(() => _safetyCarPanel.SetActive(false)); // Disable after animation
        }

        private void ChangeSafetyCarText(object sender, EndSafetyCarEvent @event)
        {
            _safetyCarText.text = "Safety Car\nEnding..";
        }
    }
}