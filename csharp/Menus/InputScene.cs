#if CLIENT || GAME
using Godot;
using System;

namespace ExtractIntoVoid.Menus;

public partial class InputScene : Control
{
    public Label Question;
	LineEdit Answer;
    Button ApplyButton;
    Button RightButton;
	Button LeftButton;
	Control Field;
	Control Buttons;

    Action leftAction;
    Action rightAction;
    Action<string> anwserDoneAction;

	public override void _Ready()
	{
		Question = GetNode<Label>("Margin/VBox/Question");
        Field = GetNode<Control>("Margin/VBox/Field");
        Answer = GetNode<LineEdit>("Margin/VBox/Field/LineEdit");
        ApplyButton = GetNode<Button>("Margin/VBox/Field/ApplyButton");

        Buttons = GetNode<Control>("Margin/VBox/Buttons");
        RightButton = GetNode<Button>("Margin/VBox/Buttons/RightButton");
        LeftButton = GetNode<Button>("Margin/VBox/Buttons/LeftButton");
        LeftButton.Pressed += LeftButton_Pressed;
        RightButton.Pressed += RightButton_Pressed;
        ApplyButton.Pressed += ApplyButton_Pressed;
    }

    private void ApplyButton_Pressed()
    {
        anwserDoneAction?.Invoke(Answer.Text);
        this.QueueFree();
    }

    private void RightButton_Pressed()
    {
        rightAction?.Invoke();
        this.QueueFree();
    }

    private void LeftButton_Pressed()
    {
        leftAction?.Invoke();
        this.QueueFree();
    }

    public void ShowButtonOnly()
	{
		Field.Hide();
		Buttons.Show();
    }

    public void ShowInputOnly()
    {
        Buttons.Hide();
        Field.Show();
    }

    public void ButtonCallback(Action rightButton, Action leftButton, Action<string> answerbutton)
    {
        leftAction = rightButton;
        rightAction = leftButton;
        anwserDoneAction = answerbutton;
    }

    public override void _ExitTree()
    {
        LeftButton.Pressed -= LeftButton_Pressed;
        RightButton.Pressed -= RightButton_Pressed;
        ApplyButton.Pressed -= ApplyButton_Pressed;
    }
}
#endif