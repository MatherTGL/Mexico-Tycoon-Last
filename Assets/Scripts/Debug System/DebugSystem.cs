using UnityEngine;
using System.Text;


namespace DebugCustomSystem
{
    public sealed class DebugSystem
    {
        private static Color _infoColor = Color.white;
        private static Color _warningColor = Color.yellow;
        private static Color _errorColor = Color.red;

        public enum SelectedColor
        {
            Green, White, Orange, Red, Black, Grey, Blue, Yellow
        }


        public static void Log(object message, SelectedColor color, string additionalMessage = "", string tag = "Non")
        {
            Debug.Log(FormatMessage(message, CheckColorType(color), tag, additionalMessage));
        }

        public static void Log(object message, SelectedColor color, string additionalMessage = "", params string[] tag)
        {
            Debug.Log(FormatMessage(message, CheckColorType(color), additionalMessage, tag));
        }

        private static string FormatMessage(object message, string colorText, string tag, string additionalMessage = "")
        {
            return $"<b><color=#F9C74F>[{tag}]</color> <color={colorText}>{additionalMessage}</color></b> <color={_infoColor}>{message}</color>";
        }

        private static string FormatMessage(object message, string colorText, string additionalMessage = "", params string[] tag)
        {
            var builderString = new StringBuilder();

            for (int i = 0; i < tag.Length; i++)
            {
                string allTags = tag[i];
                builderString.Append($"[{allTags}]");
            }

            return $"<b><color=#F9C74F>{builderString}</color> <color={colorText}>{additionalMessage}</color></b> <color={_infoColor}>{message}</color>";
        }

        private static string CheckColorType(SelectedColor color)
        {
            if (color == SelectedColor.Green)
                return "#2A9D8F";
            else if (color == SelectedColor.White)
                return "#EDF2F4";
            else if (color == SelectedColor.Orange)
                return "#F4A261";
            else if (color == SelectedColor.Grey)
                return "#415A77";
            else if (color == SelectedColor.Red)
                return "#E56B6F";
            else if (color == SelectedColor.Blue)
                return "#0097C7";
            else if (color == SelectedColor.Black)
                return "#1B263B";
            else if (color == SelectedColor.Yellow)
                return "#F9C74F";
            return "#EDF2F4";
        }
    }
}
