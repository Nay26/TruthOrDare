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
using Dalamud.Game.Gui;
using Util;
using Dalamud.Plugin.Services;
using System.Linq;

namespace TruthOrDare
{
    public class TruthOrDare : IDalamudPlugin
    {
        [PluginService] public static IClientState ClientState { get; private set; } = null!;
        [PluginService] public static IObjectTable Objects { get; private set; } = null!;

        [PluginService]
        internal static IGameInteropProvider GameInteropProvider { get; private set; } = null!;

        internal static PluginAddressResolver Address { get; set; } = null!;
        [PluginService] public static ISigScanner SigScanner { get; private set; } = null!;
        [PluginService] public static IChatGui ChatGui { get; private set; } = null!;
        public static Chat Chat;

        public string Name => "TruthOrDare";
        private const string CommandName = "/tord";
        private DalamudPluginInterface PluginInterface { get; init; }
        private ICommandManager CommandManager { get; init; }

        private static MainWindow MainWindow;
        public WindowSystem WindowSystem = new("TruthOrDare");

        public TruthOrDare(
            [RequiredVersion("1.0")] DalamudPluginInterface pluginInterface,
            [RequiredVersion("1.0")] ICommandManager commandManager)
        {
            this.PluginInterface = pluginInterface;
            this.CommandManager = commandManager;

            WindowSystem = new WindowSystem(Name);
            MainWindow = new MainWindow(this) { IsOpen = false };
            MainWindow.Config = PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
            Chat = new Chat(SigScanner.);
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
            WindowSystem.Windows.FirstOrDefault(w => w.WindowName.Equals("Truth Or Dare")).IsOpen = true;
        }

        private void DrawUI()
        {
            this.WindowSystem.Draw();
        }
    }
}
