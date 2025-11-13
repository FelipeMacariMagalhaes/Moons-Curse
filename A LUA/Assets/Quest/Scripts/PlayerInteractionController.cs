using UnityEngine;

public class PlayerInteractionController : MonoBehaviour
{
    public MonoBehaviour movementScript;  
    public Transform playerModel;         
    private Vector3 normalPos;
    private Vector3 crouchedPos;

    void Start()
    {
        if (playerModel != null)
        {
            normalPos = playerModel.localPosition;
            crouchedPos = normalPos + new Vector3(0, -0.5f, 0);
        }
    }

    public void LockPlayer()
    {
        if (movementScript != null)
            movementScript.enabled = false;

        if (playerModel != null)
            playerModel.localPosition = crouchedPos;
    }

    public void UnlockPlayer()
    {
        if (movementScript != null)
            movementScript.enabled = true;

        if (playerModel != null)
            playerModel.localPosition = normalPos;
    }
}
