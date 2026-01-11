using System;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour 
{
    public static UIManager instance;

    public Image sticksTens, sticksOnes;
    public Sprite[] numbers;

    public Image fireHealthBar;

    public GameObject interactPopup;

    public Transform clockHand;
    public GameObject clock;

    public Transform fire, compassPointer, player;

    public Image rainOverlay;

    private void Start() { instance = this; }


    private void Update()
    {
        Vector2 direction = fire.position - player.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (angle < 0)
            angle += 360f;
        
        compassPointer.rotation = Quaternion.Euler(0, 0, angle + 270);

    }

    public void SetInteractPopup(bool value)
    { 
        interactPopup.SetActive(value);
    }

    public void SetStickAmount(int amount)
    {
        if (amount < 0)
        {
            amount = 0;
        }

        if (amount > 99)
        {
            amount = 99;
        }

        if (amount < 10)
        {
            sticksOnes.sprite = numbers[amount];
            sticksTens.sprite = numbers[0];
        }
        else
        {
            sticksTens.sprite = numbers[amount / 10];
            sticksOnes.sprite = numbers[amount - ((amount / 10) * 10)];
        }
    }

    public void SetClockHandRotation(float rotation)
    {
        clockHand.localRotation = Quaternion.Euler(0, 0, rotation);
    }

    public void SetFireHealthBarPos(Vector2 pos)
    {
        fireHealthBar.rectTransform.anchoredPosition = pos;
    }
}
