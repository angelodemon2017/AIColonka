public interface IWVState
{
    int GetCountLaunched { get; }
    float GetIntervalLaunched { get; }
    Weapon GetWeapon { get; }
}