[System.Serializable]
public class Damage
{
    public EnumDamageType DamageType;
    public int ValueDamage;

    public Damage(EnumDamageType damageType, int damage)
    {
        DamageType = damageType;
        ValueDamage = damage;
    }
}