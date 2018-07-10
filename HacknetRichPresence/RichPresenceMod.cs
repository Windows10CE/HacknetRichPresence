using Pathfinder.Event;

namespace HacknetRichPresence {
    public class RichPresenceMod : Pathfinder.ModManager.IMod {
        public string Identifier => "Hacknet Rich Presence";

        public void Load() {
            EventManager.RegisterListener<DrawMainMenuEvent>(RPCHandler.Init);
            EventManager.RegisterListener<GameUpdateEvent>(RPCHandler.Update);
        }
        
        public void LoadContent() { }

        public void Unload() { }
    }
}
