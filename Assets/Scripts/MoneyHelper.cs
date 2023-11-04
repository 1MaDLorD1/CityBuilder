using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyHelper
{
    private int money = 5000;

    [field: NonSerialized]
    public ResourceManager resourceManager;

    public MoneyHelper(int startMoneyAmount, ResourceManager resourceManager)
    {
        this.resourceManager = resourceManager;
        this.money = startMoneyAmount;
    }

    public int Money 
    {
        get
        {
            return money;
        }
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
            int moneyWithoutTaxes;

            if ((float)resourceManager.HappinessHelper.Happiness > -50)
            {
                moneyWithoutTaxes = (int)(structure.GetIncome() + structure.GetIncome() * resourceManager.PopulationHelper.Population / 100 + structure.GetIncome() * ((float)resourceManager.HappinessHelper.Happiness / 50));
            }
            else
            {
                moneyWithoutTaxes = (int)(structure.GetIncome() + structure.GetIncome() * resourceManager.PopulationHelper.Population / 100 + structure.GetIncome() * ((float)resourceManager.HappinessHelper.Happiness / 25));
            }

            Money += (int)(moneyWithoutTaxes + resourceManager.PopulationHelper.Population * ((float)resourceManager.TaxesManager.Taxes / 100));
        }
    }
}
