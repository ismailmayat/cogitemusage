namespace CogItemUsage
{
    public class DeleteActionItem : umbraco.interfaces.IAction
    {
        #region IAction Members
        private string _alias = "delete";
        public string Alias
        {
            get { return _alias; }
        }

        public bool CanBePermissionAssigned
        {
            get { return true; }
        }

        public string Icon
        {
            get { return ".sprDelete"; }
        }

        public string JsFunctionName
        {
            get
            {
                return "itemUsageDelete()";
            }
        }

        public string JsSource
        {
            get { return "js/itemUsage.js"; }
        }

        public char Letter
        {
            get { return '#'; }
        }

        public bool ShowInNotifier
        {
            get { return false; }
        }

        #endregion
    }
}