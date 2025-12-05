using UnityEngine;
public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
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
                    // Try to find an existing instance in the scene
                    _instance = FindFirstObjectByType<T>();

                    // If no instance exists, create a new one
                    if (_instance == null)
                    {
                        GameObject singletonObject = new GameObject();
                        _instance = singletonObject.AddComponent<T>();
                        singletonObject.name = $"{typeof(T).Name} (Singleton)";
                    }
                }
                return _instance;
            }
        }
    }

    protected virtual void Awake() // Changed to protected to allow overrides if needed
    {
        if (_instance != null && _instance != this)
        {
            Debug.LogWarning($"Duplicate Singleton of type {typeof(T).Name}. Destroying the new one.");
            Destroy(gameObject);
        }
        else
        {
            _instance = (T)this;
            DontDestroyOnLoad(gameObject);
            OnAwake();
        }
    }

    protected virtual void OnAwake() { } // For derived classes to override

    protected virtual void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }

    protected virtual void OnApplicationQuit()
    {
        _isQuitting = true;
    }
}