using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FlagGoal{

    private const int modifier = 3;
    
    public const int blueRequiredAmount =  100;
    public const int redRequiredAmount =  -100;

    public int itemsRequired;
    
    public int currentAmount;

    public void RedCapture(){
        currentAmount -= modifier;
        if (currentAmount <= -100)
            currentAmount = -100;
    }

    public void BlueCapture(){
        currentAmount += modifier;
        if (currentAmount >= 100)
            currentAmount = 100;
    }

    public teamType WhoReached(){
        if (currentAmount >= blueRequiredAmount)
            return teamType.Blue;
        else if (currentAmount <= redRequiredAmount)
            return teamType.Red;
        else
            return teamType.None;
    }
}

public enum teamType{
    Blue,
    Red,
    None
}
