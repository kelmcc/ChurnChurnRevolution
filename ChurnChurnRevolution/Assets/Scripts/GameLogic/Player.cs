using System.Collections.Generic;
using SoundManager;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private EffectSoundBank _sfx;
    [SerializeField] private Image _characterImage;
    [SerializeField] private Sprite[] _movementSprites;
    [SerializeField] private List<GameObject> _butterBuildUp;
    
    private KeyCode[] inputChain;
    private int currentChainIndex;
    private Slider progressBar;

    private float progress;
    private float decayRate = 0.1f; // How fast the bar falls
    private float fillAmount = 0.02f; // How much each correct input fills the bar

    public bool HasWon => progress >= 1f;

    public void Initialize(KeyCode[] chain, Slider assignedProgressBar)
    {
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
        HandleInput();
        DecayProgress();
        UpdateProgressBar();
        UpdateButterSplash();
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
        //Debug.Log($"Progress: {progress}, Current chain index: {currentChainIndex}");
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
        _characterImage.sprite = _movementSprites[currentChainIndex];
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

}