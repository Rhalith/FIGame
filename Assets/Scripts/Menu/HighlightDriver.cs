using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Scripts.Events;
using Scripts.EventBus;

namespace Scripts.Menu
{
    public class HighlightDriver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler, IPointerClickHandler, ISubmitHandler
    {
        [SerializeField] private AudioClip _audio;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private Image _image;
        [SerializeField] private Color _color, _defaultColor;
        public void OnPointerEnter(PointerEventData eventData)
        {
            Highlight();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Unhighlight();
        }
        
        public void OnSelect(BaseEventData eventData)
        {
            Highlight();
        }

        public void OnDeselect(BaseEventData eventData)
        {
            Unhighlight();
        }
        
        public void ResetHighlighter()
        {
            Unhighlight();
        }

        private void Unhighlight()
        {
            _audioSource.Stop();
            _image.color = _defaultColor;
        }

        private void Highlight()
        {
            _audioSource.clip = _audio;
            _image.color = _color;
            _audioSource.Play();
        }


        public void OnPointerClick(PointerEventData eventData)
        {
            EventBus<CheckSelectableElementEvent>.Emit(this, new CheckSelectableElementEvent { CanSelect = false });
        }

        public void OnSubmit(BaseEventData eventData)
        {
            EventBus<CheckSelectableElementEvent>.Emit(this, new CheckSelectableElementEvent { CanSelect = false });
        }
    }
}