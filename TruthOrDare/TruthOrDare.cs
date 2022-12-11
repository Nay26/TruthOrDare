using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin;
using System.IO;
using System.Reflection;
using Dalamud.Interface.Windowing;
using TruthOrDare.Windows;
using Dalamud.Game.ClientState;
using Dalamud.Game.ClientState.Objects;
using Dalamud.Game;
using XivCommon;
using Dalamud.Game.Gui;

namespace TruthOrDare
{
    public class TruthOrDare : IDalamudPlugin
    {
        [PluginService] public static ClientState ClientState { get; private set; } = null!;
        [PluginService] public static ObjectTable Objects { get; private set; } = null!;
        [PluginService] public static SigScanner SigScanner { get; private set; } = null!;
        [PluginService] public static ChatGui ChatGui { get; private set; } = null!;

        public string Name => "TruthOrDare";
        private const string CommandName = "/tord";
        private DalamudPluginInterface PluginInterface { get; init; }
        private CommandManager CommandManager { get; init; }

        private static MainWindow MainWindow;
        public WindowSystem WindowSystem = new("TruthOrDare");
        public static XivCommonBase XivCommon;

        public TruthOrDare(
            [RequiredVersion("1.0")] DalamudPluginInterface pluginInterface,
            [RequiredVersion("1.0")] CommandManager commandManager)
        {
            this.PluginInterface = pluginInterface;
            this.CommandManager = commandManager;
            XivCommon = new XivCommonBase();

            WindowSystem = new WindowSystem(Name);
            MainWindow = new MainWindow(this) { IsOpen = false };
            MainWindow.Config = PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
            MainWindow.Config.Initialize(PluginInterface);
            WindowSystem.AddWindow(MainWindow);
            MainWindow.Initialize();

            this.CommandManager.AddHandler(CommandName, new CommandInfo(OnCommand)
            {
                HelpMessage = "A useful message to display in /xlhelp"
            });
            
            this.PluginInterface.UiBuilder.Draw += DrawUI;

            ChatGui.ChatMessage += MainWindow.Game.OnChatMessage;
        }

        public void Dispose()
        {
            this.WindowSystem.RemoveAllWindows();
            this.CommandManager.RemoveHandler(CommandName);
        }

        private void OnCommand(string command, string args)
        {
            // in response to the slash command, just display our main ui
            WindowSystem.GetWindow("Truth Or Dare").IsOpen = true;
        }

        private void DrawUI()
        {
            this.WindowSystem.Draw();
        }
    }
}
