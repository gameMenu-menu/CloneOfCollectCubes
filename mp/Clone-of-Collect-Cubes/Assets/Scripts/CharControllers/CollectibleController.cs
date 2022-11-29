using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleController : MonoBehaviour
{

    MeshRenderer mRenderer;
    Collider coll;
    Rigidbody rB;

    public void Initialize(Vector3 pos, Color color)
    {
        if(mRenderer == null)
        {
            mRenderer = GetComponent<MeshRenderer>();
        }
        if(coll == null)
        {
            coll = GetComponent<Collider>();
        }
        if(rB == null)
        {
            rB = GetComponent<Rigidbody>();
        }
        rB.isKinematic = false;
        coll.enabled = true;
        mRenderer.material.SetColor("_Color", color);
        transform.position = pos;
        transform.rotation = Quaternion.identity;
    }

    public void OnCollect(Material mat, int nLayer, bool player)
    {
        mRenderer.material = mat;

        StartCoroutine(CheckRB(nLayer, player));
    }

    IEnumerator CheckRB(int nLayer, bool player)
    {

        while(true)
        {
            if(rB.velocity.sqrMagnitude < 0.01f)
            {
                coll.enabled = false;
                rB.isKinematic = true;

                gameObject.layer = nLayer;

                if(player)
                {
                    SceneManager.Instance.controller.CollectCube(transform);
                }
                else
                {
                    SceneManager.Instance.controller.EnemyCollectCube(transform);
                }
                
                yield break;
            }

            yield return null;
        }
    }
}
