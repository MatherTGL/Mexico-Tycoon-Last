namespace MenuCustomizer
{
    /// <summary>The type of the customized menu item.</summary>
    public enum MCType
    {
        /// <summary>Unspecified item.</summary>
        None,
        /// <summary>Select the command from menu item path.</summary>
        MenuPath,
        /// <summary>Unity's original command.</summary>
        Original,
        /// <summary>Input the command.</summary>
        Input,
        /// <summary>Add the separator.</summary>
        Separator,
    }
}
