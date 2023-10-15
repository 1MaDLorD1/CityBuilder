using IJunior.TypedScenes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuButton : MonoBehaviour
{
    [SerializeField] private Button[] mainMenuBtn;
    [SerializeField] private Button[] againBtn;

    void Start()
    {
        for (int i = 0; i < mainMenuBtn.Length; i++)
        {
            mainMenuBtn[i].onClick.AddListener(OnMainMenuHandler);
            againBtn[i].onClick.AddListener(OnAgainHandler);
        }
    }

    private void OnMainMenuHandler()
    {
        AudioManager.Instance.PlayButtonClickedSound();
        MainMenu.Load();
    }

    private void OnAgainHandler()
    {
        AudioManager.Instance.PlayButtonClickedSound();
        BuildingMechanicPrototype.Load();
    }
}
