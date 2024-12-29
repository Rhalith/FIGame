using Scripts.Player;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.SafetyCar
{
    public class ObstacleHit : MonoBehaviour
    {
        [SerializeField] private float _speed; // Speed of the obstacle movement
        [SerializeField] private float _damage = 10f; // Damage dealt by the obstacle
        [SerializeField] private Image _image; // Sprite renderer for obstacle

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