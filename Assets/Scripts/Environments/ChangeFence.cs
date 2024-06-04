using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeFence : MonoBehaviour
{
    // 새로운 Mesh를 설정할 변수
    public Mesh newFenceMesh;

    // 프리팹들에 접근할 부모 프리팹
    public GameObject parentPrefab;

    // 울타리 재료 확보되었는지 체크(나무 확보되었으면 true)
    public bool hasFenceMaterials = false;

    void Update()
    {
        if (hasFenceMaterials)
        {
            InstallNewFence();
        }
    }

    // 새로운 울타리 설치 메서드
    void InstallNewFence()
    {
        // 자식 프리팹들 찾기
        MeshFilter[] meshFilters = parentPrefab.GetComponentsInChildren<MeshFilter>();

        // 모든 자식 프리팹의 Mesh 변경
        foreach (MeshFilter mf in meshFilters)
        {
            mf.mesh = newFenceMesh;
        }

        hasFenceMaterials = false;
    }

}
