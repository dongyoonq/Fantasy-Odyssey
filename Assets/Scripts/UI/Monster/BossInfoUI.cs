using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossInfoUI : MonoBehaviour
{
    [SerializeField] public Slider hpBar;
    [SerializeField] public TMP_Text monsterName;

    private Monster boss;

    private void OnEnable()
    {
        boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<Monster>();
        monsterName.text = boss.name;
        hpBar.maxValue = boss.data.maxHp;
    }

    private void Update()
    {
        hpBar.value = boss.currHp;
    }
}
