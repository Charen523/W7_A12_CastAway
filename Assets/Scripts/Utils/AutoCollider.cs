using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AutoCollider : MonoBehaviour
{
#if UNITY_EDITOR
    private void OnValidate()
    {
        Renderer mr = GetComponentInChildren<MeshRenderer>();
        if (mr == null) mr = GetComponentInChildren<SkinnedMeshRenderer>();
        var box = GetComponentInChildren<BoxCollider>();
        box.center = mr.bounds.center - transform.position;
        box.size = mr.bounds.size;
        UnityEditor.EditorApplication.delayCall += () =>
        {
            DestroyImmediate(this);
        };
    }
#endif
}