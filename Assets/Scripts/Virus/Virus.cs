using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Virus : Malware
{
    private void Start()
    {
        Type = EMalwareType.Virus;
    }

    public override void Skill()
    {
        
    }
}
