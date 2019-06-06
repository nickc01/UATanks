
public enum PlayerCount
{
    OnePlayer,
    TwoPlayers
}


public class PlayerCountDropDown : SavedEnumDropdown<PlayerCount>
{
    public override PlayerCount DefaultValue => PlayerCount.OnePlayer;
}