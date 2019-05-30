using UnityEngine;

public class PlayerSpecific : MonoBehaviour, IPlayerSpecific
{
    public int PlayerID { get; set; } = 1;

    public virtual void OnNewPlayerChange() { }
}
