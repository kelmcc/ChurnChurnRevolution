using System.Collections;
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

    [SerializeField] private GameObject _winBG;
    [SerializeField] private GameObject _win1;
    [SerializeField] private GameObject _win2;
    [SerializeField] private GameObject _win3;

    private void Start()
    {
        EffectSoundInstance instance = _music.Play();
        instance.IsLooping = true;

        _winBG.SetActive(false);
        _win1.SetActive(false);
        _win2.SetActive(false);
        _win3.SetActive(false);

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
            _player1.PleaseStop = true;
            Debug.Log("Player 1 Wins!");
            //Time.timeScale = 0f;
            _winningPlayer = _player1;
            TriggerWinUI();
        }
        else if (_player2.HasWon)
        {
            _player2.PleaseStop = true;
            Debug.Log("Player 2 Wins!");
            //Time.timeScale = 0f;
            _winningPlayer = _player2;
            TriggerWinUI();
        }
    }

    private void TriggerWinUI()
    {
        _winningPlayer.ShowWinState();
        StartCoroutine(PlayWin());
    }

    private IEnumerator PlayWin()
    {
        _winBG.SetActive(true);
        
        _win1.SetActive(true);

        yield return new WaitForSeconds(2f);

        _win1.SetActive(false);
        _win2.SetActive(true);
        yield return new WaitForSeconds(2f);
        
        _win2.SetActive(false);
        _win3.SetActive(true);
        Time.timeScale = 0f;
        yield return null;
    }
}