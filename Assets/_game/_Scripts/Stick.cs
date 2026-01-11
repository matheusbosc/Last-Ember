using UnityEngine;

public class Stick : MonoBehaviour
{
    
    bool isPlayerNear = false;

    private bool interactionPopupStatus = false;
    

    // Update is called once per frame
    void Update()
    {
        bool isPaused = GameManager.instance.isPaused;

        if (isPaused) return;
        
        if (isPlayerNear)
        {
            UIManager.instance.SetInteractPopup(true);
            interactionPopupStatus = true;
            
            if (Input.GetKeyDown(KeyCode.E))
            {
                Inventory.instance.AddItem(1);
                ResourceManager.instance.RemoveStick();
                UIManager.instance.SetInteractPopup(false);
                SoundManager.instance.PickupItem();
                Destroy(gameObject);
            }
        }
        else
        {
            if (interactionPopupStatus)
            {
                UIManager.instance.SetInteractPopup(false);
                interactionPopupStatus = false;
            }
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) isPlayerNear = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if  (other.CompareTag("Player")) isPlayerNear = false;
    }
}
