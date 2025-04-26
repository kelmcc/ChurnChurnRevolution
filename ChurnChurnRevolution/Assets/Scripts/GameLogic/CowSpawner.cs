using SoundManager;
using UnityEngine;
using UnityEngine.Pool;

public class CowSpawner : MonoBehaviour
{
    [SerializeField] private EffectSoundBank _sfx;
    
    [Header("Cow Settings")]
    [SerializeField] private Transform _cowPrefab;
    [SerializeField] private Transform _cowParent;

    [Header("Game World Bounds")]
    [SerializeField] private Vector2 _minBounds = new Vector2(-8f, -5f);
    [SerializeField] private Vector2 _maxBounds = new Vector2(8f, 5f);

    private ObjectPool<Transform> _cowPool;

    private void Awake()
    {
        _cowPool = new ObjectPool<Transform>(
            CreateCow,
            OnTakeCowFromPool,
            OnReturnCowToPool,
            OnDestroyCow,
            collectionCheck: false, 
            defaultCapacity: 10,
            maxSize: 100
        );
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnCow();
        }
    }

    private Transform CreateCow()
    {
        Transform cow = Instantiate(_cowPrefab, _cowParent);
        cow.gameObject.SetActive(false);
        _sfx.Play();
        return cow;
    }

    private void OnTakeCowFromPool(Transform cow)
    {
        cow.gameObject.SetActive(true);
    }

    private void OnReturnCowToPool(Transform cow)
    {
        cow.gameObject.SetActive(false);
    }

    private void OnDestroyCow(Transform cow)
    {
        Destroy(cow.gameObject);
    }

    private void SpawnCow()
    {
        Transform cow = _cowPool.Get();

        float randomX = Random.Range(_minBounds.x, _maxBounds.x);
        float randomY = Random.Range(_minBounds.y, _maxBounds.y);
        Vector3 spawnPosition = new Vector3(randomX, randomY,0f);

        Quaternion spawnRotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));

        cow.SetPositionAndRotation(spawnPosition, spawnRotation);
    }

    public void ReturnCowToPool(Transform cow)
    {
        _cowPool.Release(cow);
    }
}
