using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors;

namespace EvrotorgApp.Behaviors
{
    public class NumericTextBoxBehavior : Behavior<TextBox>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PreviewTextInput += OnPreviewTextInput;
            AssociatedObject.PreviewKeyDown += OnPreviewKeyDown;
            DataObject.AddPastingHandler(AssociatedObject, OnPaste);
        }

        private void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                var currentText = textBox.Text;

                if (e.Key is Key.Delete)
                {
                    string newText;

                    if (textBox.SelectionLength > 0)
                    {
                        newText = currentText.Remove(textBox.SelectionStart, textBox.SelectionLength);
                    }
                    else
                    {
                        if (textBox.SelectionStart == textBox.Text.Length)
                        {
                            return;
                        }

                        newText = currentText.Remove(textBox.SelectionStart, 1);
                    }

                    e.Handled = !IsValid(newText);
                }
                else if (e.Key is Key.Back)
                {
                    string newText;

                    if (textBox.SelectionLength > 0)
                    {
                        newText = currentText.Remove(textBox.SelectionStart, textBox.SelectionLength);
                    }
                    else
                    {
                        if (textBox.SelectionStart == 0)
                        {
                            return;
                        }

                        newText = currentText.Remove(textBox.SelectionStart - 1, 1);
                    }

                    e.Handled = !IsValid(newText);
                }
            }
        }

        private void OnPaste(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(DataFormats.Text))
            {
                var text = Convert.ToString(e.DataObject.GetData(DataFormats.Text));

                if (!IsValid(text))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }

        private void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                var currentText = textBox.Text;
                var newText = currentText.Insert(textBox.SelectionStart, e.Text);
                e.Handled = !IsValid(newText);
            }
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.PreviewTextInput -= OnPreviewTextInput;
            DataObject.RemovePastingHandler(AssociatedObject, OnPaste);
        }

        private bool IsValid(string newText)
        {
            if (string.IsNullOrEmpty(newText))
            {
                return true;
            }

            var isValidDecimal = decimal.TryParse(newText, out var value);
            return isValidDecimal && value >= 0;
        }
    }
}