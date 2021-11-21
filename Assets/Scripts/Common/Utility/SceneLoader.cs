using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public Canvas canvas;
    public Slider slider;
    AsyncOperation operation = null;

    private void Awake()
    {
        canvas.gameObject.SetActive(false);
    }

    public void StartLoadScene(int sceneToLoad)
    {
        canvas.gameObject.SetActive(true);
        operation = SceneManager.LoadSceneAsync(sceneToLoad);
    }

    private void Update()
    {
        if (operation != null)
        {
            float progressValue = Mathf.Clamp01(operation.progress / 0.9f);
            slider.value = progressValue;
        }
    }
}
