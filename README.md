<div align="center">
  <img src="https://pan.samyyc.dev/s/VYmMXE" />
  <h2><strong>Placeholder API</strong></h2>
  <h3>A shared API to let you use placeholders in all the places you're thinking.</h3>
</div>

<p align="center">
  <img src="https://img.shields.io/badge/build-passing-brightgreen" alt="Build Status">
  <img src="https://img.shields.io/github/downloads/SwiftlyS2-Plugins/PlaceholderAPI/total" alt="Downloads">
  <img src="https://img.shields.io/github/stars/SwiftlyS2-Plugins/PlaceholderAPI?style=flat&logo=github" alt="Stars">
  <img src="https://img.shields.io/github/license/SwiftlyS2-Plugins/PlaceholderAPI" alt="License">
</p>

## Building

- Open the project in your preferred .NET IDE (e.g., Visual Studio, Rider, VS Code).
- Build the project. The output DLL and resources will be placed in the `build/` directory.
- The publish process will also create a zip file for easy distribution.

## Publishing

- Use the `dotnet publish -c Release` command to build and package your plugin.
- Distribute the generated zip file or the contents of the `build/publish` directory.

## API Reference

### IPlaceholderAPIv1

The PlaceholderAPI exposes a shared interface that can be used by other plugins to register custom placeholders and process messages.

#### Methods

##### RegisterPlaceholder
```csharp
void RegisterPlaceholder(string identifier, Regex finder, Func<IPlayer?, string, string> placeholderAction)
```
Register a custom placeholder with a unique identifier, a regex pattern to match, and an action that processes the placeholder.

**Parameters:**
- `identifier` - A unique string to identify your placeholder
- `finder` - A Regex pattern to match the placeholder in messages
- `placeholderAction` - A function that takes an optional player and the matched placeholder content, returning the replacement string

**Example:**
```csharp
// In your plugin's UseSharedInterface method
var placeholderAPI = interfaceManager.GetSharedInterface<IPlaceholderAPIv1>("PlaceholderAPI.v1");

// Register a custom placeholder
placeholderAPI.RegisterPlaceholder(
    "myCustomPlaceholder",
    new Regex(@"{CUSTOMVALUE}"),
    (player, content) => "My Custom Value"
);
```

##### UnregisterPlaceholder
```csharp
void UnregisterPlaceholder(string identifier)
```
Remove a previously registered placeholder by its identifier.

**Parameters:**
- `identifier` - The unique identifier of the placeholder to remove

##### ProcessMessage
```csharp
string ProcessMessage(IPlayer? player, string message)
```
Process a single message, replacing all placeholders with their corresponding values.

**Parameters:**
- `player` - The player context for player-specific placeholders (can be null)
- `message` - The message containing placeholders to process

**Returns:** The processed message with all placeholders replaced

**Example:**
```csharp
var processedMsg = placeholderAPI.ProcessMessage(player, "Welcome {PLAYERNAME} to {HOSTNAME}!");
```

##### ProcessMessages
```csharp
List<string> ProcessMessages(IPlayer? player, List<string> messages)
```
Process multiple messages at once, replacing all placeholders in each message.

**Parameters:**
- `player` - The player context for player-specific placeholders (can be null)
- `messages` - A list of messages containing placeholders to process

**Returns:** A list of processed messages with all placeholders replaced

## Built-in Placeholders

The PlaceholderAPI comes with several built-in placeholders:

| Placeholder | Description | Example Output |
|------------|-------------|----------------|
| `{MAXPLAYERS}` | Maximum number of players allowed on the server | `64` |
| `{SERVERIP}` | The server's IP address | `192.168.1.1` |
| `{SERVERPORT}` | The server's port number | `27015` |
| `{PLAYERIP}` | The player's IP address | `192.168.1.100` |
| `{PLAYERCOUNT}` | Current number of players on the server | `24` |
| `{PLAYERNAME}` | The player's name (or "Console" if no player) | `PlayerName123` |
| `{STEAMID}` | The player's SteamID | `76561198012345678` |
| `{STEAMID32}` | The player's SteamID | `STEAM_0:0:569544375` |
| `{STEAMID3}` | The player's SteamID | `[U:1:1139088750]` |
| `{HOSTNAME}` | The server's hostname | `My Awesome Server` |
| `{MAPNAME}` | The current map name | `de_dust2` |
| `{DATE}` | Current date in yyyy-MM-dd format | `2025-12-06` |
| `{TIME}` | Current time in HH:mm:ss format | `14:30:45` |
| `{DATETIME}` | Current date and time | `2025-12-06 14:30:45` |
| `{UPTIME}` | Server uptime | `05h 23m 14s` |
| `{UNIXTIMESTAMP:seconds}` | Converts Unix timestamp to local datetime | `{UNIXTIMESTAMP:1700000000}` → `2023-11-14 17:33:20` |

## Usage Example

### Using PlaceholderAPI in Your Plugin

1. **Add the contract reference** to your plugin, following https://swiftlys2.net/docs/development/shared-api/#complete-example

2. Use the interface named `PlaceholderAPI.v1`.
