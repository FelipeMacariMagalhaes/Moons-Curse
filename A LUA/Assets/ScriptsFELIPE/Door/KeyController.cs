using UnityEngine;
using TMPro;

public class KeyController : MonoBehaviour
{

    public int id;
    public float distanceToDetect;
    public GameObject objTextKey;
    public string textKey;



    private Transform player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(transform.position, player.position) <= distanceToDetect)
        {
            objTextKey.SetActive(true);
            objTextKey.GetComponent <TextMeshProUGUI>(). text = textKey;

            if(Input.GetKeyDown(KeyCode.E))
            {   
                player.GetComponent<InventoryController>().AddItem(id);
                objTextKey.SetActive(false);
                Destroy(gameObject);
            }
        }
        else
        {
            objTextKey.SetActive(false);
        }
    }
}
