using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class StatusUI : PopUpUI
{
    public GameObject statusPanel;
    public TMP_Text baseStatusText;
    public TMP_Text applyStatusText;
    public bool activeStatus = false;

    Vector2 orgPosition;

    private void Start()
    {
        Player.Instance.statusUI = this;
        Player.Instance.OnChangeEquipment.AddListener(ShowStatus);
        orgPosition = statusPanel.transform.GetChild(0).position;
        statusPanel.transform.GetChild(0).GetChild(0).GetComponent<Button>().onClick.AddListener(() => { OpenStatus(); GameManager.Sound.PlaySFX("Click"); });
    }

    public void OpenStatus()
    {
        GameManager.Sound.PlaySFX("OpenUI");
        activeStatus = !activeStatus;
        statusPanel.transform.GetChild(0).gameObject.SetActive(activeStatus);
        statusPanel.transform.GetChild(0).position = orgPosition;
        ShowStatus();
        statusPanel.transform.SetAsLastSibling();
    }

    public void OpenStatus(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            GameManager.Sound.PlaySFX("OpenUI");
            activeStatus = !activeStatus;
            statusPanel.transform.GetChild(0).gameObject.SetActive(activeStatus);
            statusPanel.transform.GetChild(0).position = orgPosition;
            ShowStatus();
            statusPanel.transform.SetAsLastSibling();
        }
    }

    public void ShowStatus()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append($"MaxHP  :  {Player.Instance.Status.maxHP}\n");
        sb.Append($"Deffense  :  {Player.Instance.Status.deffense}\n");
        sb.Append($"AttackPower  :  {Player.Instance.Status.attackPower}\n");
        sb.Append($"AttackSpeed  :  {Player.Instance.Status.attackSpeed}\n");
        sb.Append($"WalkSpeed  :  {Player.Instance.Status.walkSpeed}\n");
        sb.Append($"RunSpeed  :  {Player.Instance.Status.runSpeed}\n");
        sb.Append($"JumpPower  :  {Player.Instance.Status.jumpPower}\n");

        baseStatusText.text = sb.ToString();
        
        sb = new StringBuilder(); 
        sb.Append($"MaxHP  :  {Player.Instance.Status.MaxHp}  (<color=#FF4500> {Player.Instance.Status.maxHP}</color> <color=#60B38F>+</color> <color=#00D2FF>{Player.Instance.Status.MaxHp - Player.Instance.Status.maxHP} </color>)\n");
        sb.Append($"Deffense  :  {Player.Instance.Status.Deffense}  (<color=#FF4500> {Player.Instance.Status.deffense}</color> <color=#60B38F>+</color> <color=#00D2FF>{Player.Instance.Status.Deffense - Player.Instance.Status.deffense} </color>)\n");
        sb.Append($"AttackPower  :  {Player.Instance.Status.AttackPower}  (<color=#FF4500> {Player.Instance.Status.attackPower}</color> <color=#60B38F>+</color> <color=#00D2FF>{Player.Instance.Status.AttackPower - Player.Instance.Status.attackPower} </color>)\n");
        sb.Append($"AttackSpeed  :  {Player.Instance.Status.AttackSpeed}  (<color=#FF4500> {Player.Instance.Status.attackSpeed}</color> <color=#60B38F>+</color> <color=#00D2FF>{Player.Instance.Status.AttackSpeed - Player.Instance.Status.attackSpeed} </color>)\n");
        sb.Append($"WalkSpeed  :  {Player.Instance.Status.WalkSpeed}  (<color=#FF4500> {Player.Instance.Status.walkSpeed}</color> <color=#60B38F>+</color> <color=#00D2FF>{Player.Instance.Status.WalkSpeed - Player.Instance.Status.walkSpeed} </color>)\n");
        sb.Append($"RunSpeed  :  {Player.Instance.Status.RunSpeed}  (<color=#FF4500> {Player.Instance.Status.runSpeed}</color> <color=#60B38F>+</color> <color=#00D2FF>{Player.Instance.Status.RunSpeed - Player.Instance.Status.runSpeed} </color>)\n");
        sb.Append($"JumpPower  :  {Player.Instance.Status.JumpPower}  (<color=#FF4500> {Player.Instance.Status.jumpPower}</color> <color=#60B38F>+</color> <color=#00D2FF>{Player.Instance.Status.JumpPower - Player.Instance.Status.jumpPower} </color>)\n");

        applyStatusText.text = sb.ToString();
    }
}
