using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }
    private static InventoryManager instance;
    public GameObject sword;/// �׽�Ʈ�� ī���� ��

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


    private void Init()   /// ���� ���� �� ������ ��� ������ ���⸦ �о���� �ʱ�ȭ �޼ҵ�
    {
        /* 
           ---------���� �ڵ�---------
           �밭 �̷� �������� ���ư��ٰ� ������ �صξ����ϴ�.
           GameObject[] weapons = Database.LoadWeapons();
           for(int i = 0; i < weapons.Length; i++)
           {
                Player.Instance.weaponManager.RegisterWeapon(weapon[i]);
           }
        */

        // ī���� �� �׽�Ʈ��
        GameObject weapon = Instantiate(sword);
        Player.Instance.weaponManager.RegisterWeapon(weapon);
        Player.Instance.weaponManager.SetWeapon(weapon);
    }
}