using System;
using System.Runtime.InteropServices;
using Dalamud.Game;
using Dalamud.Hooking;
using Dalamud.Logging;
using Dalamud.Plugin.Services;

namespace TruthOrDare.Util
{
    public delegate void UpdatePartyDelegate(IntPtr hudAgent);

    public sealed class UpdateParty : SeFunction<UpdatePartyDelegate>
    {
        public UpdateParty() : base("40 ?? 48 83 ?? ?? 48 8B ?? 48 ?? ?? ?? 48 ?? ?? ?? ?? ?? ?? 83 ?? ?? ?? ?? ?? ?? 74 ?? 48")
        {
        }
    }

    public class SeFunction<T> where T : Delegate
    {
        public IntPtr Address;
        protected T? FuncDelegate;

        public SeFunction(int offset)
        {
            Address = TruthOrDare.SigScanner.Module.BaseAddress + offset;
        }

        public SeFunction(string signature, int offset = 0)
        {
            Address = TruthOrDare.SigScanner.ScanText(signature);
            if (Address != IntPtr.Zero)
                Address += offset;
            var baseOffset = (ulong)Address.ToInt64() - (ulong)TruthOrDare.SigScanner.Module.BaseAddress.ToInt64();
        }

        public T? Delegate()
        {
            if (FuncDelegate != null)
                return FuncDelegate;

            if (Address != IntPtr.Zero)
            {
                FuncDelegate = Marshal.GetDelegateForFunctionPointer<T>(Address);
                return FuncDelegate;
            }

            return null;
        }

        public dynamic? Invoke(params dynamic[] parameters)
        {
            if (FuncDelegate != null)
                return FuncDelegate.DynamicInvoke(parameters);

            if (Address != IntPtr.Zero)
            {
                FuncDelegate = Marshal.GetDelegateForFunctionPointer<T>(Address);
                return FuncDelegate!.DynamicInvoke(parameters);
            }
            else
            {
                return null;
            }
        }

        public Hook<T>? CreateHook(T detour)
        {
            if (Address != IntPtr.Zero)
            {
                var hook = TruthOrDare.GameInteropProvider.HookFromAddress<T>(Address, detour);
                hook.Enable();
                return hook;
            }

            return null;
        }
    }
}
