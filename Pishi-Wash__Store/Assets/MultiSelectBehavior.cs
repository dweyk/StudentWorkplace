namespace StudentWorkplace.Assets;

using System.Collections;

using DevExpress.Mvvm.UI.Interactivity;

using Microsoft.VisualBasic;

public class MultiSelectBehavior : Behavior<ListView>
{
	public static readonly DependencyProperty SelectedItemsProperty =
		DependencyProperty.Register(nameof(SelectedItems), typeof(Collection), typeof(MultiSelectBehavior),
			new PropertyMetadata(null));

	public Collection SelectedItems
	{
		get { return (Collection)GetValue(SelectedItemsProperty); }
		set { SetValue(SelectedItemsProperty, value); }
	}

	protected override void OnAttached()
	{
		base.OnAttached();
		if (AssociatedObject != null)
		{
			AssociatedObject.SelectionChanged += OnSelectionChanged;
		}
	}

	protected override void OnDetaching()
	{
		if (AssociatedObject != null)
		{
			AssociatedObject.SelectionChanged -= OnSelectionChanged;
		}
		base.OnDetaching();
	}

	private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		if (SelectedItems != null!)
		{
			SelectedItems.Clear();
			foreach (var item in AssociatedObject.SelectedItems)
			{
				SelectedItems.Add(item);
			}
		}
	}
}