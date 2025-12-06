using System.Text.RegularExpressions;
using SwiftlyS2.Shared.Players;

namespace PlaceholderAPI.Contract;

public interface IPlaceholderAPIv1
{
    // Action is (player, placeholderContent) => returnValue
    public void RegisterPlaceholder(string identifier, Regex finder, Func<IPlayer?, string, string> placeholderAction);
    public void UnregisterPlaceholder(string identifier);

    public string ProcessMessage(IPlayer? player, string message);
    public List<string> ProcessMessages(IPlayer? player, List<string> messages);
}