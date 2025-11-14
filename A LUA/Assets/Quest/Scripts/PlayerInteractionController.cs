using UnityEngine;

public class PlayerInteractionController : MonoBehaviour
{
    public MonoBehaviour movementScript;
    public Transform playerModel;

    private Vector3 normalPos;
    private Vector3 crouchedPos;

    void Start()
    {
        normalPos = playerModel.localPosition;
        crouchedPos = normalPos + new Vector3(0, -0.5f, 0);
    }

    public void LockPlayer()
    {
        movementScript.enabled = false;
        playerModel.localPosition = crouchedPos;
    }

    public void UnlockPlayer()
    {
        movementScript.enabled = true;
        playerModel.localPosition = normalPos;
    }
}
