public abstract class Equipment : Item
{
    public EquipmentData equipmentData;

    public Equipment(EquipmentData data) : base(data)
    {
        equipmentData = data;
    }

    public abstract void ApplyStatusModifier();
    public abstract void RemoveStatusModifier();
}