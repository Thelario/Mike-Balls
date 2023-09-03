using Game.Managers;
using UnityEngine;

namespace Game
{
	public class SpawnerManager : Singleton<SpawnerManager>
	{
		[Header("References")]
		[SerializeField] private Transform cameraTransform;
		[SerializeField] private Transform thisTransform;
		
		[Header("Prefabs")]
		[SerializeField] private GameObject[] enemies;
		[SerializeField] private GameObject[] bosses;
		
		[Header("Fields")]
		[SerializeField] private float initialSpawnTime;

		[Header("Spawn Offsets")] 
		[SerializeField] private float distanceFromCenterToSpawn;

		[Header("Draw Gizmos")] 
		[SerializeField] private bool drawGizmos;

		private float _spawnTime;
		private float _spawnTimeCounter;
		private bool _canStartSpawning;

		public void ConfigureSpawner()
		{
			_spawnTime = initialSpawnTime;
			_spawnTimeCounter = _spawnTime;
			_canStartSpawning = true;
			
			DestroyEnemies();
		}

		private void Update()
		{
			CheckSpawnEnemy();
		}
		
		private void CheckSpawnEnemy()
		{
			if (!_canStartSpawning)
				return;
			
			_spawnTimeCounter -= Time.deltaTime;
			if (_spawnTimeCounter <= 0f)
				SpawnEnemy();
		}
		
		private void SpawnEnemy()
		{
			_spawnTimeCounter = _spawnTime;
			Instantiate(enemies[Random.Range(0, enemies.Length)], GetRandomSpawnPoint(), Quaternion.identity, thisTransform);
		}

		public void SpawnBoss(int level)
		{
			Instantiate(bosses[level], GetRandomSpawnPoint(), Quaternion.identity, thisTransform);
		}

		public void LevelUp()
		{
			_spawnTime /= Random.Range(1.15f, 1.35f);
		}

		public void PushAllEnemiesBackwards()
		{
			if (thisTransform == null)
				return;
			
			foreach (Transform enemy in thisTransform)
			{
				if (!enemy.TryGetComponent(out IPushable ip))
					return;
				
				ip.Push(3.5f, .4f);
			}
		}

		private void DestroyEnemies()
		{
			if (thisTransform.childCount <= 0)
				return;
			
			foreach (Transform t in thisTransform)
				Destroy(t.gameObject);
		}
		
		private Vector3 GetRandomSpawnPoint()
		{
			float x, y;
			do {
				x = Random.Range(-1f, 1f);
				y = Random.Range(-1f, 1f);
			}
			while (x == 0f && y == 0f);

			Vector3 pos = distanceFromCenterToSpawn * new Vector3(x, y).normalized + CameraFollow.Instance.CameraPosition;
			
			return new Vector3(pos.x, pos.y, 0f);
		}
		
		private void OnDrawGizmos()
		{
			if (drawGizmos)
				Gizmos.DrawSphere(Vector3.zero, distanceFromCenterToSpawn);
		}
	}
}