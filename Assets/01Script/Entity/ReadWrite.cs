public class ReadWrite : InteractionObject
{
    private void Start()
    {
        OnClick += UIManager.Instance.OpenReadWrite;
    }
}
