using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using UnityEngine;

public class MonsterSpawn : MonoBehaviour
{
    [SerializeField] public string monsterId;
    [SerializeField] public List<SpawnPoint> spawnPoint;
    [SerializeField] int spawnTime;
    [NonSerialized] public Monster[] monters;
    [NonSerialized] public int currMonster;

    private void Awake()
    {
        monters = new Monster[spawnPoint.Count];
        for (int i = 0; i < spawnPoint.Count; i++)
        {
            CreateMonster(i);
        }

        currMonster = monters.Length;
    }

    private void Update()
    {
        if (currMonster < spawnPoint.Count)
        {
            for (int i = 0; i < spawnPoint.Count; i++)
            {
                if (spawnPoint[i].state == SpawnPoint.State.Empty)
                {
                    if (currMonster < monters.Length)
                        currMonster++;
                    StartCoroutine(SpawnRoutine(i));
                }
            }
        }
    }
    
    IEnumerator SpawnRoutine(int index)
    {
        yield return new WaitForSeconds(spawnTime);
        CreateMonster(index);
    }

    void CreateMonster(int index)
    {
        if (monters[index] != null && monters[index].IsValid())
            return;

        monters[index] = GameManager.Resource.Instantiate<Monster>($"Prefabs/Monster/{monsterId}/{monsterId}", spawnPoint[index].transform.position, spawnPoint[index].transform.rotation, transform.parent);
        spawnPoint[index].state = SpawnPoint.State.Use;
        monters[index].gameObject.name = $"{monsterId}";
        monters[index].spawnInfo = this;
    }
}