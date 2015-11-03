using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using umbraco.cms.businesslogic.datatype;
using umbraco.cms.businesslogic.propertytype;

namespace CogItemUsage
{
    public class FieldPicker : DataEditorSettingType
    {
        private readonly CheckBoxList _checkBoxList = new CheckBoxList();
        private string _val = string.Empty;
        private readonly List<Guid> _defaultDataTypes = new List<Guid> 
                                                            {
                                                                new Guid("5e9b75ae-face-41c8-b47e-5f4b0fd82f83"), // RTEs
                                                                new Guid("c2d6894b-e788-4425-bcf2-308568e3d38b"), // MNTPs
                                                                new Guid("158aa029-24ed-4948-939e-c3da209e5fba") // Content Pickers
                                                            };

        public override string Value
        {
            get
            {
                return string.Join(",", _checkBoxList.Items.OfType<ListItem>().Where(item => item.Selected).Select(item => item.Value));
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _val = value;
                }
            }
        }

        private List<string> GetFields(Guid dataTypeId)
        {
            var fields = new List<string>();
            IEnumerable<DataTypeDefinition> dataTypeDefinitions = DataTypeDefinition.GetAll().Where(d => d.DataType != null && d.DataType.Id == dataTypeId);

            foreach (var dtd in dataTypeDefinitions)
            {
                fields.AddRange(from pt in PropertyType.GetByDataTypeDefinition(dtd.Id) select pt.Alias);
            }

            if (dataTypeId == new Guid("5e9b75ae-face-41c8-b47e-5f4b0fd82f83"))
            {
                fields = fields.Select(field => "rteLink" + field).ToList();
            }

            return fields.Distinct().ToList();
        }

        public override System.Web.UI.Control RenderControl(DataEditorSetting setting)
        {
            _checkBoxList.ID = setting.GetName();
            _checkBoxList.RepeatColumns = 4;

            _checkBoxList.Items.Clear();

            IDictionary<string, string> propertyTypes = PropertyType.GetAll()
                .GroupBy(p => p.Alias)
                .Select(grp => grp.First())
                .Select(prop =>
                            {
                                if (prop.DataTypeDefinition.DataType.Id == new Guid("5e9b75ae-face-41c8-b47e-5f4b0fd82f83"))
                                {
                                    return new { Value = "rteLink" + prop.Alias, Key = prop.Alias };
                                }
                                return new { Value = prop.Alias, Key = prop.Alias };

                            })
                .ToDictionary(i => i.Key, i => i.Value);

            _checkBoxList.Items.AddRange(propertyTypes.Select(property => new ListItem(property.Key, property.Value)).ToArray());

            // default to all RTEs, MNTPs and Content Pickers if empty
            if (string.IsNullOrEmpty(_val))
            {
                _val = string.Join(",", _defaultDataTypes.SelectMany(GetFields));
            }

            foreach (string value in _val.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
            {
                if (_checkBoxList.Items.OfType<ListItem>().Any(item => item.Value == value))
                {
                    _checkBoxList.Items.OfType<ListItem>().Single(item => item.Value == value).Selected = true;
                }
            }

            return _checkBoxList;
        }
    }
}