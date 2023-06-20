using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class SpiderSpawn : MonoBehaviour
{
    [SerializeField] public List<SpiderSpawnPoint> spawnPoint;
    [SerializeField] int spawnTime;
    [NonSerialized] public Spider[] spiders;
    [NonSerialized] public int currSpider;

    private void Awake()
    {
        spiders = new Spider[spawnPoint.Count];
        for (int i = 0; i < spawnPoint.Count; i++)
        {
            CreateSpider(i);
        }

        currSpider = spiders.Length;
    }

    private void Update()
    {
        if (currSpider < spawnPoint.Count)
        {
            for (int i = 0; i < spawnPoint.Count; i++)
            {
                if (spawnPoint[i].state == SpiderSpawnPoint.State.Empty)
                {
                    currSpider++;
                    StartCoroutine(SpawnRoutine(i));
                }
            }
        }
    }
    
    IEnumerator SpawnRoutine(int index)
    {
        yield return new WaitForSeconds(spawnTime);
        CreateSpider(index);
    }

    void CreateSpider(int index)
    {
        spiders[index] = GameManager.Resouce.Instantiate<Spider>("Prefabs/Monster/Spider", spawnPoint[index].transform.position, spawnPoint[index].transform.rotation, transform.parent);
        spawnPoint[index].state = SpiderSpawnPoint.State.Use;
        spiders[index].gameObject.name = "Spider";
        spiders[index].spawnInfo = this;
    }
}