using UnityEngine;

public class HpPortion : Portion
{
    public HpPortion(PortionData data) : base(data)
    {
    }

    public override void Use()
    {
        base.Use();

        if (Player.Instance.CurrentHP >= Player.Instance.Status.MaxHp)
            return;

        if (!(Player.Instance.CurrentHP + portionData.Value > Player.Instance.Status.MaxHp))
            Player.Instance.CurrentHP += portionData.Value;
        else
            Player.Instance.CurrentHP += Player.Instance.Status.MaxHp - Player.Instance.CurrentHP;
    }
}