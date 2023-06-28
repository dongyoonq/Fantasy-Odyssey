using UnityEngine;

public class HpPotion : Potion
{
    public HpPotion(PotionData data) : base(data)
    {
    }

    private void OnEnable()
    {
        potionData = Resources.Load<PotionData>("Data/ItemData/PotionData/Hp Potion");
    }

    public override void Use()
    {
        base.Use();

        if (Player.Instance.CurrentHP >= Player.Instance.Status.MaxHp)
            return;

        if (!(Player.Instance.CurrentHP + potionData.Value > Player.Instance.Status.MaxHp))
            Player.Instance.CurrentHP += potionData.Value;
        else
            Player.Instance.CurrentHP += Player.Instance.Status.MaxHp - Player.Instance.CurrentHP;
    }
}