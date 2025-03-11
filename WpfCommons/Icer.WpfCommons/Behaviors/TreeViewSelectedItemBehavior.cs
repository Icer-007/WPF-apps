using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Icer.WpfCommons.Behaviors
{
    public class TreeViewSelectedItemBehavior : Behavior<TreeView>
    {
        public static readonly DependencyProperty SelectedItemProperty =
                    DependencyProperty.Register(
                        "SelectedItem",
                        typeof(object),
                        typeof(TreeViewSelectedItemBehavior),
                        new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        // Using a DependencyProperty as the backing store for SetSelectedItemCommand. This enables
        // animation, styling, binding, etc...
        public static readonly DependencyProperty SetSelectedItemCommandProperty =
            DependencyProperty.Register(
                "SetSelectedItemCommand",
                typeof(ICommand),
                typeof(TreeViewSelectedItemBehavior),
                new PropertyMetadata(null));

        public object SelectedItem
        {
            get => this.GetValue(SelectedItemProperty);
            set => this.SetValue(SelectedItemProperty, value);
        }

        public ICommand SetSelectedItemCommand
        {
            get => (ICommand)this.GetValue(SetSelectedItemCommandProperty);
            set => this.SetValue(SetSelectedItemCommandProperty, value);
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.SelectedItemChanged += this.AssociatedObject_SelectedItemChanged;
        }

        protected override void OnDetaching()
        {
            this.AssociatedObject.SelectedItemChanged -= this.AssociatedObject_SelectedItemChanged;
            base.OnDetaching();
        }

        private void AssociatedObject_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            this.SelectedItem = this.AssociatedObject.SelectedItem;
            this.SetSelectedItemCommand?.Execute(this.AssociatedObject.SelectedItem);
        }
    }
}
