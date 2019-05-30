using UnityEngine;

public class PlayerSpecific : MonoBehaviour, IPlayerSpecific
{
    public int PlayerNumber { get; set; } = 1;

    public virtual void OnNewPlayerChange() { }
}
