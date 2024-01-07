using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Menu
{
    public class HighlightDriver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private AudioClip _audio;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private Image _image;
        [SerializeField] private Color _color, _defaultColor;
        public void OnPointerEnter(PointerEventData eventData)
        {
            _audioSource.clip = _audio;
            _image.color = _color;
            _audioSource.Play();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _audioSource.Stop();
            _image.color = _defaultColor;
        }

        public void ResetHighlighter()
        {
            _audioSource.Stop();
            _image.color = _defaultColor;
        }
    }
}