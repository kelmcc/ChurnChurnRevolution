using System.Collections;
using SoundManager;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private EffectSoundBank _music;

    [SerializeField] private Player _player1;
    [SerializeField] private Player _player2;

    [SerializeField] private Slider _player1ProgressBar;
    [SerializeField] private Slider _player2ProgressBar;
    
    [SerializeField] private Player.PlayerConfig _playerConfig;

    private Player _winningPlayer = null;

    [SerializeField] private WinLogic _winLogic;
    [SerializeField] private Transitions _transitions;

    private int WinnerSceneIndex => _player1.HasWon ? 2 : 3;

    private void Awake()
    {
        _transitions.OnTransOutComplete += () =>
        {
            // do something at the start
        };
    }

    private void Start()
    {
        _transitions.TransitionOut();
        
        EffectSoundInstance instance = _music.Play();
        instance.IsLooping = true;

        var joysticks = Joystick.all;

        Joystick p2Joystick = joysticks.Count > 0 ? joysticks[0] : null;
        KeyCode p2Kick = KeyCode.Space;
        Joystick p1Joystick = joysticks.Count > 1 ? joysticks[1] : null;
        KeyCode p1Kick = KeyCode.Mouse0;

        Debug.Log($"p1Joystick deviceId {p1Joystick?.deviceId}\np2Joystick {p2Joystick?.deviceId}");
        _player1.Initialize(
            _playerConfig,
            new KeyCode[] { KeyCode.D, KeyCode.W, KeyCode.A, KeyCode.S }, p1Kick,
            _player1ProgressBar,
            p1Joystick
        );

        _player2.Initialize(
            _playerConfig,
            new KeyCode[] { KeyCode.RightArrow, KeyCode.UpArrow, KeyCode.LeftArrow, KeyCode.DownArrow },
            p2Kick,
            _player2ProgressBar,
            p2Joystick
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
            _winningPlayer = _player1;
            TriggerWinUI();
        }
        else if (_player2.HasWon)
        {
            Debug.Log("Player 2 Wins!");
            _winningPlayer = _player2;
            TriggerWinUI();
        }
    }

    private void TriggerWinUI()
    {
        _player1.PleaseStop = true;
        _player2.PleaseStop = true;

        _winningPlayer.ShowWinState();
        StartCoroutine(PlayWin());

        _transitions.OnTransInComplete += () =>
        {
            SceneManager.LoadScene(WinnerSceneIndex);
        };
    }

    private IEnumerator PlayWin()
    {
        yield return new WaitForSeconds(0.5f);
        _transitions.TransitionIn();

        yield return null;
    }
}