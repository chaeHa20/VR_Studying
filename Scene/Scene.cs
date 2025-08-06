using System;
using UnityEditorInternal;
using UnityEngine;

public class Scene : MonoBehaviour, IDisposable
{
    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {
    }

    private void Update()
    {
        update(Time.deltaTime);
    }

    private void FixedUpdate()
    {
        fixedUpdate();
    }

    protected virtual void update(float dt)
    {
    }

    protected virtual void fixedUpdate()
    {
    }


    public virtual void Dispose()
    {
        Dispose(true);
    }

    protected virtual void Dispose(bool disposing)
    {

    }

    public static void destroy(Scene scene)
    {
        if (null == scene)
            return;

        scene.Dispose();
        GameObject.Destroy(scene.gameObject);
    }
}
