using System;
using System.Linq;
using Examine;
using Examine.Providers;
using umbraco.editorControls.userControlGrapper;
using umbraco.cms.businesslogic.datatype;
using UmbracoExamine;

namespace CogItemUsage
{
    public partial class ItemUsageDatatype : System.Web.UI.UserControl, IUsercontrolDataEditor
    {
        [DataEditorSetting("List of fields to search on", type = typeof(FieldPicker))]
        public string PropertyTypes { get; set; }

        [DataEditorSetting("Header text for listing table")]
        public string ItemUsageHeaderText { get; set; }

        [DataEditorSetting("No results found message")]
        public string NoResultstext { get; set; }

        [DataEditorSetting("Show on load")]
        public string ShowOnLoad { get; set; }

        public const string InternalIndex = "InternalSearcher";
        readonly BaseSearchProvider searcher = ExamineManager.Instance.SearchProviderCollection[InternalIndex];
        private ISearchResults _results;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Visible)
            {
                if (ShowOnLoad.Equals("0"))
                {
                    findUsage.Visible = true;
                }
                if (PropertyTypes != string.Empty && ShowOnLoad.Equals("1"))
                {
                    GetUsageData();
                }
                else if (PropertyTypes == string.Empty)
                {
                    litMessage.Text = "Please select some properties to search on in the data type configuration.";
                }
            }
        }

        private void GetUsageData()
        {
            int nodeId = Int32.Parse(Request.QueryString["id"]);
            var criteria = searcher.CreateSearchCriteria(IndexTypes.Content);
            string[] fields = PropertyTypes.Split(new[] { ',' });

            var query = criteria.OrderBy("__nodeName");
            query = fields.Aggregate(query, (current, field) => current.Or().Field(field, nodeId.ToString()));

            _results = searcher.Search(query.Compile());
            queryGenerated.Value = criteria.ToString();

            if (_results.Any())
            {
                rptItemUsage.DataSource = _results;
                rptItemUsage.DataBind();
                litMessage.Text = "The page is referenced by the pages listed below";
            }
            else
            {
                rptItemUsage.Visible = false;
                litMessage.Text = NoResultstext;
            }
        }

        #region IUsercontrolDataEditor Members

        public object value
        {
            get { return string.Empty; }
            set { value.ToString(); }
        }

        #endregion

        protected void findUsage_Click(object sender, EventArgs e)
        {
            if (PropertyTypes != string.Empty)
            {
                GetUsageData();
            }
        }
    }
}