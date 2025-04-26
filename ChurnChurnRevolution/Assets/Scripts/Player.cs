using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private KeyCode[] inputChain;
    private int currentChainIndex;
    private Slider progressBar;

    private float progress;
    private float decayRate = 0.1f; // How fast the bar falls
    private float fillAmount = 0.02f; // How much each correct input fills the bar

    public bool HasWon => progress >= 1f;

    public void Initialize(KeyCode[] chain, Slider assignedProgressBar)
    {
        inputChain = chain;
        progressBar = assignedProgressBar;
        currentChainIndex = 0;
        progress = 0f;
        
        if (progressBar != null)
        {
            progressBar.value = 0f;
        }
    }

    private void Update()
    {
        HandleInput();
        DecayProgress();
        UpdateProgressBar();
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
        }
        else if (Input.GetKeyDown(inputChain[(currentChainIndex + inputChain.Length - 1) % inputChain.Length]))
        {
            // Allow reverse order
            progress += fillAmount;
            currentChainIndex = (currentChainIndex + inputChain.Length - 1) % inputChain.Length;
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
}