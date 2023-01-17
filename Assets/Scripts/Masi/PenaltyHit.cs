using Assets.Scripts.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.Masi
{
    public class PenaltyHit : MonoBehaviour
    {
        [SerializeField] private Penalty _penalty;
        [SerializeField] private Image _image;
        [SerializeField] private Rigidbody2D _rigidBody;
        [SerializeField] private float _speed;

        public Vector2 moveDirection;
        public float Speed { get => _speed; set => _speed = value; }

        public void ChangePenalty(Penalty penalty, Sprite sprite)
        {
            switch (penalty)
            {
                case Penalty.FiveSeconds:
                    _penalty = Penalty.FiveSeconds;
                    break;
                case Penalty.TenSeconds:
                    _penalty = Penalty.TenSeconds;
                    break;
            }
            _image.sprite = sprite;
        }
        public void Shoot(Transform target)
        {
            gameObject.SetActive(true);
            moveDirection = (target.localPosition - transform.localPosition).normalized * _speed * 500;
            _rigidBody.velocity = moveDirection;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            CheckCollision(collision);
        }

        private void CheckCollision(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                CheckPenalty(collision);
                gameObject.SetActive(false);
            }
            else if (!collision.CompareTag("SafetyCar"))
            {
                StartCoroutine(DisableObject());
            }
        }
        private void CheckPenalty(Collider2D collision)
        {
            if (_penalty.Equals(Penalty.FiveSeconds))
            {
                collision.GetComponent<PlayerSpecs>().DamagePlayer(5f);
            }
            else
            {
                collision.GetComponent<PlayerSpecs>().DamagePlayer(10f);
            }
        }
        IEnumerator DisableObject()
        {
            yield return new WaitForSeconds(1f);
            gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            transform.localPosition = new Vector3(750, 0, 0);
            _rigidBody.velocity = new Vector2(0,0);
        }

    }

    public enum Penalty
    {
        FiveSeconds,
        TenSeconds
    }
}