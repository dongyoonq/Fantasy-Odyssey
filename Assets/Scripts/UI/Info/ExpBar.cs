using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExpBar : MonoBehaviour
{
    Slider expBar;
    TMP_Text expText;
    
    private void Start()
    {
        expBar = GetComponent<Slider>();
        expBar.maxValue = Player.Instance.NextLevelExp;
        //expText = transform.GetChild(1).GetComponent<TMP_Text>();
        Player.Instance.OnLevelUp.AddListener(NextLevelUpdate);
    }

    void Update()
    {
        expBar.value = Player.Instance.Exp;
        //expText.text = $"{Player.Instance.Exp}/{Player.Instance.NextLevelExp}";
    }

    void NextLevelUpdate()
    {
        expBar.maxValue = Player.Instance.NextLevelExp;
    }
}