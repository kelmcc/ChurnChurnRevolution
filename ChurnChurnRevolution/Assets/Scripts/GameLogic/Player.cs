using System.Collections.Generic;
using SoundManager;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private EffectSoundBank _sfx;
    [SerializeField] private EffectSoundBank _kickSfx;
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

    private KeyCode[] inputChain;
    private int currentChainIndex;
    private Slider progressBar;

    private float progress;
    private float decayRate = 0.1f; // How fast the bar falls
    private float fillAmount = 0.02f; // How much each correct input fills the bar

    public bool HasWon => progress >= 1f;
    public bool PleaseStop = false;

    [SerializeField] private KeyCode kickInput = KeyCode.Space;
    [SerializeField] private float kickDuration = 0.2f;
    [SerializeField] private float kickTimeOutDuration = 1f;

    private bool isKicking;
    private float currentKickTimeOut;
    private float currentKickingTimer;

    public void Initialize(KeyCode[] chain, Slider assignedProgressBar)
    {
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
    }

    private void HandleInput()
    {
        if (inputChain == null || inputChain.Length == 0)
        {
            return;
        }

        // Check input
        if (Input.GetKeyDown(inputChain[currentChainIndex]))
        {
            progress += fillAmount;
            currentChainIndex = (currentChainIndex + 1) % inputChain.Length;
            UpdateFX();
        }
        else if (Input.GetKeyDown(inputChain[(currentChainIndex + inputChain.Length - 1) % inputChain.Length]))
        {
            // Allow reverse order
            progress += fillAmount;
            currentChainIndex = (currentChainIndex + inputChain.Length - 1) % inputChain.Length;
            UpdateFX();
        }

        // Check for kick input
        if (Input.GetKeyDown(kickInput) && !isKicking && currentKickTimeOut <= 0f)
        {
            StartKickAction();
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
        currentKickingTimer = kickDuration;
        isKicking = true;

        _kickCollider.enabled = true;
        _sfx.Play();
    }

    public void TurnOffKick()
    {
        _bodyImage.sprite = _defaultSprite;
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
                currentKickTimeOut = kickTimeOutDuration;
            }
        }
        else if (currentKickTimeOut > 0f)
        {
            currentKickTimeOut -= Time.deltaTime;
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

            IKickable kickable = other.GetComponent<IKickable>();
            if (kickable != null)
            {
                
                kickable.OnKicked(_opponent);
            }

            TurnOffKick();
        }
    }
}
