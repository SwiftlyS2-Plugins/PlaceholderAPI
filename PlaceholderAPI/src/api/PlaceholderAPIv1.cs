using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using PlaceholderAPI.Contract;
using SwiftlyS2.Shared.Players;

namespace PlaceholderAPI.API;

public class PlaceholderAPIv1 : IPlaceholderAPIv1
{
    private readonly ConcurrentDictionary<string, (Regex, Func<IPlayer?, string, string>)> _placeholders;

    public PlaceholderAPIv1(ref ConcurrentDictionary<string, (Regex, Func<IPlayer?, string, string>)> placeholders)
    {
        _placeholders = placeholders;
    }

    public string ProcessMessage(IPlayer? player, string message)
    {
        foreach (var (finder, action) in _placeholders.Values)
        {
            message = finder.Replace(message, match => action(player, match.Value));
        }
        return message;
    }

    public List<string> ProcessMessages(IPlayer? player, List<string> messages)
    {
        var processedMessages = new List<string>();
        foreach (var message in messages)
        {
            processedMessages.Add(ProcessMessage(player, message));
        }
        return processedMessages;
    }

    public void RegisterPlaceholder(string identifier, Regex finder, Func<IPlayer?, string, string> placeholderAction)
    {
        if (!_placeholders.ContainsKey(identifier))
        {
            _placeholders.TryAdd(identifier, (finder, placeholderAction));
        }
    }

    public void UnregisterPlaceholder(string identifier)
    {
        _placeholders.TryRemove(identifier, out _);
    }
}