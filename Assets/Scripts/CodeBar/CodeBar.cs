using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CodeBar : MonoBehaviour, ICodeObserver
{
    public bool SendCode;

    public Queue<string> codes;

    public TextMeshProUGUI[] BlockCodes;

    private List<ICodeBarObserver> observers;

    private void Awake()
    {
        observers = new List<ICodeBarObserver>();
    }

    void Start()
    {
        SendCode = false;

        CodeMaker.Instance.Subscribe(this);

        codes = new Queue<string>();
    }

    public void Send(string code)
    {
        if (codes.Count >= BlockCodes.Length)
        {
            codes.Dequeue();
        }
        codes.Enqueue(code);
        UpdateCodeBlocks();
    }

    public void UpdateCode(string code) {}

    public void UpdateCodeBlocks()
    {
        int i = 0;
        foreach(string code in codes)
        {
            BlockCodes[i].text = code;
            i++;
        }
    }

    public void Clear()
    {
        codes.Clear();
        foreach(TextMeshProUGUI codeText in BlockCodes)
        {
            codeText.text = "";
        }
    }

    public void OnBarReleased()
    {
        SendCode = true;
    }

    public void OnBarGrabbed()
    {
        SendCode = false;
    }

    public void Subscribe(ICodeBarObserver ob)
    {
        observers.Add(ob);
    }

    protected void OnTriggerExit2D(Collider2D collision)
    {
        if (SendCode && collision.gameObject.GetComponent<MalwareDirection>() != null)
        {
            EMalwareType type = collision.gameObject.GetComponent<MalwareDirection>().type;
            SendCode = false;
            Debug.Log($"Collided with {type}");
            switch(type)
            {
                case EMalwareType.Discard:
                    CodeMaker.Instance.ClearCode();
                    return;
            }

            Notify(CodeMaker.Instance.Code, type);
        }
    }

    private void Notify(string code, EMalwareType type)
    {
        foreach(ICodeBarObserver observer in observers)
        {
            observer.Send(code, type);
            print($"Send code: '{code}' to destroy {type}");
        }
    }

}
