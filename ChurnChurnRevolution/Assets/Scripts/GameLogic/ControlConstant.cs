using UnityEngine;

public class ControlConstant : MonoBehaviour
{
    public int Player1Joy = 0;
    public int Player2Joy = 1;
    public static ControlConstant Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SwitchInputs()
    {
        (Player1Joy, Player2Joy) = (Player2Joy, Player1Joy);
    }
}