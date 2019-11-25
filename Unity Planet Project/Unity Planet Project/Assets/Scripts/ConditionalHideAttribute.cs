using UnityEngine;
using System;
using System.Collections;

//Original version of the ConditionalHideAttribute created by Brecht Lecluyse (www.brechtos.com)
//Modified by: Sebastian Lague and Dimitrios Tsolis

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property |
    AttributeTargets.Class | AttributeTargets.Struct, Inherited = true)]
public class ConditionalHideAttribute : PropertyAttribute
{
    public string shownSettingsIfSelected;
    public int selectedSettingNumber;

    //The follwoing two methods were implemented to show certain attributes in the inspector when certain aspects are changes.
    //For example, if simple Noise is chosen for a noise layer, only the simple noise settings will be shown instead of both the rigid and simple noise settings

    public ConditionalHideAttribute(string boolVariableName)
    {
        shownSettingsIfSelected = boolVariableName;  
    }

    public ConditionalHideAttribute(string enumVariableName, int enumIndex)
    {
        shownSettingsIfSelected = enumVariableName;
        this.selectedSettingNumber = enumIndex;
    }

}



