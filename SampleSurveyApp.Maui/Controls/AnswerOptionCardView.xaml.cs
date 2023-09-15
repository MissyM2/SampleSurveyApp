namespace SampleSurveyApp.Maui.Controls;

public partial class AnswerOptionCardView : ContentView
{
	public AnswerOptionCardView()
	{
		InitializeComponent();
	}

    void OnNormalStateIsActiveChanged(object sender, EventArgs e)
    {
        StateTriggerBase stateTrigger = sender as StateTriggerBase;
        Console.WriteLine($"Normal state active: {stateTrigger.IsActive}");
    }

    void OnSelectedStateIsActiveChanged(object sender, EventArgs e)
    {
        StateTriggerBase stateTrigger = sender as StateTriggerBase;
        Console.WriteLine($"Selected state active: {stateTrigger.IsActive}");
    }

}
