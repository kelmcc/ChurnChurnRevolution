using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class CloudSpawner : MonoBehaviour
{
    [Header("Cloud Settings")]
    [SerializeField] private List<Transform> _cloudPrefabs;
    [SerializeField] private Transform _cloudParent;

    [Header("Spawn Bounds")]
    [SerializeField] private Vector2 _minSpawnBounds = new Vector2(-8f, -5f);
    [SerializeField] private Vector2 _maxSpawnBounds = new Vector2(8f, 5f);
    
    [Header("Kill Bounds")]
    [SerializeField] private Vector2 _maxKillBounds = new Vector2(8f, 5f);

    [Header("Spawn Timing")]
    [SerializeField] private float _spawnInterval = 2f; 
    private float _spawnTimer;
    
    private ObjectPool<Transform> _cloudPool;

    private void Awake()
    {
        _cloudPool = new ObjectPool<Transform>(
            CreateCloud,
            OnTakeCloudFromPool,
            OnReturnCloudToPool,
            OnDestroyCloud,
            collectionCheck: false, 
            defaultCapacity: 10,
            maxSize: 100
        );
    }

    private void Update()
    {
        _spawnTimer += Time.deltaTime;

        if (_spawnTimer >= _spawnInterval)
        {
            _spawnTimer = 0f;
            SpawnCloud();
        }
    }

    private Transform CreateCloud()
    {
        Transform cloud = Instantiate(_cloudPrefabs[Random.Range(0, _cloudPrefabs.Capacity)], _cloudParent);
        cloud.gameObject.SetActive(false);
        return cloud;
    }

    private void OnTakeCloudFromPool(Transform cloud)
    {
        cloud.gameObject.SetActive(true);
    }

    private void OnReturnCloudToPool(Transform cloud)
    {
        cloud.gameObject.SetActive(false);
    }

    private void OnDestroyCloud(Transform cloud)
    {
        Destroy(cloud.gameObject);
    }

    private void SpawnCloud()
    {
        Transform cloud = _cloudPool.Get();

        float randomY = Random.Range(_minSpawnBounds.y, _maxSpawnBounds.y);
        Vector3 spawnPosition = new Vector3(_minSpawnBounds.x, randomY, 0f);

        cloud.SetPositionAndRotation(spawnPosition, Quaternion.identity);

        Cloud cloudComponent = cloud.GetComponent<Cloud>();
        if (cloudComponent != null)
        {
            cloudComponent.Initialize(this, _maxKillBounds.x);
        }
    }


    public void ReturnCloudToPool(Transform cloud)
    {
        _cloudPool.Release(cloud);
    }
}