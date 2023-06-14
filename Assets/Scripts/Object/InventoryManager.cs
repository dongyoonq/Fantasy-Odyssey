using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }
    private static InventoryManager instance;
    public GameObject sword;/// 테스트용 카론의 노

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }
        DestroyImmediate(gameObject);
    }

    void Start()
    {
        Init();
    }


    private void Init()   /// 게임 시작 후 유저의 사용 가능한 무기를 읽어오는 초기화 메소드
    {
        /* 
           ---------슈도 코드---------
           대강 이런 로직으로 돌아간다고만 생각을 해두었습니다.
           GameObject[] weapons = Database.LoadWeapons();
           for(int i = 0; i < weapons.Length; i++)
           {
                Player.Instance.weaponManager.RegisterWeapon(weapon[i]);
           }
        */

        // 카론의 노 테스트용
        GameObject weapon = Instantiate(sword);
        Player.Instance.weaponManager.RegisterWeapon(weapon);
        Player.Instance.weaponManager.SetWeapon(weapon);
    }
}