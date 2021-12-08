using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace WpfApp1.Helpers
{
    public partial class NumberBox
    {
        private readonly Regex _nonNumberRegex = new Regex(@"[^0-9,.\b]+", RegexOptions.IgnorePatternWhitespace);

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex expression = _nonNumberRegex;
            e.Handled = expression.IsMatch(e.Text);
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if((Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)) && Keyboard.IsKeyDown(Key.V))
            {
                FilterViaRegex(e);
            }
        }


        private void FilterViaRegex(KeyEventArgs e)
        {
            var expression = _nonNumberRegex;
            e.Handled = expression.IsMatch(Clipboard.GetText());
        }
    }
}
