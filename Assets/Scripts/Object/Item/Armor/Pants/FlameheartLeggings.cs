using UnityEngine;

public class FlameheartLeggings: Armor
{
    private void OnEnable()
    {
        equipmentData = Resources.Load<EquipmentData>("Data/ItemData/ArmorData/Pants/Flameheart Leggings");
        armorData = Resources.Load<ArmorData>("Data/ItemData/ArmorData/Pants/Flameheart Leggings");
    }

    public FlameheartLeggings(ArmorData data) : base(data)
    {
    }

    public override void ApplyStatusModifier()
    {
        Player.Instance.Status.MaxHp += armorData.MaxHP;
        Player.Instance.Status.Deffense += armorData.Deffense;
        Player.Instance.Status.WalkSpeed += armorData.WalkSpeed;
        Player.Instance.Status.RunSpeed += armorData.RunSpeed;
        Player.Instance.Status.JumpPower += armorData.JumpPower;

        if (Player.Instance.CurrentHP > Player.Instance.Status.MaxHp)
            Player.Instance.currentHp = Player.Instance.Status.MaxHp;
    }

    public override void RemoveStatusModifier()
    {
        Player.Instance.Status.MaxHp -= armorData.MaxHP;
        Player.Instance.Status.Deffense -= armorData.Deffense;
        Player.Instance.Status.WalkSpeed -= armorData.WalkSpeed;
        Player.Instance.Status.RunSpeed -= armorData.RunSpeed;
        Player.Instance.Status.JumpPower -= armorData.JumpPower;
    }
}