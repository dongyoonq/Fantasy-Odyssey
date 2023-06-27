using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MonsterInfoUI : MonoBehaviour
{
    [NonSerialized] public Animator animator;
    [SerializeField] public Slider hpBar;
    [SerializeField] public TMP_Text monsterName;

    private void Start()
    {
        Player.Instance.monsterUI = this;
        animator = GetComponent<Animator>();
    }

    public void finishActive()
    {
        animator.SetBool("Active", false);
    }

    public void finishUnActive()
    {
        animator.SetBool("UnActive", false);
    }
}
