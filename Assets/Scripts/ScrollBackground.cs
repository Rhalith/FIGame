using System.Collections;
using Scripts.EventBus;
using Scripts.Events;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class ScrollBackground : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _pitLaneSpeed;
        [SerializeField] private RawImage _trackImage;
        [SerializeField] private RawImage _pitLaneImage;

        public float Speed { get => _speed; set => _speed = value; }
        
        private bool _isPitLane;
        private Vector3 _startPosition;

        private void OnEnable()
        {
            EventBus<PitLaneEntranceEvent>.AddListener(SetPitLane);
            EventBus<StartPitEnterEvent>.AddListener(StartPitLaneMovement);
        }

        private void Start()
        {
            _startPosition = _pitLaneImage.transform.localPosition;
        }

        private void OnDisable()
        {
            EventBus<PitLaneEntranceEvent>.RemoveListener(SetPitLane);
            EventBus<StartPitEnterEvent>.RemoveListener(StartPitLaneMovement);
        }

        private void StartPitLaneMovement(object sender, StartPitEnterEvent @event)
        {
            _pitLaneImage.transform.localPosition = _startPosition;
            _isPitLane = true;
        }

        private void SetPitLane(object sender, PitLaneEntranceEvent @event)
        {
            if(!@event.IsEntering) StartCoroutine(ResetPitLane());
        }

        private void FixedUpdate()
        {
            _trackImage.uvRect = new Rect(_trackImage.uvRect.position + new Vector2(Time.deltaTime * Speed, _trackImage.uvRect.position.y) , _trackImage.uvRect.size);
            if(_isPitLane)
                _pitLaneImage.transform.localPosition += new Vector3(Time.deltaTime * Speed * -_pitLaneSpeed, 0, 0);
        }

        private IEnumerator ResetPitLane()
        {
            yield return new WaitForSeconds(1f);
            _isPitLane = false;
            _pitLaneImage.transform.localPosition = _startPosition;
        }
    }
}
