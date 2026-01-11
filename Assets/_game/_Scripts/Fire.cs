using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class Fire : MonoBehaviour
{

    public Sprite yellowFire, orangeFire, redFire, deadFire;
    public float fireHealth;
    public float maxFireHealth = 100;
    public float fireDeathRate = 1.5f;
    public bool gameStarted = false, gameover = false;
    public float fireHealthIncreaseAmount = 5;
    bool interactionPopupStatus = false;
    public SpriteRenderer firepit;

    public GameObject[] smoke;
    public ParticleSystem[] smokePs;

    public float multiplier = 1;

    public bool isPlayerNear = false;
    
    public Vector2 healthBarMaxPos, healthBarMinPos;

    public GameObject cmCam;
    
    bool isPaused = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fireHealth = maxFireHealth;
        BeginGame();
        SoundManager.instance.fireAudioSource = GetComponent<AudioSource>();
        SoundManager.instance.ToggleFire(true);
    }

    // Update is called once per frame
    void Update()
    {
        isPaused = GameManager.instance.isPaused;
        
        if (!gameStarted || gameover) return;

        if (isPaused)
        {
            foreach (var s in smokePs)
            {
                s.Pause();
            }
        }
        else
        {
            foreach (var s in smokePs)
            {
                s.Play();
            }
        }
        
        if (fireHealth <= 0)
        {
            foreach (var g in smoke)
            {
                g.SetActive(false);
            }
            firepit.sprite = deadFire;
            gameover = true;
            cmCam.SetActive(true);
            SoundManager.instance.ToggleFire(false);
            SoundManager.instance.Extinguish();
            StartCoroutine(EndGame());
        }

        if (isPlayerNear)
        {
            UIManager.instance.SetInteractPopup(true);
            interactionPopupStatus = true;
            
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (fireHealth < (maxFireHealth - fireHealthIncreaseAmount))
                {
                    int woodAmount = Inventory.instance.GetItem(1);

                    if (woodAmount > 0)
                    {
                        SoundManager.instance.DepositWood();
                        woodAmount = 1;
                        Inventory.instance.DeleteItem(1);
                    }
                    
                   
                
                    fireHealth += fireHealthIncreaseAmount * woodAmount;
                    if (fireHealth > maxFireHealth) fireHealth = maxFireHealth;
                }
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
        
        if (fireHealth <= 0)
        {
            print("Game Over");
            return;
        }

        fireHealth -= fireDeathRate * multiplier * Time.deltaTime;
        fireHealth = Mathf.Max(fireHealth, 0);
        
        
        if (fireHealth > 0)
        {
            firepit.sprite = yellowFire;
            if (fireHealth > 33.3)
            {
                firepit.sprite = orangeFire;
                if (fireHealth > 66.6)
                {
                    firepit.sprite = redFire;
                }
                
            }
            
        }
        
        
        UIManager.instance.SetFireHealthBarPos(Vector2.Lerp(healthBarMaxPos, healthBarMinPos, fireHealth / maxFireHealth));
    }

    IEnumerator EndGame()
    {
        GameManager.instance.EndGame();
        GameManager.instance.isPlaying = false;
        yield return new WaitForSeconds(3.5f);
        GameManager.instance.endScreen.gameObject.SetActive(true);
    }

    public void BeginGame()
    {
        gameStarted = true;
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
