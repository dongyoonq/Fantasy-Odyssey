using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UserInfo : MonoBehaviour
{
    TMP_Text playerLevel;
    TMP_Text playerName;

    void Start()
    {
        playerLevel = GameObject.Find("UserLevel").transform.GetChild(0).GetComponent<TMP_Text>();
        playerName = GameObject.Find("UserLevel").transform.GetChild(1).GetComponent<TMP_Text>();
    }

    private void Update()
    {
        playerLevel.text = Player.Instance.Level.ToString();
        playerName.text = Player.Instance.PlayerName;
    }
}
