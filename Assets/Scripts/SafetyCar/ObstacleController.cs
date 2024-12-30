using System.Collections.Generic;
using Scripts.EventBus;
using Scripts.Events;
using UnityEngine;

namespace Scripts.SafetyCar
{
    public class ObstacleController : MonoBehaviour
    {
        [SerializeField] private GameObject obstaclePrefab; // Obstacle prefab
        [SerializeField] private List<Sprite> obstacleSprites; // List of obstacle sprites
        [SerializeField] private List<ObstacleHit> obstaclePool; // Pool of obstacles
        [SerializeField] private float spawnInterval = 2f; // Interval between spawns
        [SerializeField] private float spawnX = 10f; // X position for spawning obstacles
        [SerializeField] private float minY = -5f; // Minimum Y position
        [SerializeField] private float maxY = 5f; // Maximum Y position
        [SerializeField] private float minZRotation = -45f; // Minimum Z rotation
        [SerializeField] private float maxZRotation = 45f; // Maximum Z rotation
        [SerializeField] private float spawnDuration = 30f; // Duration to spawn obstacles

        private bool _canSpawnObstacles;
        private float _spawnTimer;
        private float _durationTimer;

        private void OnEnable()
        {
            EventBus<StartSafetyCarEvent>.AddListener(StartSpawningObstacles);
            EventBus<IncreaseDifficultyEvent>.AddListener(DecreaseSpawnInterval);
            EventBus<DisablePenaltiesEvent>.AddListener(DisableObstacles);
            EventBus<ResetSafetyCarEvent>.AddListener(ResetSafetyCar);
        }

        private void OnDisable()
        {
            EventBus<StartSafetyCarEvent>.RemoveListener(StartSpawningObstacles);
            EventBus<IncreaseDifficultyEvent>.RemoveListener(DecreaseSpawnInterval);
            EventBus<DisablePenaltiesEvent>.RemoveListener(DisableObstacles);
            EventBus<ResetSafetyCarEvent>.RemoveListener(ResetSafetyCar);
        }

        private void DisableObstacles(object sender, DisablePenaltiesEvent @event)
        {
            StopSpawningObstacles();
        }

        private void DecreaseSpawnInterval(object sender, IncreaseDifficultyEvent @event)
        {
            spawnInterval /= @event.IncreaseRate;
        }
        
        private void ResetSafetyCar(object sender, ResetSafetyCarEvent @event)
        {
            StopSpawningObstacles();
        }

        private void Update()
        {
            if (!_canSpawnObstacles) return;

            _spawnTimer += Time.deltaTime;
            _durationTimer += Time.deltaTime;

            // Spawn obstacles at regular intervals
            if (_spawnTimer >= spawnInterval)
            {
                _spawnTimer = 0f;
                SpawnObstacle();
            }

            // Stop spawning after the specified duration
            if (_durationTimer >= spawnDuration)
            {
                StopSpawningObstacles();
                EventBus<EndSafetyCarEvent>.Emit(this, new EndSafetyCarEvent());
            }
        }

        private void StartSpawningObstacles(object sender, StartSafetyCarEvent @event)
        {
            _canSpawnObstacles = true;
            _durationTimer = 0f; // Reset the timer when spawning starts
        }

        private void StopSpawningObstacles()
        {
            _canSpawnObstacles = false;
            ResetObstacles();
        }

        private ObstacleHit GetPooledObstacle()
        {
            foreach (var obstacle in obstaclePool)
            {
                if (!obstacle.gameObject.activeInHierarchy)
                {
                    PrepareObstacle(obstacle);
                    return obstacle;
                }
            }

            // Instantiate a new obstacle if none are available
            var newObstacle = Instantiate(obstaclePrefab).GetComponent<ObstacleHit>();
            obstaclePool.Add(newObstacle);
            PrepareObstacle(newObstacle);
            return newObstacle;
        }

        private void PrepareObstacle(ObstacleHit obstacle)
        {
            // Assign a random sprite to the obstacle
            int spriteIndex = Random.Range(0, obstacleSprites.Count);
            obstacle.ChangeObstacle(obstacleSprites[spriteIndex]);
        }

        private void SpawnObstacle()
        {
            var obstacle = GetPooledObstacle();

            // Generate a random Y position
            float randomY = Random.Range(minY, maxY);

            // Generate a random Z rotation
            float randomZRotation = Random.Range(minZRotation, maxZRotation);

            // Position and rotate the obstacle
            obstacle.transform.localPosition = new Vector2(spawnX, randomY);
            obstacle.transform.localEulerAngles = new Vector3(0, 0, randomZRotation);

            obstacle.gameObject.SetActive(true);
        }

        private void ResetObstacles()
        {
            foreach (var obstacle in obstaclePool)
            {
                obstacle.gameObject.SetActive(false);
            }
        }
    }
}
