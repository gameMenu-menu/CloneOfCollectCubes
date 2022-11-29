using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectPlaceController : MonoBehaviour
{
    const string stringLayerCollectible = "Collectible";
    
    int nLayerCollectible;

    const string stringLayerCollecting = "Collecting";
    int nLayerCollecting, nLayerEnemyCube, nLayerPlayerCube;
    
    List<Transform> Collectings = new List<Transform>();

    public Material CollectedMaterial;

    public bool EnemyPlace;

    public float collectSpeed;

    void Start()
    {
        nLayerCollectible = LayerMask.NameToLayer(stringLayerCollectible);
        nLayerCollecting = LayerMask.NameToLayer(stringLayerCollecting);

        nLayerEnemyCube = LayerMask.NameToLayer(EnemyController.stringLayerCarryingCube);
        nLayerPlayerCube = LayerMask.NameToLayer(PlayerController.stringLayerCarryingCube);
    }

    void Update()
    {
        if(Collectings.Count != 0)
        {
            for(int i=0; i<Collectings.Count; i++)
            {

                Vector3 dir = transform.position - Collectings[i].position;

                

                dir = dir.normalized;
                Collectings[i].position += dir * collectSpeed * Time.deltaTime;

                dir = transform.position - Collectings[i].position;

                if(dir.sqrMagnitude <= 1f)
                {
                    
                    if(!EnemyPlace)
                    {
                        Collectings[i].GetComponent<CollectibleController>().OnCollect(CollectedMaterial, nLayerCollectible, true);
                    }
                    else
                    {
                        Collectings[i].GetComponent<CollectibleController>().OnCollect(CollectedMaterial, nLayerCollectible, false);
                    }

                    Collectings.Remove(Collectings[i]);
                    i--;

                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer.Equals(nLayerCollectible) || other.gameObject.layer.Equals(nLayerEnemyCube) || other.gameObject.layer.Equals(nLayerPlayerCube))
        {
            if( !Collectings.Contains(other.transform) )
            {
                Collectings.Add(other.transform);

                other.gameObject.layer = nLayerCollecting;
            }
        }
    }
}
