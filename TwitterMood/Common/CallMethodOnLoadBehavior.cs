using System.Reflection;
using WinRtBehaviors;
using Windows.UI.Xaml;

namespace TwitterMood.Common
{
    public class CallMethodOnLoadBehavior : Behavior<FrameworkElement>
    {
        public string MethodName
        {
            get { return (string)GetValue(MethodNameProperty); }
            set { SetValue(MethodNameProperty, value); }
        }

        public static readonly DependencyProperty MethodNameProperty =
            DependencyProperty.Register("MethodName", typeof(string), typeof(CallMethodOnLoadBehavior), new PropertyMetadata(default(CallMethodOnLoadBehavior)));

        public object TargetObject
        {
            get { return (object)GetValue(TargetObjectProperty); }
            set { SetValue(TargetObjectProperty, value); }
        }

        public static readonly DependencyProperty TargetObjectProperty =
            DependencyProperty.Register("TargetObject", typeof(object), typeof(CallMethodOnLoadBehavior), new PropertyMetadata(default(CallMethodOnLoadBehavior)));

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            if (AssociatedObject == null || string.IsNullOrEmpty(MethodName) || TargetObject == null)
                return;

            MethodInfo methodInfo = TargetObject.GetType().GetTypeInfo().GetDeclaredMethod(MethodName);

            if (methodInfo != null)
                methodInfo.Invoke(TargetObject, null);
        }
    }
}