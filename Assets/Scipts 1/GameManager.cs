using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static UnityEvent MovesLeftDecrease = new UnityEvent();
    public static UnityEvent HexogonLeftCount = new UnityEvent();
    public static UnityEvent<string> SoundEffects=new UnityEvent<string>();


    [SerializeField] private int initialMovesLeft = 15; // Initial value for moves left
    [SerializeField] private Text movesLeftText;
    [SerializeField] private GameObject lossPanel;
    [SerializeField] private GameObject winPanel;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip clickSound;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip winClip;
    [SerializeField] private AudioClip lossClip;

    private int movesLeft;
    private int hexogonCount;

    private void Awake()
    {
        ResetValues();
        lossPanel.SetActive(false);
    }

    private void ResetValues()
    {
        lossPanel.SetActive(false);
        movesLeft = initialMovesLeft;
        movesLeftText.text = movesLeft.ToString() + " Moves";
        movesLeftText.color = Color.black; // Reset color to default
        movesLeftText.gameObject.SetActive(true);
        
        winPanel.SetActive(false);
        Time.timeScale = 1;
    }

    private void OnEnable()
    {
        MovesLeftDecrease.AddListener(MovesLeft);
        HexogonLeftCount.AddListener(HexogonCount);
        SoundEffects.AddListener(SoundManager);
    }

    private void Start()
    {
        lossPanel.SetActive(false);
        audioSource =GetComponent<AudioSource>();
        GameObject[] hexogonsLeft = GameObject.FindGameObjectsWithTag("Hexagon");
        hexogonCount = hexogonsLeft.Length;
        Debug.Log("HexagonCount = " + hexogonCount);
    }
    private void LateUpdate()
    {
        if(movesLeft<1 && hexogonCount > 1)
        {
            StartCoroutine(CallingLossPanel()); 
        }
    }
    IEnumerator CallingLossPanel()
    {
        yield return new WaitForSeconds(1f);
        NoMovesLeft();

    }
    private void OnDisable()
    {
        MovesLeftDecrease.RemoveListener(MovesLeft);
        HexogonLeftCount.RemoveListener(HexogonCount);
        SoundEffects.RemoveListener(SoundManager);
    }

    public void MovesLeft()
    {
        if (movesLeft > 0)
        {
            movesLeft--;
            if (movesLeft < 6)
            {
                movesLeftText.color = Color.red;
            }
            movesLeftText.text = movesLeft.ToString() + " Moves";
        }
        else
        {
            NoMovesLeft();
        }
    }

    public void NoMovesLeft()
    {
        SoundManager("loss");
        Time.timeScale = 0;
        lossPanel.SetActive(true);
        movesLeftText.gameObject.SetActive(false);
    }

    public void HexogonCount()
    {
        hexogonCount--;
        if (hexogonCount == 0)
        {
            StartCoroutine(GameWon());
        }
    }

    private IEnumerator GameWon()
    {
        yield return new WaitForSeconds(1.5f);
        SoundManager("win");
        winPanel.SetActive(true);
        Time.timeScale = 0;
        movesLeftText.gameObject.SetActive(false);
    }

    void SoundManager(string effect)
    {
        if (effect == "click")
        {
            audioSource.PlayOneShot(clickSound);
        }
        else if (effect=="collision")
        {
            audioSource.Stop();
        }
        else if (effect == "win")
        {
            audioSource.PlayOneShot(winClip);
        }
        else if(effect == "loss")
        {
            audioSource.PlayOneShot(lossClip);
        }
       
    }
}
