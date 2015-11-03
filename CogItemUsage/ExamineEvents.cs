using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Examine;
using umbraco.BusinessLogic;
using umbraco.MacroEngines;
using umbraco.NodeFactory;
using umbraco.cms.businesslogic.datatype;
using umbraco.cms.businesslogic.propertytype;
using umbraco.cms.businesslogic.web;
using UmbracoExamine;

namespace CogItemUsage
{
    public class ExamineEvents : ApplicationBase
    {
        private const string InternalIndexer = "InternalIndexer";

        private static IEnumerable<string> RteFields
        {
            get
            {
                var rtes = new List<string>();
                var rteGuid = new Guid("5e9b75ae-face-41c8-b47e-5f4b0fd82f83");
                IEnumerable<DataTypeDefinition> dataTypeDefinitions = DataTypeDefinition.GetAll().Where(d => d.DataType != null && d.DataType.Id == rteGuid);
                
                foreach (var dtd in dataTypeDefinitions)
                {
                    rtes.AddRange(from pt in PropertyType.GetByDataTypeDefinition(dtd.Id)
                                  select pt.Alias);
                }

                return rtes.Distinct().ToList();

            }
        }
        private static IEnumerable<string> TreePickerFields
        {
            get
            {
                var rtes = new List<string>();
                var rteGuid = new Guid("c2d6894b-e788-4425-bcf2-308568e3d38b");
                foreach (var dtd in DataTypeDefinition.GetAll().Where(d => d.DataType != null && d.DataType.Id == rteGuid))
                {
                    rtes.AddRange(from pt in PropertyType.GetByDataTypeDefinition(dtd.Id)
                                  select pt.Alias);
                }

                return rtes.Distinct().ToList();

            }
        }

        public ExamineEvents()
        {
            ExamineManager.Instance.IndexProviderCollection[InternalIndexer].GatheringNodeData += ExamineEventsInternal_GatheringNodeData;
        }

        void ExamineEventsInternal_GatheringNodeData(object sender, IndexingNodeDataEventArgs e)
        {
            //need to fudge rte field so that we have internal link nodeids in the internal index            
            var rteFields = RteFields;

            if (e.IndexType == IndexTypes.Content)
            {
                var d = new Document(e.NodeId);
                foreach (var rteField in rteFields)
                {
                    if (d.getProperty(rteField) != null && d.getProperty(rteField).Value != null)
                    {
                        var rteEncoded = HttpUtility.HtmlEncode(d.getProperty(rteField).Value.ToString().Replace("localLink:", "localLink: ").Replace("}", " } "));
                        e.Fields.Add("rteLink" + rteField, rteEncoded);
                    }
                }

                var treePickerFields = TreePickerFields;
                foreach (var treePickerField in treePickerFields)
                {
                    if (e.Fields.ContainsKey(treePickerField))
                    {
                        var content = e.Fields[treePickerField];

                        // if it's a csv type and there's more than one item,
                        // separate with a space so the nodes are indexed separately
                        if (content.Contains(","))
                        {
                            content = content.Replace(",", " ");
                        }
                        else
                        {
                            // if it's an XML type tree picker, get the xml and transform into a space separated list
                            var node = new Node(e.NodeId);
                            var value = node.GetProperty(treePickerField).Value;

                            if (value.Contains("<MultiNodePicker"))
                            {
                                var dynamicXml = new DynamicXml(value);
                                content = string.Join(" ", dynamicXml.Descendants().Select(de => de.InnerText));
                            }
                        }
                        e.Fields[treePickerField] = content;
                    }
                }

                e.Fields.Add("IsPublished", d.Published.ToString());
            }
        }
    }
}