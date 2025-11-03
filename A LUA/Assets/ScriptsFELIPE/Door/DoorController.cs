using UnityEngine;
using TMPro;
using System.Linq;
public class DoorController : MonoBehaviour
{
    public int id;
    public float distanceToDetect;
    public GameObject objTextDoor;
    public bool unlocked;
    public string textLockedDoor, textOpenedDoor, textUnlockedDoor;

    private TextMeshProUGUI textDoor;
    private InventoryController inventoryController;
    private Transform player;
    private Animator anim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inventoryController = player.GetComponent<InventoryController>();
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        textDoor = objTextDoor.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {


        if(CheckProximity())
        {
            objTextDoor.SetActive(true);

            if(unlocked)
            {
                textDoor.text = textUnlockedDoor;

                if(Input.GetKeyDown(KeyCode.E))
                    OpenDoor();
            }
            else
            {
                var key = inventoryController.keys.Where(x=> x == id).FirstOrDefault();
            
                if(key != 0 )
                {
                    textDoor.text = textOpenedDoor;

                    if(Input.GetKeyDown(KeyCode.E))
                    {
                        unlocked = true;
                        OpenDoor();
                    }
                }
                else
                {   
                    textDoor.text = textLockedDoor;
                }
            }
        }
        else
        {
            objTextDoor.SetActive(false);
            
            
        }
    }

    bool CheckProximity()
    {
        return Vector3.Distance(transform.position, player.position) >= distanceToDetect;
    }

    void OpenDoor()
    {
        anim.SetTrigger("Change");
    }
    
}
