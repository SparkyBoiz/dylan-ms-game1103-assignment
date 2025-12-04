using UnityEngine;
using System.Reflection;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    private static readonly object _lock = new object();
    private static bool _isQuitting = false;

    public static T Instance
    {
        get
        {
            if (_isQuitting)
            {
                return null;
            }

            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = FindFirstObjectByType<T>(FindObjectsInactive.Include);
                    
                    if (_instance == null)
                    {
                        var attribute = typeof(T).GetCustomAttribute<PrefabAttribute>();
                        if (attribute != null)
                        {
                            GameObject prefab = Resources.Load<GameObject>(attribute.Path);
                            if (prefab != null)
                            {
                                GameObject go = Instantiate(prefab);
                                go.name = $"{typeof(T).Name} (Singleton)";
                            }
                            else
                            {
                    
                            }
                        }
                        else
                        {
                            GameObject singletonObject = new GameObject();
                            singletonObject.name = $"{typeof(T).Name} (Singleton)";
                            _instance = singletonObject.AddComponent<T>();
                        }
                    }
                }
                return _instance;
            }
        }
    }

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
            DontDestroyOnLoad(gameObject);
            OnAwake();
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    protected virtual void OnAwake() { }

    protected virtual void OnApplicationQuit()
    {
        _isQuitting = true;
    }
}