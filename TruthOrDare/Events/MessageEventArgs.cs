using System;

namespace TruthOrDare
{
    public class MessageEventArgs : EventArgs
    {
        public string Message { get; private set; }
        public MessageType MessageType { get; private set; }
        public MainTab ModuleTab { get; private set; }

        public MessageEventArgs(string message, MessageType messageType, MainTab moduleTab)
        {
            Message = message;
            MessageType = messageType;
            ModuleTab = moduleTab;
        }
    }

    public enum MessageType
    {
        Normal, TruthOrDareRoll
    }
}
