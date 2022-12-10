using Dalamud.Configuration;
using Dalamud.Plugin;
using System;

namespace TruthOrDare
{
    public enum MainTab { PlayerList, Game, About }
    public enum NameMode { First, Last, Both }
    [Serializable]
    public class Configuration : IPluginConfiguration
    {
        public int Version { get; set; } = 0;

        public bool SomePropertyToBeSavedAndWithADefault { get; set; } = true;

        // the below exist just to make saving less cumbersome
        [NonSerialized]
        private DalamudPluginInterface? PluginInterface;
        public bool Debug { get; set; } = false;
        public string RollCommand { get; set; } = "/dice"; // /random
        public string ChatChannel { get; set; } = "/p"; // /s
        public bool AutoParty { get; set; } = true;
        public NameMode AutoNameMode { get; set; } = NameMode.First;

        public TruthOrDareConfig TruthOrDareConfig { get; set; } = new TruthOrDareConfig();
        public void Initialize(DalamudPluginInterface pluginInterface)
        {
            PluginInterface = pluginInterface;
        }

        public void Save()
        {
            PluginInterface!.SavePluginConfig(this);
        }
    }
}
