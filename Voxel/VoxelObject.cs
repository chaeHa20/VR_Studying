using UnityEngine;

public class VoxelObject : MonoBehaviour
{
    [SerializeField] Rigidbody m_rb;
    [SerializeField] float m_destroyedTime = 3.0f;
    [SerializeField] float m_movingSpeed = 5.0f;
    [SerializeField] MeshRenderer m_mesh;

    private int m_index = 0;

    public void initObject(VoxelGenerator generator)
    {
        Vector3 direction = Random.insideUnitSphere;
        m_rb.linearVelocity = direction * m_movingSpeed;
        m_mesh.material.color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 1.0f);

        ARVRCoroutineManager.instance.waitDestroyed(m_destroyedTime, () =>
        {
            generator.pushObject(this);
        });
    }
}
