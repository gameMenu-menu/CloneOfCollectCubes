using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public const string stringLayerNonCarryingCube = "Collectible";
    public const string stringLayerCarryingCube = "PlayerCube";

    int nLayerNonCarryingCube;
    int nLayerCarryingCube;
    float MoveSpeed, TurnSpeed, FrictionFactor, InputScaler;

    public Rigidbody RB;

    public Vector3 mouseStartPosition, resultPos, resultPosForRotation, playerPos, playerPosForRotation, mouseStartPositionForRotation;

    bool inputExists;

    float clickTime = 0;

    Vector3 oldPos = Vector3.zero;
    Vector3 newPos = Vector3.zero;

    public Transform StartPosition;

    void Start()
    {
        nLayerNonCarryingCube = LayerMask.NameToLayer(stringLayerNonCarryingCube);
        nLayerCarryingCube = LayerMask.NameToLayer(stringLayerCarryingCube);
    }
    
    
    void OnEnable()
    {
        SceneManager.Instance.controller.OnPrepareScene += OnPrepareScene;
        SceneManager.Instance.controller.OnRefreshAgents += OnRefreshAgents;
    }

    void OnDisable()
    {
        SceneManager.Instance.controller.OnPrepareScene -= OnPrepareScene;
        SceneManager.Instance.controller.OnRefreshAgents -= OnRefreshAgents;
    }

    void Update()
    {

        Friction();

        if(SceneManager.Instance.UIChecker.IsPointerOverUIElement())
        {
            return;
        }
        
        
        CalculateInput();

        if(inputExists)
        {
            Turn();
            Move();
            
        }

        

        bool draggingMouse = false;

        if(Input.GetMouseButton(0))
        {
            newPos = Input.mousePosition;

            if( (newPos - oldPos).sqrMagnitude > 50f )
            {
                oldPos = Input.mousePosition;
                draggingMouse = true;
            }

        }

        if(draggingMouse)
        {
            resultPosForRotation = Input.mousePosition - mouseStartPositionForRotation;

            if(!SceneManager.Instance.IsGameStarted())
            {
                SceneManager.Instance.controller.StartGame();
            }
        }

       
    }

    void Friction()
    {
        RB.velocity = Vector3.Lerp(RB.velocity, Vector3.zero, Time.deltaTime * FrictionFactor );
    }

    void CalculateInput()
    {
        if(Input.GetMouseButton(0) && clickTime < Time.time)
        {
            if( (mouseStartPositionForRotation - Input.mousePosition).sqrMagnitude > 0.4f )
            {
                mouseStartPositionForRotation = Input.mousePosition;
                clickTime = Time.time + 0.15f;
            }
            playerPosForRotation = transform.position;

            
        }

        if(Input.GetMouseButtonDown(0))
        {
            mouseStartPosition = Input.mousePosition;
            playerPos = transform.position;
        }
        else if(Input.GetMouseButton(0))
        {
            Vector3 mousePos = Input.mousePosition;
            inputExists = true;

            resultPos = (mousePos - mouseStartPosition);
            

            resultPos.z = resultPos.y;
            resultPos.y = transform.position.y;

            resultPos *= InputScaler;
        }
        else
        {
            inputExists = false;
        }
    }

    void Move()
    {
        Vector3 dir = resultPos - transform.position;

        dir = playerPos + resultPos;

        dir = dir - transform.position;

        if(dir.sqrMagnitude < 5f) return;

        dir = dir.normalized;


        
        RB.velocity += dir * Time.deltaTime * MoveSpeed;
    }

    void Turn()
    {
        
        Vector3 dir = resultPosForRotation - transform.position;

        dir = resultPosForRotation;

        dir.z = dir.y;
        dir.y = 0;

        if(dir == Vector3.zero) return;

        Quaternion to = Quaternion.LookRotation(dir);

        transform.rotation = Quaternion.Lerp(transform.rotation, to, TurnSpeed * Time.deltaTime);

    }

    void OnPrepareScene()
    {
        GameVariables variables = SceneManager.Instance.stageController.Variables;
        MoveSpeed = variables.MoveSpeed;
        TurnSpeed = variables.TurnSpeed;
        FrictionFactor = variables.FrictionFactor;

        InputScaler = variables.InputScaler;
    }

    void OnRefreshAgents()
    {
        transform.position = StartPosition.position;

        transform.rotation = Quaternion.identity;

        RB.velocity = Vector3.zero;
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
