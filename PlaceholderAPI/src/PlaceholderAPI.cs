using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using PlaceholderAPI.API;
using PlaceholderAPI.Contract;
using SwiftlyS2.Shared;
using SwiftlyS2.Shared.Commands;
using SwiftlyS2.Shared.Players;
using SwiftlyS2.Shared.Plugins;

namespace PlaceholderAPI;

[PluginMetadata(Id = "PlaceholderAPI", Version = "1.0.0", Name = "PlaceholderAPI", Author = "Swiftly Development Team")]
public partial class PlaceholderAPI(ISwiftlyCore core) : BasePlugin(core)
{
    public ConcurrentDictionary<string, (Regex, Func<IPlayer?, string, string>)> Placeholders = [];
    private IPlaceholderAPIv1? _placeholderAPIv1;

    public override void ConfigureSharedInterface(IInterfaceManager interfaceManager)
    {
        interfaceManager.AddSharedInterface<IPlaceholderAPIv1, PlaceholderAPIv1>("PlaceholderAPI.v1", new PlaceholderAPIv1(ref Placeholders));
    }

    public override void UseSharedInterface(IInterfaceManager interfaceManager)
    {
        _placeholderAPIv1 = interfaceManager.GetSharedInterface<IPlaceholderAPIv1>("PlaceholderAPI.v1");
    }

    public override void Load(bool hotReload)
    {
        Placeholders.TryAdd("maxplayers", new(new Regex("{MAXPLAYERS}"), (player, placeholderContent) =>
        {
            return Core.Engine.GlobalVars.MaxClients.ToString();
        }));

        Placeholders.TryAdd("serverip", new(new Regex("{SERVERIP}"), (player, placeholderContent) =>
        {
            return Core.Engine.ServerIP ?? "0.0.0.0";
        }));

        Placeholders.TryAdd("serverport", new(new Regex("{SERVERPORT}"), (player, placeholderContent) =>
        {
            var portCvar = Core.ConVar.Find<int>("hostport");
            return portCvar?.Value.ToString() ?? "27015";
        }));

        Placeholders.TryAdd("playerip", new(new Regex("{PLAYERIP}"), (player, placeholderContent) =>
        {
            return player?.IPAddress ?? "0.0.0.0";
        }));

        Placeholders.TryAdd("playercount", new(new Regex("{PLAYERCOUNT}"), (player, placeholderContent) =>
        {
            return Core.PlayerManager.GetAllPlayers().Count().ToString();
        }));

        Placeholders.TryAdd("playername", new(new Regex("{PLAYERNAME}"), (player, placeholderContent) =>
        {
            return player?.Controller.PlayerName ?? "Console";
        }));

        Placeholders.TryAdd("steamid", new(new Regex("{STEAMID}"), (player, placeholderContent) =>
        {
            return player?.SteamID.ToString() ?? "0";
        }));

        Placeholders.TryAdd("hostname", new(new Regex("{HOSTNAME}"), (player, placeholderContent) =>
        {
            var hostnameCvar = Core.ConVar.Find<string>("hostname");
            return hostnameCvar?.Value ?? "";
        }));

        Placeholders.TryAdd("mapname", new(new Regex("{MAPNAME}"), (player, placeholderContent) =>
        {
            return Core.Engine.GlobalVars.MapName.Value;
        }));

        Placeholders.TryAdd("date", new(new Regex("{DATE}"), (player, placeholderContent) =>
        {
            return DateTime.Now.ToString("yyyy-MM-dd");
        }));

        Placeholders.TryAdd("time", new(new Regex("{TIME}"), (player, placeholderContent) =>
        {
            return DateTime.Now.ToString("HH:mm:ss");
        }));

        Placeholders.TryAdd("datetime", new(new Regex("{DATETIME}"), (player, placeholderContent) =>
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }));

        Placeholders.TryAdd("uptime", new(new Regex("{UPTIME}"), (player, placeholderContent) =>
        {
            var uptime = DateTime.Now - System.Diagnostics.Process.GetCurrentProcess().StartTime;
            return $"{(int)uptime.TotalHours:D2}h {uptime.Minutes:D2}m {uptime.Seconds:D2}s";
        }));

        Placeholders.TryAdd("unixtimestamp", new(new Regex(@"{UNIXTIMESTAMP:(.*?)}"), (player, placeholderContent) =>
        {
            var match = Regex.Match(placeholderContent, @"{UNIXTIMESTAMP:(.*?)}");
            if (match.Success && long.TryParse(match.Groups[1].Value, out long seconds))
            {
                return DateTimeOffset.FromUnixTimeSeconds(seconds).LocalDateTime.ToString("yyyy-MM-dd HH:mm:ss");
            }
            return DateTimeOffset.FromUnixTimeSeconds(0).LocalDateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }));
    }

    public override void Unload()
    {
    }
}