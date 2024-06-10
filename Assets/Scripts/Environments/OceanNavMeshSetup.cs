using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class OceanNavMeshSetup : MonoBehaviour
{
    public NavMeshSurface navMeshSurface;
    public NavMeshModifierVolume navMeshModifierVolume;

    void Start()
    {
        // 수면 아래에 NavMesh 설정
        navMeshSurface.center = new Vector3(0, -5, 0); // 수면 아래로 이동
        navMeshSurface.size = new Vector3(800, 10, 800); // 영역 크기 설정
        navMeshSurface.BuildNavMesh(); // NavMesh 생성

        // Modifier Volume 설정
        navMeshModifierVolume.center = new Vector3(0, -5, 0); // 수면 아래로 이동
        navMeshModifierVolume.size = new Vector3(800, 10, 800); // 영역 크기 설정
        navMeshModifierVolume.area = 0; // Walkable 영역으로 설정
    }
}