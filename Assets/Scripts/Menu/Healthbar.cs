using Scripts.Events;
using Scripts.EventBus;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Menu
{
    public class Healthbar : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private Gradient _gradient;
        [SerializeField] private Image _fill;
        [SerializeField] private TMP_Text _healthText;
        [SerializeField] private Animator _animator;
        
        private float _health;

        private void OnEnable()
        {
            EventBus<ChangeHealthEvent>.AddListener(ChangeHealth);
        }
        
        private void OnDisable()
        {
            EventBus<ChangeHealthEvent>.RemoveListener(ChangeHealth);
        }

        private void ChangeHealth(object sender, ChangeHealthEvent @event)
        {
            _health += @event.HealthChange;
            if(_health < 0)
            {
                _health = 0;
            }
            SetHealth(_health);
            ChangeHealthText(@event.HealthChange);
        }

        private void SetHealth(float health)
        {
            _slider.value = health;
            _health = health;
            _fill.color = _gradient.Evaluate(_slider.normalizedValue);
        }

        private void ChangeHealthText(float healthChange)
        {
            _healthText.text = _health.ToString();
            if(healthChange <= 0)
            {
                _animator.SetTrigger("damage");
            }
        }
    }
}