
public enum Difficulty
{
    Easy,
    Medium,
    Hard
}



public class DifficultyDropDown : SavedEnumDropdown<Difficulty>
{
    public override Difficulty DefaultValue => Difficulty.Medium;
}
