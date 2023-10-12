using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TaxesManager : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private TextMeshProUGUI _taxesValue;
    [SerializeField] private Image _fillArea;

    private int taxes = 0;

    public int Taxes { get => taxes; }

    private void Update()
    {
        if (taxes != (int)_slider.value)
        {
            taxes = (int)_slider.value;
            _taxesValue.text = taxes + "%";
        }
    }
}
