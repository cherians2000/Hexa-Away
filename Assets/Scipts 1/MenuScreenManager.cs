using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScreenManager : MonoBehaviour
{
    public Slider slider;
    public GameObject MenuButtons;
    [SerializeField] private float changeDuration = 5f;
    private void Awake()
    {
       
    }
    void Start()
    {
        if(slider != null)
        {
            slider.maxValue = 100;
            slider.minValue = 0;

            StartCoroutine(SliderValueChange());
        }
       
    }

    IEnumerator SliderValueChange()
    {
        float elapsedTime = 0f;

        while (elapsedTime < changeDuration)
        {
            elapsedTime += Time.deltaTime;
            slider.value = Mathf.Lerp(0, 100, elapsedTime / changeDuration);
            yield return null; // Wait for the next frame
        }

        // Ensure the slider reaches the max value
        slider.value = 100;
        // Deactivate the slider and activate the menu buttons
        slider.gameObject.SetActive(false);
        MenuButtons.SetActive(true);
    }
    public void Level1()
    {
        SceneManager.LoadScene("Level 1");
    }
    public void Level2()
    {
        SceneManager.LoadScene("Level 2");
    }
    public void Menu()
    {
        SceneManager.LoadScene("Menu");
    }
    public void OnApplicationQuit()
    {
        Application.Quit();
    }
}
