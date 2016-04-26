using System;
using Xamarin.Forms;

namespace SlideOverKit
{
    public class PopupViewAttached
    {
        const int menuOffSet = 5;

        public static readonly BindableProperty TargetProperty =
            BindableProperty.CreateAttached (
                propertyName: "Target",
                returnType: typeof(string),
                declaringType: typeof(VisualElement),
                defaultValue: null,
                defaultBindingMode: BindingMode.OneWay,
                validateValue: null,
                propertyChanged: OnTargetChanged);

        public static void SetTarget (BindableObject bindable, object target)
        {
            bindable.SetValue (TargetProperty, target);
        }

        public static object GetTarget (BindableObject bindable)
        {
            return (object)bindable.GetValue(TargetProperty);
        }

        public static void OnTargetChanged (BindableObject bindable, object oldValue, object newValue)
        {
            var control = bindable as VisualElement;

            var parent = control.Parent;
            //FIXME if we use attached binding in XAML, control.Parent alway return null
            while (!(parent == null || parent is IPopupContainerPage)) {
                parent = parent.Parent;
            }

            if (parent is IPopupContainerPage) {
                var container = parent as IPopupContainerPage;
                if (container.PopupViews.ContainsKey (newValue.ToString ())) {
                    var popup = container.PopupViews [newValue.ToString ()] as SlidePopupView;
                    if (popup != null) {
                        popup.TargetControl = control;
                    }
                }

            }
        }            
    }
}

