using System;
using UnityEngine;

public class BarrelItem : Interactable
{
    public int barrelID;
    private void Awake()
    {
        barrelID = GetComponent<P2BarrelClick>().barrelID;
    }
    protected override string DefaultMessage => "Press E to knock on barrel " + barrelID;
}
