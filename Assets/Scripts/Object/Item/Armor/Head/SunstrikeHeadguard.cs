using UnityEngine;

public class SunstrikeHeadguard : Armor
{
    private void OnEnable()
    {
        equipmentData = Resources.Load<EquipmentData>("Data/ItemData/ArmorData/Head/Sunstrike Headguard");
        armorData = Resources.Load<ArmorData>("Data/ItemData/ArmorData/Head/Sunstrike Headguard");
    }

    public SunstrikeHeadguard(ArmorData data) : base(data)
    {
    }

    public override void ApplyStatusModifier()
    {
        Player.Instance.Status.MaxHp += armorData.MaxHP;
        Player.Instance.Status.Deffense += armorData.Deffense;
        Player.Instance.Status.WalkSpeed += armorData.WalkSpeed;
        Player.Instance.Status.RunSpeed += armorData.RunSpeed;
        Player.Instance.Status.JumpPower += armorData.JumpPower;
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