using System;
using System.Collections;
using System.Collections.Generic;
using PlayableNodes;
using UnityEngine;

public class Runner : MonoBehaviour
{
    public TrackPlayerCollection _trackPlayerCollection;

    private void Awake()
    {
        OpenAnimation();
    }

    public void OpenAnimation()
    {
        _trackPlayerCollection.PlayAsync("Open");
    }
    
    public void CloseAnimation()
    {
        _trackPlayerCollection.PlayAsync("Close");
    }
}
