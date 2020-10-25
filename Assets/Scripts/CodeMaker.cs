using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CodeButton
{
    public Button CodeBtn;
    public string Code;
}

public class CodeMaker : MonoBehaviour
{
    public static CodeMaker Instance;

    [SerializeField]
    public CodeButton[] CodeButtons;

    public string Code {
        get => code;
        private set {
            code = value;
            OnCodeChange?.Invoke(code);
        }
    }

    public delegate void CodeChangeHandler(string code);
    public event CodeChangeHandler OnCodeChange;

    private List<ICodeObserver> codeObservers;

    private string code;

    private void Awake()
    {
        Instance = this;
        codeObservers = new List<ICodeObserver>();
    }

    void Start()
    {
        foreach(CodeButton codeBtn in CodeButtons)
            codeBtn.CodeBtn.onClick.AddListener(() => AddToCode(codeBtn.Code));
    }

    private void AddToCode(string code)
    {
        Code += code;

        foreach (ICodeObserver ob in codeObservers)
        {
            ob.Send(code);
            ob.UpdateCode(Code);
        }
    }

    public void ClearCode()
    {
        Code = "";

        foreach (ICodeObserver ob in codeObservers) ob.Clear();
    }

    public void Subscribe(ICodeObserver observer)
    {
        codeObservers.Add(observer);
    }
}
