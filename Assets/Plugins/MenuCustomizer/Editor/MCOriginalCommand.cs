namespace MenuCustomizer
{
    /// <summary>The type of Unity's original command.</summary>
    public enum MCOriginalCommand
    {
        None,
        Copy,
        Paste,
        Duplicate,
        Delete,
        Rename,
        SelectPrefab
    }

    /// <summary>Extensions for 'MCOriginalCommand'</summary>
    public static class MCOriginalCommandExtensions
    {
        /// <summary>Convert command to the command name.</summary>
        public static string ToCommand(this MCOriginalCommand command)
        {
            switch(command)
            {
                case MCOriginalCommand.Copy: { return "Copy"; }
                case MCOriginalCommand.Paste: { return "Paste"; }
                case MCOriginalCommand.Duplicate: { return "Duplicate"; }
                case MCOriginalCommand.Delete: { return "Delete"; }
            }
            return "";
        }

        /// <summary>Convert command to the menu label.</summary>
        public static string ToLabel(this MCOriginalCommand command)
        {
            switch(command)
            {
                case MCOriginalCommand.Copy:
                    return "Copy";
                case MCOriginalCommand.Paste:
                    return "Paste";
                case MCOriginalCommand.Duplicate:
                    return "Duplicate";
                case MCOriginalCommand.Delete:
                    return "Delete";
                case MCOriginalCommand.Rename:
                    return "Rename";
                case MCOriginalCommand.SelectPrefab:
                    return "Select Prefab";
            }
            return "";
        }
    }
}
