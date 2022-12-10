using ImGuiNET;

namespace TruthOrDare.Extensions
{
    public static class ImGuiEx
    {
        /// <summary>Extension for converting between string/integer for display & updating of a value.</summary>
        public static void InputText(string label, ref int value, uint length = 255)
        {
            string strVal = value.ToString();
            ImGui.InputText(label, ref strVal, length);
            int.TryParse(strVal, out value);
        }

        /// <summary>Extension to enable passing property as value with referencing behaviour.</summary>
        public static bool InputInt(string label, object obj, string nameofProp)
        {
            var p = obj.GetType().GetProperty(nameofProp);
            int x = (int)(p?.GetValue(obj) ?? 0);
            bool r = ImGui.InputInt(label, ref x);
            p?.SetValue(obj, x);
            return r;
        }

        /// <summary>Extension to enable passing property as value with referencing behaviour.</summary>
        public static bool InputFloat(string label, object obj, string nameofProp)
        {
            var p = obj.GetType().GetProperty(nameofProp);
            float x = (float)(p?.GetValue(obj) ?? 0f);
            bool r = ImGui.InputFloat(label, ref x);
            p?.SetValue(obj, x);
            return r;
        }

        /// <summary>Extension to enable passing property as value with referencing behaviour.</summary>
        public static bool Checkbox(string label, object obj, string nameofProp)
        {
            var p = obj.GetType().GetProperty(nameofProp);
            bool x = (bool)(p?.GetValue(obj) ?? false);
            bool r = ImGui.Checkbox(label, ref x);
            p?.SetValue(obj, x);
            return r;
        }
    }
}
