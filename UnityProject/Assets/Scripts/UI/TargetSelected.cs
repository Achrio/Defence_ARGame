using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetSelected : MonoBehaviour
{
    Text thisText;
    string thisString;
    public PluginActivity plugin;

    public void targetSelected() {
        thisText = this.GetComponent<Text>();
        thisString = thisText.text;

        string[] target = thisString.Split(' ');
        plugin.getTarget(target[0]);
    }
}
