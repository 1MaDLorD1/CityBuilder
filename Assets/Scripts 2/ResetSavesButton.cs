using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class ResetSavesButton : MonoBehaviour
{
    [SerializeField] private Button _resetSavesButton;

    private void Start()
    {
        _resetSavesButton.onClick.AddListener(ResetSaves);
    }

    private void ResetSaves()
    {
        YandexGame.ResetSaveProgress();
        YandexGame.SaveProgress();
    }
}
