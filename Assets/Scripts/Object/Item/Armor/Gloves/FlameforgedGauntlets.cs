using UnityEngine;

public class FlameforgedGauntlets : Armor
{
    private void OnEnable()
    {
        equipmentData = Resources.Load<EquipmentData>("Data/ItemData/ArmorData/Gloves/Flameforged Gauntlets");
        armorData = Resources.Load<ArmorData>("Data/ItemData/ArmorData/Gloves/Flameforged Gauntlets");
    }

    public FlameforgedGauntlets(ArmorData data) : base(data)
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