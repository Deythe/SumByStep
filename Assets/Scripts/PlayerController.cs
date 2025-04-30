using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    public void Move(InputAction.CallbackContext context)
    {
        Vector2 roundDirection = new Vector2(Mathf.RoundToInt(context.ReadValue<Vector2>().x), Mathf.RoundToInt(context.ReadValue<Vector2>().y));
        //Debug.Log(context.ReadValue<Vector2>());
        //actionsBehaviour.ExecuteAllActions(roundDirection);
    }

    public void CancelActions(InputAction.CallbackContext context)
    {
        //actionsBehaviour.UndoLastAction();
    }
}
