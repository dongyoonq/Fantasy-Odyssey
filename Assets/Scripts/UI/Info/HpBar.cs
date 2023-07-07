using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    Slider hpBar;
    TMP_Text HpText;
    
    private void Start()
    {
        hpBar = GetComponent<Slider>();
        hpBar.maxValue = Player.Instance.Status.MaxHp;
        HpText = transform.GetChild(1).GetComponent<TMP_Text>();
    }

    void Update()
    {
        if (hpBar.IsValid())
        {
            hpBar.value = Player.Instance.CurrentHP;
            hpBar.maxValue = Player.Instance.Status.MaxHp;
        }

        if (HpText.IsValid())
            HpText.text = $"{Player.Instance.CurrentHP}/{Player.Instance.Status.MaxHp}";
    }
}