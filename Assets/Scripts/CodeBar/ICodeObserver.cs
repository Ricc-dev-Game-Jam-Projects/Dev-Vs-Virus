public interface ICodeObserver
{
    void UpdateCode(string code);
    void Send(string code);
    void Clear();
}
