using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;

public class HappinessHelper
{
    [field: NonSerialized]
    public ResourceManager _resourceManager;

    private int _happiness = 0;

    public HappinessHelper(ResourceManager resourceManager)
    {
        _resourceManager = resourceManager;
    }

    public int Happiness
    {
        get { return _happiness; }
        set { _happiness = value; }
    }

    public void CalculateHappiness(IEnumerable<StructureBaseSO> buildings)
    {
        foreach (var structure in buildings)
        {
            if (structure.requireRoadAccess == true && structure.requirePower == true && structure.requireWater == true)
            {
                var power = structure.HasPower();
                var water = structure.HasWater();
                var road = structure.HasRoadAccess();
                if (power && water && road)
                {
                    Happiness += (1 - _resourceManager.TaxesManager.Taxes / 50);
                }
                else if (water && road || water && power || power && road)
                {
                    Happiness -= _resourceManager.TaxesManager.Taxes / 50;
                }
                else if (water || power || road)
                {
                    Happiness -= (1 + _resourceManager.TaxesManager.Taxes / 50);
                }
                else
                {
                    Happiness -= (2 + _resourceManager.TaxesManager.Taxes / 50);
                }
            }
        }
    }
}
