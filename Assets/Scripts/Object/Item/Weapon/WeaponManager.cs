using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager
{
    // ���� ���� ��ũ��Ʈ
    public Weapon Weapon { get; private set; }
    // �� �κ��� ũ�� �Ű� �� ���ŵ� �˴ϴ�.
    public Action<GameObject> unRegisterWeapon { get; set; }
    // ���⸦ ��� ���� Ʈ������
    private Transform handPosition;
    // ���� �� ���� ������Ʈ
    private GameObject weaponObject;
    // ���� WeaponManager�� ��ϵ� ���� ����Ʈ
    private List<GameObject> weapons = new List<GameObject>();

    public WeaponManager(Transform hand)
    {
        handPosition = hand;
    }

    // ���� ���
    public void RegisterWeapon(GameObject weapon)
    {
        if (!weapons.Contains(weapon))
        {
            Weapon weaponInfo = weapon.GetComponent<Weapon>();
            weapon.transform.SetParent(handPosition);
            weapon.transform.localPosition = weaponInfo.WeaponData.localPosition;
            weapon.transform.localEulerAngles = weaponInfo.WeaponData.localRotation;
            weapon.transform.localScale = weaponInfo.WeaponData.localScale;
            weapons.Add(weapon);
            weapon.SetActive(false);
        }
    }

    // ���� ����
    public void UnRegisterWeapon(GameObject weapon)
    {
        if (weapons.Contains(weapon))
        {
            weapons.Remove(weapon);
            unRegisterWeapon.Invoke(weapon);
        }
    }

    // ���� ����
    public void SetWeapon(GameObject weapon)
    {
        if (Weapon == null)
        {
            weaponObject = weapon;
            Weapon = weapon.GetComponent<Weapon>();
            weaponObject.SetActive(true);
            Player.Instance.animator.runtimeAnimatorController = Weapon.WeaponData.WeaponAnimator;
            return;
        }

        for (int i = 0; i < weapons.Count; i++)
        {
            if (weapons[i].Equals(Weapon))
            {
                weaponObject = weapon;
                weaponObject.SetActive(true);
                Weapon = weapon.GetComponent<Weapon>();
                Player.Instance.animator.runtimeAnimatorController = Weapon.WeaponData.WeaponAnimator;
                continue;
            }
            weapons[i].SetActive(false);
        }
    }
}