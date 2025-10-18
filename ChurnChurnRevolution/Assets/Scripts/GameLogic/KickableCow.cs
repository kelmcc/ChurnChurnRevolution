using System;
using SoundManager;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class KickableCow : MonoBehaviour
{
    [Header("Visuals")]
    [SerializeField] private Image _image;
    [SerializeField] private Sprite _kickedSprite;

    private Transform _kickTo;
    private Player _targetedPlayer;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float reachThreshold = 0.5f;
    [SerializeField] private float flingForce = 10f;

    [SerializeField] private EffectSoundBank _sfx;
    public Action<Player> OnCowReachingTarget;

    private bool _isFlyingAway = false;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void OnKicked(Player targetPlayer)
    {
        Debug.Log("Cow was kicked!");
        _image.sprite = _kickedSprite;

        _targetedPlayer = targetPlayer;
        _kickTo = targetPlayer.HeadTarget;
        _isFlyingAway = false;
        _sfx.Play();
        _rigidbody.linearDamping = 0;
        
        if (_rigidbody != null)
        {
            _rigidbody.isKinematic = true;
        }
    }

    private void Update()
    {
        if (_isFlyingAway || _kickTo == null)
        {
            return;
        }

        Vector3 direction = (_kickTo.position - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, _kickTo.position);

        transform.position += direction * moveSpeed * Time.deltaTime;

        if (distance <= reachThreshold)
        {
            Debug.Log("Cow reached player!");
            OnCowReachingTarget?.Invoke(_targetedPlayer);
            FlingAway();
        }
    }

    private void FlingAway()
    {
        _isFlyingAway = true;
        _kickTo = null;

        if (_rigidbody == null)
        {
            _rigidbody = gameObject.AddComponent<Rigidbody>();
        }

        _rigidbody.isKinematic = false;
        Vector3 randomDir = new Vector3(UnityEngine.Random.Range(-1f, 1f), 1f, UnityEngine.Random.Range(-1f, 1f)).normalized;
        _rigidbody.AddForce(randomDir * flingForce, ForceMode.Impulse);
    }
}