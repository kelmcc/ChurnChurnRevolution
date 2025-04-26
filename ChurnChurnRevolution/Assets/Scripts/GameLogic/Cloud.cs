using UnityEngine;

public class Cloud : MonoBehaviour
{
    private float _speed;
    private CloudSpawner _spawner;
    private float _killX;

    public void Initialize(CloudSpawner spawner, float killX)
    {
        _spawner = spawner;
        _killX = killX;
        _speed = Random.Range(0.5f, 2f); // Random cloud speed
    }

    private void Update()
    {
        transform.position += Vector3.right * _speed * Time.deltaTime;

        if (transform.position.x > _killX)
        {
            _spawner.ReturnCloudToPool(transform);
        }
    }
}