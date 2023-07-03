using UnityEngine;

public class StealthwalkersDust : Armor
{
    private void OnEnable()
    {
        equipmentData = Resources.Load<EquipmentData>("Data/ItemData/ArmorData/Boots/Stealthwalkers Dust");
        armorData = Resources.Load<ArmorData>("Data/ItemData/ArmorData/Boots/Stealthwalkers Dust");
    }

    public StealthwalkersDust(ArmorData data) : base(data)
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