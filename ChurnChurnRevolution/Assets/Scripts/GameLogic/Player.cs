using System;
using System.Collections.Generic;
using SoundManager;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private EffectSoundBank _sfx;
    [SerializeField] private EffectSoundBank _kickSfx;
    [SerializeField] private EffectSoundBank _StunnedSfx;
    [SerializeField] private Image _armsImage;
    [SerializeField] private Image _bodyImage;
    [SerializeField] private Collider _kickCollider;
    [SerializeField] private Sprite[] _movementSprites;
    [SerializeField] private Sprite _defaultSprite;
    [SerializeField] private Sprite _kickSprite;
    [SerializeField] private List<GameObject> _butterBuildUp;
    [SerializeField] private GameObject _winState;
    [SerializeField] private Player _opponent;
    public Transform HeadTarget;
    [SerializeField] private GameObject StunnedAnimation;

    private KeyCode[] inputChain;
    private KeyCode _kickInput;
    private Joystick _joystick;
    private int currentChainIndex;
    private Slider progressBar;

    private float progress;
    private float decayRate = 0.1f; // How fast the bar falls
    private float fillAmount = 0.02f; // How much each correct input fills the bar

    public bool HasWon => progress >= 1f;
    public bool PleaseStop = false;

    private PlayerConfig _config;
    
    private bool isKicking;
    private float currentKickTimeOut;
    private float currentKickingTimer;

    private bool isStunned;
    private float currentStunTimer;

    [Serializable]
    public class PlayerConfig
    {
        public float StunDuration = 0.5f;
        public float KickDuration = 0.2f;
        public float KickTimeOutDuration = 1f;
    }
    
    public void Initialize(PlayerConfig playerConfig, KeyCode[] chain, KeyCode kickInput, Slider assignedProgressBar, Joystick joystick)
    {
        _config = playerConfig;
        _joystick = joystick;
        _kickInput = kickInput;
        _kickCollider.enabled = false;
        _winState.SetActive(false);

        TurnOffAllSplashes();

        inputChain = chain;
        progressBar = assignedProgressBar;
        currentChainIndex = 0;
        progress = 0f;

        if (progressBar != null)
        {
            progressBar.value = 0f;
        }
    }

    private void TurnOffAllSplashes()
    {
        foreach (var splash in _butterBuildUp)
        {
            splash.SetActive(false);
        }
    }

    private void Update()
    {
        if (PleaseStop)
        {
            return;
        }

        HandleInput();
        DecayProgress();
        UpdateProgressBar();
        UpdateButterSplash();
        UpdateKickState();
        UpdateStunState();
    }

    private void HandleInput()
    {
        if (inputChain == null || inputChain.Length == 0 || isStunned)
        {
            return;
        }

        // keyboard
        if (Input.GetKeyDown(inputChain[currentChainIndex]))
        {
            progress += fillAmount;
            currentChainIndex = (currentChainIndex + 1) % inputChain.Length;
            UpdateFX();
        }
        else if (Input.GetKeyDown(inputChain[(currentChainIndex + inputChain.Length - 1) % inputChain.Length]))
        {
            progress += fillAmount;
            currentChainIndex = (currentChainIndex + inputChain.Length - 1) % inputChain.Length;
            UpdateFX();
        }

        // joystick
        if (_joystick != null)
        {
            Vector2 stick = _joystick.stick.ReadValue();

            if (stick.x > 0.5f && currentChainIndex == 0)
            {
                TriggerChainAdvance();
            }

            if (stick.y > 0.5f && currentChainIndex == 1)
            {
                TriggerChainAdvance();
            }

            if (stick.x < -0.5f && currentChainIndex == 2)
            {
                TriggerChainAdvance();
            }

            if (stick.y < -0.5f && currentChainIndex == 3)
            {
                TriggerChainAdvance();
            }
        }

        // === Kick input ===
        if (Input.GetKeyDown(_kickInput) && !isKicking && currentKickTimeOut <= 0f)
        {
            StartKickAction();
        }
    }

    private void TriggerChainAdvance()
    {
        progress += fillAmount;
        currentChainIndex = (currentChainIndex + 1) % inputChain.Length;
        UpdateFX();
    }

    private void Stun()
    {
        isStunned = true;
        currentStunTimer = _config.StunDuration;
        StunnedAnimation.SetActive(true);
        _StunnedSfx.Play();
    }
    
    private void TurnOffStun()
    {
        isStunned = false;
        StunnedAnimation.SetActive(false);
    }

    public void UpdateJoystick(Joystick newJoy)
    {
        _joystick = newJoy;
    }
    
    private void UpdateStunState()
    {
        if (isStunned)
        {
            currentStunTimer -= Time.deltaTime;
            if (currentStunTimer <= 0f)
            {
                TurnOffStun();
            }
        }
    }

    private void DecayProgress()
    {
        if (progress > 0f)
        {
            progress -= decayRate * Time.deltaTime;
            progress = Mathf.Max(progress, 0f);
        }
    }

    private void UpdateProgressBar()
    {
        if (progressBar != null)
        {
            progressBar.value = progress;
        }
    }

    private void UpdateFX()
    {
        _armsImage.sprite = _movementSprites[currentChainIndex];
        _sfx.Play();
    }

    private void UpdateButterSplash()
    {
        int numberOfSplashes = Mathf.Clamp((int)(progress * _butterBuildUp.Count), 0, _butterBuildUp.Count);

        for (int i = 0; i < _butterBuildUp.Count; i++)
        {
            if (i < numberOfSplashes)
            {
                _butterBuildUp[i].SetActive(true);
            }
            else
            {
                _butterBuildUp[i].SetActive(false);
            }
        }
    }

    public void ShowWinState()
    {
        _winState.SetActive(true);
        _armsImage.gameObject.SetActive(false);
    }

    public void StartKickAction()
    {
        _bodyImage.sprite = _kickSprite;
        currentKickingTimer = _config.KickDuration;
        isKicking = true;

        _kickCollider.enabled = true;
        _sfx.Play();
    }

    public void TurnOffKick()
    {
        _kickCollider.enabled = false;
    }

    private void UpdateKickState()
    {
        if (isKicking)
        {
            currentKickingTimer -= Time.deltaTime;
            if (currentKickingTimer <= 0f)
            {
                isKicking = false;
                TurnOffKick();
                currentKickTimeOut = _config.KickTimeOutDuration;
            }
        }
        else if (currentKickTimeOut > 0f)
        {
            currentKickTimeOut -= Time.deltaTime;
            _bodyImage.sprite = _defaultSprite;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isKicking || !_kickCollider.enabled)
        {
            return;
        }

        if (other.CompareTag("Kickable"))
        {
            Debug.Log($"Kicked object: {other.name}");
            _kickSfx.Play();

            KickableCow kickable = other.GetComponent<KickableCow>();
            if (kickable != null)
            {
                kickable.OnCowReachingTarget += (player) =>
                {
                    player.Stun();
                };
                kickable.OnKicked(_opponent);
            }

            TurnOffKick();
        }
    }
}
