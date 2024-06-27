using System;
using ET.Client;
using UnityEngine.UIElements;

namespace Timeline.Editor
{
    public abstract class ParamResolver
    {
    }

    public sealed class ParamResolver<T, K>: ParamResolver where T : BaseField<K>
    {
        private readonly T editorField;
        private readonly Label label;
        private readonly TextField textField;

        private readonly BehaviorControllerEditor controllerEditor;
        private readonly SharedVariable variable;

        public ParamResolver(SharedVariable _variable, BehaviorControllerEditor _editor)
        {
            variable = _variable;
            controllerEditor = _editor;

            var paramView = new BehaviorParamView() { variable = _variable };
            controllerEditor.parameterViewContainer.Add(paramView);

            //Editor Field
            paramView.style.flexDirection = FlexDirection.Row;
            editorField = Activator.CreateInstance(GetType().GenericTypeArguments[0]) as T;
            editorField.name = "param-editor-field";
            editorField.SetValueWithoutNotify((K)variable.value);
            editorField.RegisterCallback<BlurEvent>(_ => { SaveValue(); });
            editorField.AddToClassList("EditorField");

            //Label
            label = new Label(variable.name);
            label.name = "param-editor-label";
            label.AddToClassList("EditorLabel");

            //textField
            textField = new TextField();
            textField.name = "param-editor-text";
            textField.RegisterCallback<BlurEvent>(_ => { SaveName(); });
            textField.SetValueWithoutNotify(variable.name);
            textField.AddToClassList("EditorText");

            paramView.Add(textField);
            paramView.Add(label);
            paramView.Add(editorField);
        }

        private void SaveValue()
        {
            controllerEditor.ApplyModify(() => { variable.value = editorField.value; }, "Save value");
        }

        private void SaveName()
        {
            controllerEditor.ApplyModify(() => { variable.name = textField.value; }, "Save Name");
            controllerEditor.RefreshParamView();
        }
    }
}