using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MainMenuController : MonoBehaviour
{
    public GameObject panelMainMenu;
    public GameObject panelSettings;
    bool isSettings
    {
        get { return panelSettings.activeSelf; }
    }
    int indexButton = 0;
    int maxMainMenuButton = 4;
    int maxSettingsRow = 3;
    Slider[] sliders;

    private void Start()
    {
        AddListeners();
        sliders = panelSettings.GetComponentsInChildren<Slider>();
        SetInitialVolume();

        panelMainMenu.SetActive(true);
        panelSettings.SetActive(false);

        HightlightMainMenuButton();
    }

    private void OnDestroy()
    {
        RemoveListeners();
    }

    void AddListeners()
    {
        InputController.moveEvent += OnMove;
        InputController.fireEvent += OnFire;
    }

    void RemoveListeners()
    {
        InputController.moveEvent -= OnMove;
        InputController.fireEvent -= OnFire;
    }

    void OnFire(object sender, InfoEventArgs<int> e)
    {
        if (e.info == 0)
        {
            if (!isSettings)
            {
                ClickMainMenuButton();
            }
            else
            {
                ClickSettingsButton();
            }
        }
    }

    void OnMove(object sender, InfoEventArgs<Point> e)
    {
        if (!isSettings)
        {
            if (e.info.x > 0)
                indexButton++;
            else if (e.info.x < 0)
                indexButton--;

            if (indexButton >= maxMainMenuButton)
                indexButton = 0;
            if (indexButton < 0)
                indexButton = maxMainMenuButton - 1;

            HightlightMainMenuButton();
        }
        else
        {
            if (e.info.y != 0)
            {
                if (e.info.y > 0)
                    indexButton--;
                else if (e.info.y < 0)
                    indexButton++;

                if (indexButton >= maxSettingsRow)
                    indexButton = 0;
                if (indexButton < 0)
                    indexButton = maxSettingsRow - 1;

                HightlightSettingsRow();
            }
            else
            {
                if (indexButton < 2)
                {
                    if (e.info.x > 0)
                        sliders[indexButton].value++;
                    else
                        sliders[indexButton].value--;
                }
            }
        }
    }

    void HightlightMainMenuButton()
    {
        HightlightButton(panelMainMenu.transform.GetChild(0).transform, maxMainMenuButton);
    }

    void HightlightSettingsRow()
    {
        HightlightButton(panelSettings.transform.GetChild(0).transform, maxSettingsRow);
    }

    void HightlightButton(Transform parent, int maxIndex)
    {
        int index = 0;
        foreach (Transform child in parent)
        {
            if (index == indexButton)
                SelectButton(child.gameObject);
            else
                DeselectButton(child.gameObject);

            index++;
            if (index >= maxIndex)
                break;
        }
    }

    void SelectButton(GameObject obj)
    {
        obj.GetComponent<Outline>().effectColor = new Color32(65, 14, 14, 128);
        obj.GetComponentsInChildren<Outline>()[1].effectColor = new Color32(255, 255, 255, 128);
    }
    
    void DeselectButton(GameObject obj)
    {
        obj.GetComponent<Outline>().effectColor = new Color32(147, 48, 48, 128);
        obj.GetComponentsInChildren<Outline>()[1].effectColor = new Color32(0, 0, 0, 128);
    }

    void ClickMainMenuButton()
    {
        switch (indexButton)
        {
            case 0:
                // Start New Game
                SceneManager.LoadScene(1);
                break;
            case 1:
                // Load Game
                break;
            case 2:
                // Settings
                panelMainMenu.SetActive(false);
                panelSettings.SetActive(true);
                indexButton = 0;
                HightlightSettingsRow();
                break;
            case 3:
                // Exit
                #if UNITY_EDITOR
                    EditorApplication.ExitPlaymode();
                #else
                    Application.Quit();
                #endif
                break;
            default:
                // Do nothing
                break;
        }
    }

    void ClickSettingsButton()
    {
        switch (indexButton)
        {
            case 0:
                // Volume BGM
                break;
            case 1:
                // Volume SFX
                break;
            case 2:
                // Save and Close
                panelMainMenu.SetActive(true);
                panelSettings.SetActive(false);
                indexButton = 0;
                VolumeData.INSTANCE.SaveVolumeSettings();
                HightlightMainMenuButton();
                break;
            default:
                // Do nothing
                break;
        }
    }

    // For slider
    public void OnSliderUpdate()
    {
        int value = Mathf.FloorToInt(sliders[indexButton].value);
        sliders[indexButton].GetComponentInChildren<Text>().text = value.ToString();
        if (indexButton == 0)
            VolumeData.INSTANCE.bgmVolume = value;
        else
            VolumeData.INSTANCE.sfxVolume = value;
    }

    void SetInitialVolume()
    {
        sliders[0].value = VolumeData.INSTANCE.bgmVolume;
        sliders[0].GetComponentInChildren<Text>().text = VolumeData.INSTANCE.bgmVolume.ToString();
        sliders[1].value = VolumeData.INSTANCE.sfxVolume;
        sliders[1].GetComponentInChildren<Text>().text = VolumeData.INSTANCE.sfxVolume.ToString();
    }
}
