using System.ComponentModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;

namespace SampleSurveyApp.Maui.Controls;

public partial class AnswerOptionCardView : ContentView
{
	public AnswerOptionCardView()
	{
		InitializeComponent();

        //this.checkMarkImage.PropertyChanged += checkMarkImageChanged;
    }

    //private void checkMarkImageChanged(object sender, PropertyChangedEventArgs e)
    //{
    //    StateTriggerBase stateTrigger = sender as StateTriggerBase;
    //    Debug.WriteLine($"Normal state active: {stateTrigger.IsActive}");
    //}

    //void OnNormalStateIsActiveChanged(object sender, EventArgs e)
    //{
    //    StateTriggerBase stateTrigger = sender as StateTriggerBase;
    //    Debug.WriteLine($"Normal state active: {stateTrigger.IsActive}");
    //}

    //void OnSelectedStateIsActiveChanged(object sender, EventArgs e)
    //{
    //    StateTriggerBase stateTrigger = sender as StateTriggerBase;
    //    Debug.WriteLine($"Selected state active: {stateTrigger.IsActive}");
    //}

}
