namespace VICTUZ_studievereniging_app.MainPages;

public class MyEventsPage : ContentPage
{
	public MyEventsPage()
	{
		Content = new VerticalStackLayout
		{
			Children = {
				new Label { HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center, Text = "Welcome to .NET MAUI!"
				}
			}
		};
	}
}