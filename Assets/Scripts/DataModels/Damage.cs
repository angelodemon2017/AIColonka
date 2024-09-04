public class Damage
{
    public bool _fromPlayer;
    public EnumDamageType DamageType;
    public int ValueDamage;

    public Damage(EnumDamageType damageType, int damage, bool fromPlayer)
    {
        DamageType = damageType;
        ValueDamage = damage;
        _fromPlayer = fromPlayer;
    }
}