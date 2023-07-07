using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectDirection : MonoBehaviour
{
    public enum MoveDir
    {
        None,
        Up,
        Down,
        Right,
        Left,
        WhileUp,
        WhileDown,
        WhileRight,
        WhileLeft
    }
    [SerializeField] private MoveDir moveDir;
    public MoveDir GetMoveDir() { return moveDir; }
    public MoveDir SetMoveDir(MoveDir _moveDir) { return moveDir = _moveDir; }
}
