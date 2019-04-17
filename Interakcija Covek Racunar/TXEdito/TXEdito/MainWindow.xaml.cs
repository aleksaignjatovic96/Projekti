using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using System.Text.RegularExpressions;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            cmbFontFamily.ItemsSource = Fonts.SystemFontFamilies.OrderBy(f => f.Source);
            rtbEditor.Focus();

            List<String> colors = new List<string>();
            colors.Add("Black");
            colors.Add("Blue");
            colors.Add("Red");
            colors.Add("Green");
            colors.Add("Yellow");
            colors.Add("Brown");
            colors.Add("Navy");
            colors.Add("Olive");
            colors.Add("Pink");
            colors.Add("Salmon");
            colors.Add("Violet");
            DataContext = colors;

            cmbSize.ItemsSource = new List<double>() { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };

            cmbSize.SelectedIndex = 5;


            cmbFontFamily.SelectedItem = rtbEditor.FontFamily;
            labelWord.Content = "Words: 0";
        }

        private void toolBarMain_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {

            // mainToolBar – instance of toolbar defined in XAML view

            foreach (FrameworkElement a in toolbar1.Items)
            {
                ToolBar.SetOverflowMode(a, OverflowMode.Never);

            }

            var overflowGrid = toolbar1.Template.FindName("OverflowGrid", toolbar1) as FrameworkElement;

            if (overflowGrid != null)
            {

                overflowGrid.Visibility =  Visibility.Collapsed;

            }

        }
        private void rtbEditor_TextChanged(object sender, TextChangedEventArgs e)
        {



            TextRange range = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);

            string allText = range.Text;

            MatchCollection wordColl = Regex.Matches(allText, @"[\W]+");
            
            
            labelWord.Content = "Words: " + wordColl.Count.ToString();

            if (allText == "\r\n") labelWord.Content = "Words: 0";


        }
        private void rtbEditor_SelectionChanged(object sender, RoutedEventArgs e)
        {
            object temp = rtbEditor.Selection.GetPropertyValue(Inline.FontWeightProperty);
            btnBold.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(FontWeights.Bold));

            temp = rtbEditor.Selection.GetPropertyValue(Inline.FontStyleProperty);
            btnItalic.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(FontStyles.Italic));

            temp = rtbEditor.Selection.GetPropertyValue(Inline.TextDecorationsProperty);
            btnUnderline.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(TextDecorations.Underline));

            temp = rtbEditor.Selection.GetPropertyValue(Inline.ForegroundProperty);
            
            if (temp != DependencyProperty.UnsetValue)
            {
                Color color = (Color)ColorConverter.ConvertFromString(temp.ToString());
                cmbColor.SelectedItem = GetColorName(color);
            }

            temp = rtbEditor.Selection.GetPropertyValue(Inline.FontSizeProperty);
            cmbSize.SelectedItem = temp;

            temp = rtbEditor.Selection.GetPropertyValue(Inline.FontFamilyProperty);
            cmbFontFamily.SelectedItem = temp;
        }

        static string GetColorName(Color col)
        {
            System.Reflection.PropertyInfo colorProperty = typeof(Colors).GetProperties()
                .FirstOrDefault(p => Color.AreClose((Color)p.GetValue(null), col));
            return colorProperty != null ? colorProperty.Name : "unnamed color";
        }

        private void cmbFontFamily_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (cmbFontFamily.SelectedItem != null)
            //  if (!rtbEditor.Selection.IsEmpty)
                 rtbEditor.Selection.ApplyPropertyValue(RichTextBox.FontFamilyProperty, cmbFontFamily.SelectedItem);


            rtbEditor.Focus();
        }

        private void cmbColor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbColor.SelectedItem != null)
                rtbEditor.Selection.ApplyPropertyValue(ForegroundProperty, cmbColor.SelectedItem);

            rtbEditor.Focus();
        }    
        private void cmbSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbSize.SelectedItem != null)
                rtbEditor.Selection.ApplyPropertyValue(Inline.FontSizeProperty, cmbSize.SelectedItem);

            rtbEditor.Focus();
        }
        
        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            if (textExist() > 0)
               rtbEditor.Document.Blocks.Clear();

            rtbEditor.Focus();
        }

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            if (textExist() > 0)
            {
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.Filter = "rtf files (*.rtf)|*.rtf|txt files (*.txt)|*.txt|All files (*.*)|*.*";

                if (dlg.ShowDialog() == true)
                {
                    FileStream fs = new FileStream(dlg.FileName, FileMode.Open);
                    TextRange range = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);
                    range.Load(fs, DataFormats.Rtf);
                    fs.Close();
                }
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            save();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            if (textExist() > 0)
               this.Close();
        }


        private int textExist()
        {

            TextRange text = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);

            if (text.Text != "" && text.Text != "\r\n")
            {
                MessageBoxResult result = MessageBox.Show("Do you want to save?", "Warning", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    if (save())
                      return 1;
                }
                else if (result == MessageBoxResult.No)
                {
                    return 2;
                }

                return 0;

            }

            return 1;
        }
        private bool save()
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "rtf files (*.rtf)|*.rtf|txt files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == true)
            {
                FileStream fileStream = new FileStream(saveFileDialog1.FileName, FileMode.Create);
                TextRange range = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);
                range.Save(fileStream, DataFormats.Rtf);
                fileStream.Close();
                return true;
            }

            return false;
        }

        private void mnuinsert1_Click(object sender, RoutedEventArgs e)
        {

            rtbEditor.CaretPosition.InsertTextInRun(DateTime.Now.ToString());
            rtbEditor.CaretPosition = rtbEditor.CaretPosition.DocumentEnd;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F5)
            {
                mnuinsert1_Click(sender, e);
            }
            else
            {
                if (!txtReplace.IsFocused)
                {
                    if (!txtFind.IsFocused )
                    {
                        if (!rtbEditor.IsFocused)
                        {
                            rtbEditor.Focus();
                        }
                    }
                }
            }
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }





        public TextRange FindTextInRange(TextRange searchRange, string searchText)
        {
            int offset = searchRange.Text.IndexOf(searchText, StringComparison.OrdinalIgnoreCase);
            if (offset < 0)
                return null;  // Not found

            var start = GetTextPositionAtOffset(searchRange.Start, offset);
            TextRange result = new TextRange(start, GetTextPositionAtOffset(start, searchText.Length));

            return result;
        }

        TextPointer GetTextPositionAtOffset(TextPointer position, int characterCount)
        {
            while (position != null)
            {
                if (position.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.Text)
                {
                    int count = position.GetTextRunLength(LogicalDirection.Forward);
                    if (characterCount <= count)
                    {
                        return position.GetPositionAtOffset(characterCount);
                    }

                    characterCount -= count;
                }

                TextPointer nextContextPosition = position.GetNextContextPosition(LogicalDirection.Forward);
                if (nextContextPosition == null)
                    return position;

                position = nextContextPosition;
            }

            return position;
        }


        private void txtFind_SelectionChanged(object sender, RoutedEventArgs e)
        {
            TextRange searchRange = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);          
            searchRange.ApplyPropertyValue(TextElement.BackgroundProperty, new SolidColorBrush(Colors.Transparent));

            if (txtFind.Text == "")
                return;


            TextRange foundRange = FindTextInRange(searchRange, txtFind.Text);

            while (foundRange != null)
            {

                foundRange.ApplyPropertyValue(TextElement.BackgroundProperty, new SolidColorBrush(Colors.Red));

                searchRange = new TextRange(foundRange.End, rtbEditor.Document.ContentEnd);

                foundRange = FindTextInRange(searchRange, txtFind.Text);

            }
        }

        private void btnReplace_Click(object sender, RoutedEventArgs e)
        {

            if (txtFind.Text == "" || txtReplace.Text == "" || txtReplace.Text == txtFind.Text)
                return;

            TextRange searchRange = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);

            TextRange foundRange = FindTextInRange(searchRange, txtFind.Text);

            while (foundRange != null)
            {

                foundRange.Text = txtReplace.Text;

                //foundRange.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold );

                searchRange = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);

                foundRange = FindTextInRange(searchRange, txtFind.Text);

            }
        }










    }
}
