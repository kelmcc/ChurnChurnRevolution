using UnityEngine;

public class KickableCow : MonoBehaviour, IKickable
{
    public void OnKicked(Player kicker)
    {
        Debug.Log("Cow was kicked!");
        Destroy(gameObject);
    }
}

public interface IKickable
{
    void OnKicked(Player kicker);
}