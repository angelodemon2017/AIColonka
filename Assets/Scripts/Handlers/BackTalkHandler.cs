using DG.Tweening;
using System.Threading.Tasks;
using Zenject;

public class BackTalkHandler
{
    private SignalBus _signalBus;

    public string KeyTalk;
    private string LocalText;

    public string GetTalk =>
        string.IsNullOrWhiteSpace(KeyTalk) ? string.Empty :
        LocalText;

    [Inject]
    private void Construct(SignalBus signalBus)
    {
        _signalBus = signalBus;

        Init();
    }

    private void Init()
    {
        _signalBus.Subscribe<BackTalkSignal>(SetTalk);
    }

    private void SetTalk(BackTalkSignal backTalkSignal)
    {
        _ = SetTalkAsync(backTalkSignal.BackTalk);
    }

    private async Task SetTalkAsync(BackTalkSO backTalkSO)
    {
        KeyTalk = backTalkSO.KeyLocal;

        DOVirtual.DelayedCall(backTalkSO.Seconds, EndTalk);
        LocalText = await Localizations.GetLocalizedText(
            backTalkSO.Table, KeyTalk);
        _signalBus.Fire(new StartBackTalkSignal());
    }

    private void EndTalk()
    {
        KeyTalk = string.Empty;
        LocalText = string.Empty;
        _signalBus.Fire(new EndBackTalkSignal());
    }
}