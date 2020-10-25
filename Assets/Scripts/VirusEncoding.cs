using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusEncoding : MonoBehaviour, ICodeBarObserver
{
    public PrefabPool VirusPool;

    public GameObject[] VirusPrefabs;

    public Transform Anchor;

    private CodeBar codeBar;

    private void Start()
    {
        VirusPool = new PrefabPool();


        codeBar = FindObjectOfType<CodeBar>();

        codeBar.Subscribe(this);

        Attack();
    }

    public void Attack()
    {
        GameObject virus = VirusPool.GetPrefabFromPool(VirusPrefabs[Random.Range(0, VirusPrefabs.Length)]);

        virus.transform.position = Anchor.position;
    }

    public void Send(string code, EMalwareType type)
    {
        print($"Received code: '{code}' to destroy {type}");
        GameObject objFound = null;
        VirusPool.IteratePool((obj) =>
        {
            Malware malware = obj.GetComponent<Malware>();
            if (malware.Type == type && code == malware.Decode)
            {
                objFound = obj;
            }
        });

        if(objFound != null)
        {
            objFound.SetActive(false);
            print($"Got it! Destroying this annoying virus of type: {type}");
        } else
        {
            print($"Couldn't find any {type} with code: '{code}'");
        }
    }
}
