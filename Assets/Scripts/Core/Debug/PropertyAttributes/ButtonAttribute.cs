using System;

[AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
public class ButtonAttribute : Attribute
{
    public string buttonText { get; }

    public ButtonAttribute(string text = null)
    {
        buttonText = text;
    }
}
