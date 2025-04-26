using SoundManager;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private EffectSoundBank _music;

    [SerializeField] private Player _player1;
    [SerializeField] private Player _player2;

    [SerializeField] private Slider _player1ProgressBar;
    [SerializeField] private Slider _player2ProgressBar;

    private Player _winningPlayer = null;
    
    private void Start()
    {
        EffectSoundInstance instance = _music.Play();
        instance.IsLooping = true;
        
        _player1.Initialize(
            new KeyCode[] { KeyCode.D, KeyCode.W, KeyCode.A, KeyCode.S },
            _player1ProgressBar
        );
        _player2.Initialize(
            new KeyCode[] { KeyCode.RightArrow, KeyCode.UpArrow, KeyCode.LeftArrow, KeyCode.DownArrow },
            _player2ProgressBar
        );
    }

    private void Update()
    {
        if (_winningPlayer != null)
        {
            return;
        }
        
        if (_player1.HasWon)
        {
            Debug.Log("Player 1 Wins!");
            Time.timeScale = 0f;
            _winningPlayer = _player1;
            TriggerWinUI();
        }
        else if (_player2.HasWon)
        {
            Debug.Log("Player 2 Wins!");
            Time.timeScale = 0f;
            _winningPlayer = _player2;
            TriggerWinUI();
        }
    }

    private void TriggerWinUI()
    {
        // What happens if we win?
    }
}