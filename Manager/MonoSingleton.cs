using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] bool m_isDontDestroyOnLoad = true;

    private static T m_instance = null;

    public static T instance{get {return m_instance;}}

    public static U getInstance<U>() where U : MonoBehaviour
    {
        T t = instance;
        if (null == t)
            return null;

        return t as U;
    }

    protected virtual void Awake()
    {
        if (m_instance == null)
        {
            m_instance = this as T;

            if (Application.isPlaying)
            {
                if (m_isDontDestroyOnLoad)
                    DontDestroyOnLoad(gameObject);
            }
        }
    }
}
