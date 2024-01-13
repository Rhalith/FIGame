using System;
using Scripts.EventBus;
using Scripts.Events;
using UnityEngine;
using UnityEngine.Serialization;
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

        private void OnEnable()
        {
            EventBus<StartPitEnterEvent>.AddListener(SetPitLane);
        }
        
        private void OnDisable()
        {
            EventBus<StartPitEnterEvent>.RemoveListener(SetPitLane);
        }

        private void SetPitLane(object sender, StartPitEnterEvent @event)
        {
            _isPitLane = true;
        }

        private void FixedUpdate()
        {
            _trackImage.uvRect = new Rect(_trackImage.uvRect.position + new Vector2(Time.deltaTime * Speed, _trackImage.uvRect.position.y) , _trackImage.uvRect.size);
            if(_isPitLane)
                _pitLaneImage.transform.localPosition += new Vector3(Time.deltaTime * Speed * -_pitLaneSpeed, 0, 0);
        }
    }
}
