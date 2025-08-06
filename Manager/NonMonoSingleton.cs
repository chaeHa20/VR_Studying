public abstract class NonMonoSingleton<T> where T : class, new()
{
    protected static T m_instance = null;

    public static T instance
    {
        get
        {
            if (null == m_instance)
                m_instance = new T();

            return m_instance;
        }
    }
    public static U getInstance<U>() where U : class
    {
        T t = instance;
        if (null == t)
            m_instance = new T();

        return t as U;
    }

    public static bool isNullInstance()
    {
        return null == m_instance;
    }
}