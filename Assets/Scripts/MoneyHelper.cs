using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyHelper
{
    private int money;

    private ResourceManager _resourceManager;

    public MoneyHelper(int startMoneyAmount, ResourceManager resourceManager)
    {
        _resourceManager = resourceManager;
        this.money = startMoneyAmount;
    }

    public int Money { get => money; 
        set 
        { 
            if(value < 0)
            {
                money = 0;
                throw new MoneyException("Not enough money");
            }
            else
            {
                money = value;
            }
            
        } 
    }

    public void ReduceMoney(int amount)
    {
        Money -= amount;
    }

    public void AddMoney(int amount)
    {
        Money += amount;
    }

    public void CalculateMoney(IEnumerable<StructureBaseSO> buildings)
    {
        CollectIncome(buildings);
        ReduceUpkeep(buildings);
    }

    private void ReduceUpkeep(IEnumerable<StructureBaseSO> buildings)
    {
        foreach (var structure in buildings)
        {
            Money -= structure.upkeepCost;
        }
    }

    private void CollectIncome(IEnumerable<StructureBaseSO> buildings)
    {
        foreach (var structure in buildings)
        {
            int moneyWithoutTaxes = (int)(structure.GetIncome() + structure.GetIncome() * ( (float)_resourceManager.HappinessHelper.Happiness / 50 ));
            Money += (int)(moneyWithoutTaxes + _resourceManager.PopulationHelper.Population * ((float)_resourceManager.TaxesManager.Taxes / 100));
        }
    }
}
