using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum Hardness { Easy, Hard }

public class EnemyController : MonoBehaviour
{
    public const string stringLayerNonCarryingCube = "Collectible";
    public const string stringLayerCarryingCube = "EnemyCube";

    
    public Hardness hardness;

    public Rigidbody RB;

    public LayerMask CubeMask, WallMask;
    public Transform Base;

    public Transform StartPosition;

    Quaternion startRotation;

    Vector3 meshSize;

    public float wallDodge, returnDistance;

    Enemy self;

    void Awake()
    {
        startRotation = transform.rotation;
        meshSize = GetComponent<MeshFilter>().mesh.bounds.size;
        
    }
    
    
    void OnEnable()
    {
        SceneManager.Instance.controller.OnChooseHardness += OnChooseHardness;
        SceneManager.Instance.controller.OnRefreshAgents += OnRefreshAgents;
    }

    void OnDisable()
    {
        SceneManager.Instance.controller.OnChooseHardness -= OnChooseHardness;
        SceneManager.Instance.controller.OnRefreshAgents -= OnRefreshAgents;
    }

    void Update()
    {
        if(SceneManager.Instance.IsGameStarted())
        {
            self.OnUpdate();
        }
        else
        {
            RB.velocity = Vector3.zero;
        }
        
    }

    void OnChooseHardness(Hardness hs)
    {
        hardness = hs;

        GameVariables variables = SceneManager.Instance.stageController.Variables;

        float moveSpeed = variables.EnemyMoveSpeed;

        if(hardness == Hardness.Hard)
        {
            self = new HardEnemy();
            moveSpeed += 100;
        }
        else
        {
            self = new EasyEnemy();
        }

        self.Inisialize(transform, moveSpeed, variables.EnemyTurnSpeed, variables.FrictionFactor, CubeMask, WallMask, wallDodge, returnDistance, Base);
    }

    void OnRefreshAgents()
    {
        transform.position = StartPosition.position;

        transform.rotation = startRotation;

        RB.velocity = Vector3.zero;
    }

    public void OnTriggerEnter(Collider other)
    {
        self.OnTriggerEnter(other);
    }

    public void OnTriggerExit(Collider other)
    {
        self.OnTriggerExit(other);
    }

    
}
