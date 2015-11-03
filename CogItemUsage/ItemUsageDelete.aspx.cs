using System;
using System.Collections.Generic;
using System.Linq;
using umbraco.BusinessLogic;
using umbraco.cms.businesslogic.datatype;

namespace CogItemUsage
{
    public partial class ItemUsageDelete : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //get the settings for item usage datatype from datatype settings in developer section
            var dataTypeDefinitions = DataTypeDefinition.GetAll().Where(dataTypeDef => dataTypeDef.Text == "Item Usage Datatype");

            if (!dataTypeDefinitions.Any())
            {
                usageControl.Visible = false;
                litMessage.Text = "There is no page usage data available - create a datatype called \"Item Usage Datatype\" and configure it to activate.";
            }
            else
            {
                var s = GetDataTypeSettingsForItemDataUsage(dataTypeDefinitions);
                PassSettingsToItemUsageUserControl(s);
            }
        }

        /// <summary>
        /// using reflection set the properties of the cogitem usage usercontrol on this page
        /// </summary>
        /// <param name="s"></param>
        private void PassSettingsToItemUsageUserControl(List<Setting<string, string>> s)
        {
            foreach (Setting<string, string> setting in s)
            {
                try
                {
                    if (!string.IsNullOrEmpty(setting.Key))
                    {
                        usageControl.GetType().InvokeMember(setting.Key, System.Reflection.BindingFlags.SetProperty, null,
                                                            usageControl, new object[] {setting.Value});
                    }
                }
                catch (MissingMethodException)
                {
                    Log.Add(LogTypes.Error, int.Parse(Request.QueryString["id"].ToString()),"Error processing settings for Item Usage Datatype settings during delete");
                }
            }
        }

        /// <summary>
        /// gets settings setup for cog item data usage. we are making assumption there will only ever be one
        /// </summary>
        /// <param name="dataTypeDefinitions"></param>
        /// <returns></returns>
        private static List<Setting<string, string>> GetDataTypeSettingsForItemDataUsage(IEnumerable<DataTypeDefinition> dataTypeDefinitions)
        {
            DataTypeDefinition dataTypeDef = dataTypeDefinitions.First();
            // load the settings
            DataEditorSettingsStorage ss = new DataEditorSettingsStorage();
            List<Setting<string, string>> s = ss.GetSettings(dataTypeDef.Id);
            ss.Dispose();
            return s;
        }
    }
}