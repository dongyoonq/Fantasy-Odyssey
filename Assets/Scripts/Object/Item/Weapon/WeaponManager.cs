using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager
{
    // 현재 무기 스크립트
    public Weapon Weapon { get; private set; }
    // 이 부분은 크게 신경 안 쓰셔도 됩니다.
    public Action<GameObject> unRegisterWeapon { get; set; }
    // 무기를 쥐는 손의 트랜스폼
    private Transform handPosition;
    // 현재 내 무기 오브젝트
    private GameObject weaponObject;
    // 현재 WeaponManager에 등록된 무기 리스트
    private List<GameObject> weapons = new List<GameObject>();

    public WeaponManager(Transform hand)
    {
        handPosition = hand;
    }

    // 무기 등록
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

    // 무기 삭제
    public void UnRegisterWeapon(GameObject weapon)
    {
        if (weapons.Contains(weapon))
        {
            weapons.Remove(weapon);
            unRegisterWeapon.Invoke(weapon);
        }
    }

    // 무기 변경
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