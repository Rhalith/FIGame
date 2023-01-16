using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Menu
{
    public class Healthbar : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private Gradient _gradient;
        [SerializeField] private Image _fill;
        public void SetHealth(float health)
        {
            _slider.value = health;
            _fill.color = _gradient.Evaluate(_slider.normalizedValue);
        }
    }
}