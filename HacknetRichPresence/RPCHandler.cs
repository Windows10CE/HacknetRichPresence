using Hacknet.Extensions;
using Pathfinder.Event;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HacknetRichPresence {
    public static class RPCHandler {
        private static bool _enabled = false;

        private const string clientId = "428716603719417867";
        private static DiscordRpc.RichPresence _presence = new DiscordRpc.RichPresence();
        private static DiscordRpc.EventHandlers _handlers;

        private static bool _auto = true;
        private static string _controller = null;

        private static State _newState = new State("In Menu", "oghn");
        private static State _currentState;

        private static SortedDictionary<string, State> _states = new SortedDictionary<string, State>() {
            { "OG Menu", new State("In Menu", "oghn") },
            { "OG Game", new State("Hacknet Campaign", "oghn") }
        };

        public static void Init(DrawMainMenuEvent e) {
            if (File.Exists(Path.Combine(Environment.CurrentDirectory, "Mods", "discord-rpc.dll"))) {
                _enabled = true;
                _handlers = new DiscordRpc.EventHandlers();
                DiscordRpc.Initialize(clientId, ref _handlers, true, null);
            } else
                Console.WriteLine(@"Mods/discord-rpc.dll does not exist! Please download it from https://drive.google.com/open?id=1rJvhzQojvlLmM0Hngr5KaQZvuuEMqGTa !");
        }

        public static void Update(GameUpdateEvent e) {
            if (!_enabled)
                return;
            if (_auto) {
                if (ExtensionLoader.ActiveExtensionInfo != (ExtensionInfo)null && _currentState.Description != ExtensionLoader.ActiveExtensionInfo.Name)
                    _newState = new State(ExtensionLoader.ActiveExtensionInfo.Name, "wksp");
                else if (ExtensionLoader.ActiveExtensionInfo == (ExtensionInfo)null)
                    _newState = Hacknet.OS.currentInstance != null ? _states.Values.Where(x => x.Description == "Hacknet Campaign").First() : _states.Values.Where(x => x.Description == "In Menu").First();
            }
            if (_newState != _currentState) {
                _currentState = _newState;
                _presence.details = _currentState.Description;
                _presence.largeImageKey = _currentState.ImageName;
                _presence.smallImageKey = _currentState.SmallImageName;
                DiscordRpc.UpdatePresence(_presence);
            }
        }

        public static bool UpdateState(string modName, string name) {
            if (_auto || modName != _controller)
                return false;
            if (_states.TryGetValue(name, out State state)) {
                _newState = state;
                return true;
            } else
                return false;
        }

        public static void AddState(string name, State state) {
            if (_states.ContainsKey(name)) {
                Console.WriteLine("Warning! A mod has tried to add a state that already exists!");
                return;
            }
            _states.Add(name, state);
        }

        public static void SetAutomaticStateDetection(string modName, bool enable) {
            if (!_auto && _controller != modName && _controller != null) {
                Console.WriteLine($"{modName} has already taken control of Rich Presence Handling!");
                return;
            }
            _auto = enable;
            _controller = modName;
        }
    }
}
