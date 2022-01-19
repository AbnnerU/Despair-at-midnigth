using UnityEngine;

public interface IMovement: IMove,IJump,IRun
{
   
    
}

public interface IMove
{
    void InputController_OnMove(Vector2 inputValue);

    void BackSpeedToNormal();

    void SetNewSpeed(float newSpeed);
}

public interface IJump
{
    void InputController_OnJump();

    void CanJump(bool canJump);
}

public interface IRun
{
    void InputController_OnRun(float inputValue);

    void CanRun(bool canRun);
}

