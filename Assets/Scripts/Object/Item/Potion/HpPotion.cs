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

        GameManager.Sound.PlaySFX("Potion 01");

        if (Player.Instance.CurrentHP >= Player.Instance.Status.MaxHp)
            return;

        if (!(Player.Instance.CurrentHP + potionData.Value > Player.Instance.Status.MaxHp))
        {
            Player.Instance.CurrentHP += potionData.Value;
            GameManager.Ui.SetFloating(Player.Instance.gameObject, +potionData.Value, new Color(0, 1, 0, 1));
        }
        else
        {
            int value = Player.Instance.Status.MaxHp - Player.Instance.CurrentHP;
            Player.Instance.CurrentHP += value;
            GameManager.Ui.SetFloating(Player.Instance.gameObject, +value, new Color(0, 1, 0, 1), 4f, 3f);
        }
    }
}