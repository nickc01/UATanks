using UnityEngine;

public class PlayerSpecific : MonoBehaviour, IPlayerSpecific
{
    public int PlayerNumber { get; set; } = 1; //The player's number

    public virtual void OnNewPlayerChange() { } //Called when a screen is added or removed
}
