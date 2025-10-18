using System;
using System.Collections.Generic;
using SoundManager;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class WinLogic : MonoBehaviour
{
    [Header("Audio")] 
    [SerializeField] private EffectSoundBank _winMusic;
    [SerializeField] private EffectSoundBank _sfxMusic;
    
    [Header("Sequence Settings")] 
    [SerializeField] private float _returnToTitleDelay = 10f;
    [SerializeField] private bool _allowEarlySkip = true;
    
    // incrementally turn on balls and lights over time
    // first play for x period and then start turning on balls and lights
    [SerializeField] private List<GameObject> _discoBallEscalation = new List<GameObject>();
    [SerializeField] private List<GameObject> _discoLightsEscalation = new List<GameObject>();

    private float _timer;
    private bool _returnTriggered;

    private void Awake()
    {
        _timer = _returnToTitleDelay;
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
    }

    private bool CheckForSkipInput()
    {
        if (Input.anyKeyDown)
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