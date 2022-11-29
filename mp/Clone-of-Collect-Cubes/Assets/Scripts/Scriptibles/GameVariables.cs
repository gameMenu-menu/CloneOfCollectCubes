using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameVariables")]
public class GameVariables : ScriptableObject
{
    public float MoveSpeed, TurnSpeed, FrictionFactor, InputScaler;

    public float EnemyMoveSpeed, EnemyTurnSpeed;

    public Texture2D[] PixelMaps;

    public int PixelSizeX, PixelSizeY, XLimit, YLimit, SpaceBetweenCubes;
}
