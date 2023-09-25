using UnityEngine;
using Sirenix.OdinInspector;

public sealed class SceneDebugger : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, GetComponent<BoxCollider>().size);
        Gizmos.color = Color.blue;
    }
}