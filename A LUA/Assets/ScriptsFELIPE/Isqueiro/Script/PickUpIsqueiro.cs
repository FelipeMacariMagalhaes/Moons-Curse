using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class PickUpIsqueiro : MonoBehaviour
{
    public GameObject lighterInHand; // isqueiro na mão
    public AudioClip pickupSound;
    private AudioSource audioSource;
    private bool pickedUp = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerStay(Collider other)
    {
        if (pickedUp) return;

        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            PickUp(other.gameObject);
        }
    }

    void PickUp(GameObject player)
    {
        pickedUp = true;

        if (pickupSound != null && audioSource != null)
            audioSource.PlayOneShot(pickupSound);

        // não ativar o isqueiro imediatamente
        if (lighterInHand != null)
            lighterInHand.SetActive(false);

        ItemSwitch switcher = player.GetComponentInChildren<ItemSwitch>();
        if (switcher != null)
        {
            switcher.GiveLighter(); // só marca que o player tem o isqueiro
        }

        gameObject.SetActive(false); // some o item do chão
    }
}