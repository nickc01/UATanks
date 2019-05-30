using Object = UnityEngine.Object;

public interface IPlayerSpecificInstantiation
{
    Object DupeObject(Object original);
    void DestroyObject(Object instance);
}
