using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp4.Code
{
    public static class AttachedNumericValidationBehavior
    {
        public static readonly BindableProperty AttachBehaviorProperty =
            BindableProperty.CreateAttached("AttachBehavior", typeof(bool),
    typeof(AttachedNumericValidationBehavior), false, propertyChanged: OnAttachBehaviorChanged);
        public static bool GetAttachBehavior(BindableObject view)
        {
            return (bool)view.GetValue(AttachBehaviorProperty);
        }
        public static void SetAttachBehavior(BindableObject view, bool value)
        {
            view.SetValue(AttachBehaviorProperty, value);
        }
        static void OnAttachBehaviorChanged(BindableObject view, object oldValue, object newValue)
        {
            Editor editor = view as Editor;
            if (editor == null)
            {
                return;
            }
            bool attachBehavior = (bool)newValue;
            if (attachBehavior)
            {
                editor.TextChanged += OneditorTextChanged;
            }
            else
            {
                editor.TextChanged -= OneditorTextChanged;
            }
        }
        static void OneditorTextChanged(object sender, TextChangedEventArgs args)
        {
            double result;
            bool isValid = double.TryParse(args.NewTextValue, out result);
            ((Editor)sender).TextColor = isValid ? Colors.Black : Colors.Red;
        }
    }
}
