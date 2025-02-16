using Avalonia.Controls;
using Avalonia.Controls.Templates;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRCFaceTracking.Core.Params.Data.Mutation;

namespace VRCFaceTracking.Helpers;

public class ComponentTemplateSelector : IDataTemplate
{
    public IDataTemplate CheckboxTemplate { get; set; }
    public IDataTemplate TextInputTemplate { get; set; }
    public IDataTemplate SliderTemplate { get; set; }
    public IDataTemplate ButtonTemplate { get; set; }
    public IDataTemplate RangeTemplate { get; set; }

    public Control Build(object param)
    {
        if (param is MutationProperty property)
            return property.Type switch
            {
                MutationPropertyType.CheckBox => CheckboxTemplate.Build(property),
                MutationPropertyType.TextBox => TextInputTemplate.Build(property),
                MutationPropertyType.Slider => SliderTemplate.Build(property),
                _ => throw new NotImplementedException()
            };
        if (param is MutationRangeProperty)
            return RangeTemplate.Build(param);
        if (param is MutationAction)
            return ButtonTemplate.Build(param);

        throw new NotImplementedException();
    }

    public bool Match(object data)
    {
        return data is MutationAction || data is MutationProperty || data is MutationRangeProperty;
    }
}
