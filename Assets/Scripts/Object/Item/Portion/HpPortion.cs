using UnityEngine;

public class HpPortion : Portion
{
    public HpPortion(PortionData data) : base(data)
    {
    }

    public override void Use()
    {
        base.Use();

        Player.Instance.CurrentHP += portionData.Value;

        if (Player.Instance.CurrentHP >= Player.Instance.Status.MaxHp)
            Player.Instance.CurrentHP = Player.Instance.Status.MaxHp;
    }
}