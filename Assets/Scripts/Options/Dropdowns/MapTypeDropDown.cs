public enum MapType
{
    Random,
    MapOfTheDay
}

public class MapTypeDropDown : SavedEnumDropdown<MapType>
{
    public override MapType DefaultValue => MapType.Random;
}