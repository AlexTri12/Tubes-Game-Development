using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    private void Awake()
    {
        panelMainMenu.SetActive(true);
        panelSettings.SetActive(false);

        AddListeners();
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

        }
    }

    void HightlightMainMenuButton()
    {
        int index = 0;
        foreach (Transform child in panelMainMenu.transform)
        {
            if (index == indexButton)
                SelectButton(child.gameObject);
            else
                DeselectButton(child.gameObject);

            index++;
            if (index >= maxMainMenuButton)
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
                break;
            case 3:
                // Exit
                break;
            default:
                // Do nothing
                break;
        }
    }
}
