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
        _ = SetTalkAsync(backTalkSignal);
    }

    private async Task SetTalkAsync(BackTalkSignal backTalkSignal)
    {
        KeyTalk = backTalkSignal.Key;

        DOVirtual.DelayedCall(backTalkSignal.Time, EndTalk);
        LocalText = await Localizations.GetLocalizedText(
            backTalkSignal.FromLocalTable, KeyTalk);
    }

    private void EndTalk()
    {
        KeyTalk = string.Empty;
        LocalText = string.Empty;
        _signalBus.Fire(new EndBackTalkSignal());
    }
}