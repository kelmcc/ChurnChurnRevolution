using System.Collections.Generic;
using SoundManager;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class WinLogic : MonoBehaviour
{
    [Header("Audio")] 
    [SerializeField] private EffectSoundBank _winMusic;
    [SerializeField] private EffectSoundBank _sfxMusic;
    
    [Header("Sequence Settings")] 
    [SerializeField] private float _returnToTitleDelay = 10f;
    [SerializeField] private bool _allowEarlySkip = true;

    [Header("Escalation Settings")]
    [SerializeField] private float _escalationStartDelay = 2f;
    [SerializeField] private float _escalationInterval = 1f;
    [SerializeField] private List<GameObject> _discoBallEscalation = new List<GameObject>();
    [SerializeField] private List<GameObject> _discoLightsEscalation = new List<GameObject>();

    [SerializeField] private CanvasGroup _character;
    
    private float _timer;
    private float _escalationTimer;
    private int _escalationIndex = 0;
    private bool _returnTriggered;
    private bool _escalationStarted;

    private void Awake()
    {
        _timer = _returnToTitleDelay;
        _escalationTimer = _escalationStartDelay;

        foreach (var ball in _discoBallEscalation)
        {
            if (ball != null)
            {
                ball.SetActive(false);
            }
        }
        foreach (var light in _discoLightsEscalation)
        {
            if (light != null)
            {
                light.SetActive(false);
            }
        }
    }

    private void Start()
    {
        if (_winMusic != null)
        {
            _winMusic.Play();
        }

        if (_sfxMusic != null)
        {
            _sfxMusic.Play();
        }
    }

    private void Update()
    {
        if (_returnTriggered)
        {
            return;
        }

        _timer -= Time.deltaTime;
        if (_timer <= 0f)
        {
            ReturnToTitle();
        }

        if (_allowEarlySkip && CheckForSkipInput())
        {
            ReturnToTitle();
        }

        HandleEscalation();
    }

    private void HandleEscalation()
    {
        if (!_escalationStarted)
        {
            _escalationTimer -= Time.deltaTime;
            if (_escalationTimer <= 0f)
            {
                _escalationStarted = true;
                _escalationTimer = _escalationInterval;
            }
            return;
        }

        _escalationTimer -= Time.deltaTime;
        if (_escalationTimer <= 0f && _escalationIndex < Mathf.Max(_discoBallEscalation.Count, _discoLightsEscalation.Count))
        {
            if (_escalationIndex < _discoBallEscalation.Count && _discoBallEscalation[_escalationIndex] != null)
            {
                _discoBallEscalation[_escalationIndex].SetActive(true);
            }

            if (_escalationIndex < _discoLightsEscalation.Count && _discoLightsEscalation[_escalationIndex] != null)
            {
                _discoLightsEscalation[_escalationIndex].SetActive(true);
            }

            _escalationIndex++;
            _escalationTimer = _escalationInterval;
        }
    }

    private bool CheckForSkipInput()
    {
        if (Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame)
        {
            return true;
        }

        foreach (var joy in Joystick.all)
        {
            if (joy.trigger.wasPressedThisFrame)
            {
                return true;
            }

            Vector2 stick = joy.stick.ReadValue();
            if (stick.magnitude > 0.5f)
            {
                return true;
            }
        }

        return false;
    }

    private void ReturnToTitle()
    {
        _returnTriggered = true;
        SceneManager.LoadScene(0);
    }
}