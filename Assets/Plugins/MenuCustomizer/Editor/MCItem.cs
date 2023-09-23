namespace MenuCustomizer
{
    /// <summary>class for each menu item.</summary>
    public class MCItem
    {
        /// <summary>The type of the customized menu item.</summary>
        public MCType type = MCType.None;

        /// <summary>The menu command for 'EditorApplication.ExecuteMenuItem'</summary>
        public string command = "";

        /// <summary>Assign when you use Unity's original command.</summary>
        public MCOriginalCommand original = MCOriginalCommand.None;

        /// <summary>Whether if you override the label or not.</summary>
        public bool overrideLabel = false;

        /// <summary>The label that is displayed on the context menu</summary>
        public string label = "";

        /// <summary>Default constructor for XML parsing.</summary>
        public MCItem()
        {
            this.type = MCType.None;
            this.command = "";
            this.original = MCOriginalCommand.None;
            this.overrideLabel = false;
            this.label = "";
        }

        /// <summary>Constructor with initializing.</summary>
        public MCItem(MCType type, string command, bool overrideLabel = false, string label = "")
        {
            this.type = type;
            this.command = command;
            this.original = MCOriginalCommand.None;
            this.overrideLabel = overrideLabel;
            this.label = overrideLabel ? label : command;
        }

        /// <summary>Constructor for Unity's original command.</summary>
        public MCItem(MCOriginalCommand command, bool overrideLabel = false, string label = "")
        {
            this.type = MCType.Original;
            this.command = "";
            this.original = command;
            this.overrideLabel = overrideLabel;
            this.label = overrideLabel ? label : command.ToLabel();
        }

        public void UpdateLabelFromCommand()
        {
            if(!this.overrideLabel)
            {
                this.label = (this.type == MCType.Original) ? this.original.ToLabel() : this.command;
            }
        }

        public static MCItem CreateSeparator()
        {
            return new MCItem(MCType.Separator, "");
        }
    }
}
