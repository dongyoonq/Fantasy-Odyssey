using System.Collections;
using System.Collections.Generic;
using UnityEngine.Pool;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    Dictionary<string, ObjectPool<GameObject>> poolDic;
    Transform poolRoot;
    Canvas canvasRoot;

    private void Awake()
    {
        poolDic = new Dictionary<string, ObjectPool<GameObject>>();
        poolRoot = new GameObject("PoolRoot").transform;
        canvasRoot = GameManager.Resource.Instantiate<Canvas>("UI/Canvas");
    }

    public T Get<T>(T original) where T : Object
    {
        return Get(original, Vector3.zero, Quaternion.identity, null);
    }

    public T Get<T>(T original, Transform parent) where T : Object
    {
        return Get(original, Vector3.zero, Quaternion.identity, parent);
    }

    public T Get<T>(T original, Vector3 position, Quaternion rotation) where T : Object
    {
        return Get(original, position, rotation, null);
    }

    public T Get<T>(T original, Vector3 position, Quaternion rotation, Transform parent) where T : Object
    {
        if (original == null)
            Debug.Log("NULL 참조 오류");

        if (original is GameObject)
        {
            GameObject prefab = original as GameObject;

            if (!poolDic.ContainsKey(prefab.name))
                CreatePool(prefab.name, prefab);

            ObjectPool<GameObject> pool = poolDic[prefab.name];
            GameObject obj = pool.Get();

            obj.transform.position = position;
            obj.transform.rotation = rotation;
            obj.transform.parent = parent;

            return obj as T;
        }
        else if (original is Component)
        {
            Component component = original as Component;
            string key = component.gameObject.name;

            if (!poolDic.ContainsKey(key))
                CreatePool(key, component.gameObject);

            GameObject obj = poolDic[key].Get();

            obj.transform.position = position;
            obj.transform.rotation = rotation;
            obj.transform.parent = parent;

            return obj.GetComponent<T>();
        }
        else
            return null;
    }

    public bool Release<T>(T original)
    {
        if (original is GameObject)
        {
            GameObject obj = original as GameObject;
            string key = obj.name;

            if (!poolDic.ContainsKey(key))
                return false;

            poolDic[key].Release(obj);
            return true;
        }
        else if (original is Component)
        {
            Component component = original as Component;
            string key = component.gameObject.name;

            if (!poolDic.ContainsKey(key))
                return false;

            poolDic[key].Release(component.gameObject);
            return true;
        }
        else
            return false;
    }

    public bool isContain<T>(T original) where T : Object
    {
        if (original is GameObject)
        {
            GameObject obj = original as GameObject;
            string key = obj.name;

            if (!poolDic.ContainsKey(key))
                return false;
            else
                return true;
        }
        else if (original is Component)
        {
            Component component = original as Component;
            string key = component.gameObject.name;

            if (!poolDic.ContainsKey(key))
                return false;
            else
                return true;
        }
        else
            return false;
    }

    public IEnumerator ReleaseRoutine<T>(T gameObject, float releaseTime)
    {
        yield return new WaitForSeconds(releaseTime);
        Release(gameObject);
    }

    private void CreatePool(string key, GameObject prefab)
    {
        ObjectPool<GameObject> pool = new ObjectPool<GameObject>
            (
                createFunc: () =>
                {
                    GameObject obj = Instantiate(prefab);
                    obj.name = key;
                    return obj;
                },

                actionOnGet: (GameObject obj) =>
                {
                    obj.SetActive(true);
                    obj.transform.SetParent(null);
                },

                actionOnRelease: (GameObject obj) =>
                {
                    obj.SetActive(false);
                    obj.transform.SetParent(poolRoot.transform);
                },

                actionOnDestroy: (GameObject obj) =>
                {
                    Destroy(obj);
                }
            );

        poolDic.Add(key, pool);
    }

    public T GetUI<T>(T original, Vector3 position) where T : Object
    {
        if (original is GameObject)
        {
            GameObject prefab = original as GameObject;
            string key = prefab.name;

            if (!poolDic.ContainsKey(key))
                CreateUIPool(key, prefab);

            GameObject obj = poolDic[key].Get();
            obj.transform.position = position;
            return obj as T;
        }
        else if (original is Component)
        {
            Component component = original as Component;
            string key = component.gameObject.name;

            if (!poolDic.ContainsKey(key))
                CreateUIPool(key, component.gameObject);

            GameObject obj = poolDic[key].Get();
            obj.transform.position = position;
            return obj.GetComponent<T>();
        }
        else
        {
            return null;
        }
    }

    public T GetUI<T>(T original) where T : Object
    {
        if (original == null)
            Debug.Log("NULL 참조 오류");

        if (original is GameObject)
        {
            GameObject prefab = original as GameObject;
            string key = prefab.name;

            if (!poolDic.ContainsKey(key))
                CreateUIPool(key, prefab);

            GameObject obj = poolDic[key].Get();
            return obj as T;
        }
        else if (original is Component)
        {
            Component component = original as Component;
            string key = component.gameObject.name;

            if (!poolDic.ContainsKey(key))
                CreateUIPool(key, component.gameObject);

            GameObject obj = poolDic[key].Get();
            return obj.GetComponent<T>();
        }
        else
        {
            return null;
        }
    }

    public bool ReleaseUI<T>(T instance) where T : Object
    {
        if (instance is GameObject)
        {
            GameObject go = instance as GameObject;
            string key = go.name;

            if (!poolDic.ContainsKey(key))
                return false;

            poolDic[key].Release(go);
            return true;
        }
        else if (instance is Component)
        {
            Component component = instance as Component;
            string key = component.gameObject.name;

            if (!poolDic.ContainsKey(key))
                return false;

            poolDic[key].Release(component.gameObject);
            return true;
        }
        else
        {
            return false;
        }
    }

    private void CreateUIPool(string key, GameObject prefab)
    {
        ObjectPool<GameObject> pool = new ObjectPool<GameObject>(
            createFunc: () =>
            {
                GameObject obj = Instantiate(prefab);
                obj.gameObject.name = key;
                return obj;
            },
            actionOnGet: (GameObject obj) =>
            {
                obj.gameObject.SetActive(true);
            },
            actionOnRelease: (GameObject obj) =>
            {
                obj.gameObject.SetActive(false);
                obj.transform.SetParent(canvasRoot.transform, false);
            },
            actionOnDestroy: (GameObject obj) =>
            {
                Destroy(obj);
            }
            );
        poolDic.Add(key, pool);
    }
}
