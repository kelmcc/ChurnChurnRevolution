using UnityEngine;
using System.Collections.Generic;

public class FallingCowSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject cowPrefab;
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private Vector3 spawnAreaSize = new Vector3(5f, 0f, 5f);

    [Header("Despawn Bounds")]
    [SerializeField] private Vector3 minBounds = new Vector3(-20f, -10f, -20f);
    [SerializeField] private Vector3 maxBounds = new Vector3(20f, 50f, 20f);

    private float _timer;
    private readonly List<GameObject> _spawnedCows = new List<GameObject>();

    private void Update()
    {
        HandleSpawn();
        HandleBoundsCheck();
    }

    private void HandleSpawn()
    {
        _timer += Time.deltaTime;
        if (_timer >= spawnInterval)
        {
            _timer = 0f;
            SpawnCow();
        }
    }

    private void SpawnCow()
    {
        Vector3 randomOffset = new Vector3(
            Random.Range(-spawnAreaSize.x * 0.5f, spawnAreaSize.x * 0.5f),
            Random.Range(-spawnAreaSize.y * 0.5f, spawnAreaSize.y * 0.5f),
            Random.Range(-spawnAreaSize.z * 0.5f, spawnAreaSize.z * 0.5f)
        );

        Vector3 spawnPos = transform.position + randomOffset;
        GameObject cow = Instantiate(cowPrefab, spawnPos, Quaternion.identity, transform);
        _spawnedCows.Add(cow);
    }

    private void HandleBoundsCheck()
    {
        for (int i = _spawnedCows.Count - 1; i >= 0; i--)
        {
            GameObject cow = _spawnedCows[i];
            if (cow == null)
            {
                _spawnedCows.RemoveAt(i);
                continue;
            }

            Vector3 pos = cow.transform.position;
            if (pos.x < minBounds.x || pos.x > maxBounds.x ||
                pos.y < minBounds.y || pos.y > maxBounds.y ||
                pos.z < minBounds.z || pos.z > maxBounds.z)
            {
                Destroy(cow);
                _spawnedCows.RemoveAt(i);
            }
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, spawnAreaSize);

        Gizmos.color = Color.red;
        Vector3 boundsCenter = (minBounds + maxBounds) * 0.5f;
        Vector3 boundsSize = maxBounds - minBounds;
        Gizmos.DrawWireCube(boundsCenter, boundsSize);
    }
#endif
}

