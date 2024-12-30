using Scripts.Player;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.SafetyCar
{
    public class ObstacleHit : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _damage = 10f;
        [SerializeField] private Image _image;

        private void Update()
        {
            MoveLeft();
        }

        private void MoveLeft()
        {
            transform.localPosition += Vector3.left * (_speed * Time.deltaTime  * 100);
        }

        public void ChangeObstacle(Sprite sprite)
        {
            _image.sprite = sprite;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                collision.GetComponent<PlayerSpecs>()?.DamagePlayer(_damage);
                gameObject.SetActive(false);
            }
        }
    }
}