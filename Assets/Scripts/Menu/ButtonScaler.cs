using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Scripts.Menu
{
    public class ButtonScaler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler, IPointerClickHandler, ISubmitHandler
    {
        [SerializeField] private float scaleFactor = 1.1f; // How much to scale up
        [SerializeField] private float scaleSpeed = 0.1f;  // Speed of scaling

        private Vector3 _originalScale;

        private void Awake()
        {
            _originalScale = transform.localScale;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            // Scale up on hover
            ScaleToSize(scaleFactor);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            // Return to normal size
            ScaleToSize(1.0f);
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            transform.localScale = _originalScale;
        }
        
        public void OnSubmit(BaseEventData eventData)
        {
            transform.localScale = _originalScale;
        }
        
        
        public void OnSelect(BaseEventData eventData)
        {
            // Scale up when selected
            ScaleToSize(scaleFactor);
        }

        public void OnDeselect(BaseEventData eventData)
        {
            // Return to normal size
            ScaleToSize(1.0f);
        }

        private void ScaleToSize(float targetScale)
        {
            StopAllCoroutines();
            StartCoroutine(ScaleOverTime(targetScale));
        }

        private IEnumerator ScaleOverTime(float targetScale)
        {
            Vector3 target = _originalScale * targetScale;

            while (Vector3.Distance(transform.localScale, target) > 0.01f)
            {
                transform.localScale = Vector3.Lerp(transform.localScale, target, scaleSpeed * Time.unscaledDeltaTime);
                yield return null;
            }

            transform.localScale = target;
        }
    }

}