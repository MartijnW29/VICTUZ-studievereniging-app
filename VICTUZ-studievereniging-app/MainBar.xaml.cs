namespace VICTUZ_studievereniging_app;

public partial class MainBar : TabbedPage
{
    public MainBar(int page = 2)
    {
        InitializeComponent();

        // Check of de gewenste index bestaat
        if (page >= 0 && page < Children.Count)
        {
            CurrentPage = Children[page];
        }
        else
        {
            CurrentPage = Children[2]; // Val terug op het eerste tabblad
        }
    }
}
