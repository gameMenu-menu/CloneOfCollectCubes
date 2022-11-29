using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BehaviourState {WaitingToStart, FindingCubes, GoingToCubes, ReturningToBase}
public abstract class Enemy
{
    int nLayerNonCarryingCube;
    int nLayerCarryingCube;

    float MoveSpeed, TurnSpeed, FrictionFactor;

    Rigidbody RB;
    BehaviourState CurrentBehaviour;

    protected Vector3 CubeSpot;

    protected LayerMask CubeMask, WallMask;

    protected Vector3 meshSize;

    float WallDodge, ReturnDistance;

    Transform Base;

    protected Transform Self;

    IEnumerator GrabRoutine;

    float checkTime;
    Vector3 checkVector;


    public void Inisialize(Transform self, float moveSpeed, float turnSpeed, float frictionFactor, LayerMask cubeMask, LayerMask wallMask, float wallDodge, float returnDistance, Transform _base)
    {
        Self = self;
        MoveSpeed = moveSpeed;
        TurnSpeed = turnSpeed;
        FrictionFactor = frictionFactor;
        CubeMask = cubeMask;
        WallMask = wallMask;
        WallDodge = wallDodge;
        ReturnDistance = returnDistance;
        Base = _base;

        RB = Self.GetComponent<Rigidbody>();

        meshSize = Self.GetComponent<MeshFilter>().mesh.bounds.size;

        nLayerNonCarryingCube = LayerMask.NameToLayer(EnemyController.stringLayerNonCarryingCube);
        nLayerCarryingCube = LayerMask.NameToLayer(EnemyController.stringLayerCarryingCube);
    }
    public void OnUpdate()
    {

        Friction();

        if(!SceneManager.Instance.IsGameStarted())
        {
            return;
        }

        Behave();

       
    }

    protected virtual void Behave()
    {
        switch(CurrentBehaviour)
        {
            case BehaviourState.WaitingToStart:
            {
                CurrentBehaviour = BehaviourState.FindingCubes;
                break;
            }
            case BehaviourState.FindingCubes:
            {
                if(FindSpot())
                {
                    CurrentBehaviour = BehaviourState.GoingToCubes;
                }
                break;
            }
            case BehaviourState.GoingToCubes:
            {
                Move(CubeSpot);
                Turn(CubeSpot);
                float dist = (Self.position - CubeSpot).sqrMagnitude;
                float maxDistance = (meshSize / 2f).sqrMagnitude * ReturnDistance;
                if( dist < maxDistance || (dist < maxDistance * WallDodge && !Movable()) )
                {
                    CurrentBehaviour = BehaviourState.ReturningToBase;
                }

                if(checkTime > Time.time)
                {
                    if( (checkVector - Self.position).sqrMagnitude < 1f )
                    {
                        CurrentBehaviour = BehaviourState.FindingCubes;
                    }
                    checkVector = Self.position;
                    checkTime = Time.time + 0.5f;
                }
                break;
            }
            case BehaviourState.ReturningToBase:
            {
                Move(Base.position);
                Turn(Base.position);
                if( (Self.position - Base.position).sqrMagnitude < ( meshSize / 3f ).sqrMagnitude )
                {
                    CurrentBehaviour = BehaviourState.FindingCubes;
                }
                break;
            }
        }
    }

    bool Movable()
    {
        Collider[] colls = Physics.OverlapSphere(Self.position + Self.forward * meshSize.z / 2f, 2.5f, WallMask);
        if(colls.Length > 0)
        {
            return false;
        }
        else return true;
    }

    protected virtual bool FindSpot()
    {
        Transform cube = SceneManager.Instance.stageController.GetCube();

        Vector3 pos = cube.position;
        pos.y = Self.position.y;

        CubeSpot = pos;

        return true;
    }

    void Friction()
    {
        RB.velocity = Vector3.Lerp(RB.velocity, Vector3.zero, Time.deltaTime * FrictionFactor );
    }

    void Move(Vector3 target)
    {
        Vector3 dir = target - Self.position;

        dir = dir.normalized;


        
        RB.velocity += dir * Time.deltaTime * MoveSpeed;
    }

    void Turn(Vector3 target)
    {
        
        Vector3 dir = target - Self.position;

        Quaternion to = Quaternion.LookRotation(dir);

        Self.rotation = Quaternion.Lerp(Self.rotation, to, TurnSpeed * Time.deltaTime);

    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer.Equals(nLayerNonCarryingCube))
        {
            other.gameObject.layer = nLayerCarryingCube;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if(other.gameObject.layer.Equals(nLayerCarryingCube))
        {
            other.gameObject.layer = nLayerNonCarryingCube;
        }
    }
}
