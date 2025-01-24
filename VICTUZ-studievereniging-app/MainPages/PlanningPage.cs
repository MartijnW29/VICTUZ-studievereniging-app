namespace VICTUZ_studievereniging_app.MainPages;

public class PlanningPage : ContentPage
{
	public PlanningPage()
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