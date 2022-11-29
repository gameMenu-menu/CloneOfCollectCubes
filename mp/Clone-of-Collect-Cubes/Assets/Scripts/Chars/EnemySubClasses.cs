using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasyEnemy : Enemy
{

}

public class HardEnemy : Enemy
{
    protected override bool FindSpot()
    {
        Transform ground = SceneManager.Instance.stageController.GetGround();

        Vector3 size = ground.GetComponent<MeshFilter>().mesh.bounds.size;

        size.x *= ground.localScale.x;
        size.z *= ground.localScale.z;

        Vector3 rightTopCorner = ground.position - size / 2f;
        Vector3 leftBottomCorner = ground.position + size / 2f;

        leftBottomCorner.y = Self.position.y;
        rightTopCorner.y = Self.position.y;

        float rightDiff = Mathf.Abs(rightTopCorner.x - leftBottomCorner.x);
        float topDiff = Mathf.Abs(leftBottomCorner.z - rightTopCorner.z);

        int countX = 0;
        int countZ = 0;

        countX = (int) ( rightDiff / meshSize.x ) + 1;
        countZ = (int) ( topDiff / meshSize.z ) + 1;

        int max = 0;
        Vector3 maxCenter = Vector3.zero;

        Vector3 center = leftBottomCorner;

        Vector3 temp = meshSize;
        temp.y = 0;

        for(int i=0; i<countX; i++)
        {
            for(int k=0; k<countZ; k++)
            {
                Collider[] colls = Physics.OverlapBox(center, meshSize, Quaternion.identity, CubeMask);
                if(colls.Length > max)
                {
                    max = colls.Length;
                    maxCenter = center;
                    
                }

                center.z -= temp.z;
            }
            center.x -= temp.x;
            center.z = leftBottomCorner.z;
        }

        Vector3 dirToMaxCenter = maxCenter - Self.position;

        dirToMaxCenter = dirToMaxCenter.normalized;

        CubeSpot = maxCenter + dirToMaxCenter * 10f;

        if(max < 3)
        {
            Transform cube = SceneManager.Instance.stageController.GetCube();

            Vector3 pos = cube.position;
            pos.y = Self.position.y;

            CubeSpot = pos;
        }



        return true;
    }

    
}
