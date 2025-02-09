using EvernoteClone.ViewModel;
using EvernoteClone.ViewModel.Helpers;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EvernoteClone.View
{
    /// <summary>
    /// Interaction logic for NotesWindow.xaml
    /// </summary>
    public partial class NotesWindow : Window
    {
        NotesVM viewModel;
        public static string localNotes = System.IO.Path.Combine(Environment.CurrentDirectory, "notes");

        public NotesWindow()
        {
            InitializeComponent();
            Directory.CreateDirectory(localNotes);

            viewModel = Resources["vm"] as NotesVM;
            viewModel.SelectedNoteChanged += ViewModel_SelectedNoteChanged;

            var fontFamilies = Fonts.SystemFontFamilies.OrderBy(f => f.Source);
            fontFamilyComboBox.ItemsSource = fontFamilies;

            List<double> fontSizes = new List<double>() { 8, 9, 10, 11, 12, 14, 16, 28, 48, 72};
            fontSizeComboBox.ItemsSource = fontSizes;
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);

            if (string.IsNullOrEmpty(App.UserId))
            {
                LoginWindow loginWindow = new LoginWindow();
                loginWindow.ShowDialog();
                viewModel.GetNotebooks();
            }
        }

        private async void ViewModel_SelectedNoteChanged(object? sender, EventArgs e)
        {
            contentRichTextBox.Document.Blocks.Clear();
            if (viewModel.SelectedNote != null)
            {
                // When the selected note changes, get the newly selected note from the database and populate the rtf with the contents.
                if (!string.IsNullOrEmpty(viewModel.SelectedNote.FileLocation))
                {
                    string localFile = await AmazonS3Helper.retrieveFromS3(viewModel.SelectedNote.Id);

                    FileStream fs = new FileStream(localFile, FileMode.Open);
                    var contents = new TextRange(contentRichTextBox.Document.ContentStart, contentRichTextBox.Document.ContentEnd);
                    contents.Load(fs, DataFormats.Rtf);
                    fs.Close();
                }
            }
            
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void contentRichTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Update the amount of characters in the rtf.
            int numChars = (new TextRange(contentRichTextBox.Document.ContentStart, contentRichTextBox.Document.ContentEnd)).Text.Length;

            statusTextBlock.Text = $"Document Length: {numChars} characters";
        }

        private void boldButton_Click(object sender, RoutedEventArgs e)
        {
            bool isButtonChecked = (sender as ToggleButton).IsChecked ?? false;

            // If the bold button is checked, make the selection bold. Otherwise, unbold the selection.
            if (isButtonChecked)
            {
                contentRichTextBox.Selection.ApplyPropertyValue(Inline.FontWeightProperty, FontWeights.Bold);
            }
            else
            {
                contentRichTextBox.Selection.ApplyPropertyValue(Inline.FontWeightProperty, FontWeights.Normal);
            }
            
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            // Utilize Azure services for speech to text.
            var speechConfig = SpeechConfig.FromSubscription(Keys.speechRegion, Keys.speechKey);
            using (var audioConfig = AudioConfig.FromDefaultMicrophoneInput())
            {
                using (var recognizer = new SpeechRecognizer(speechConfig, audioConfig))
                {
                    var result = await recognizer.RecognizeOnceAsync();
                    contentRichTextBox.Document.Blocks.Add(new Paragraph(new Run(result.Text)));
                }
            }
        }

        private void italicButton_Click(object sender, RoutedEventArgs e)
        {
            bool isButtonEnabled = (sender as ToggleButton).IsChecked ?? false;

            // If the italic button is checked, make the selection italicized. Otherwise, make it normal.
            if (isButtonEnabled)
                contentRichTextBox.Selection.ApplyPropertyValue(Inline.FontStyleProperty, FontStyles.Italic);
            else
                contentRichTextBox.Selection.ApplyPropertyValue(Inline.FontStyleProperty, FontStyles.Normal);
        }

        private void underlineButton_Click(object sender, RoutedEventArgs e)
        {
            bool isButtonEnabled = (sender as ToggleButton).IsChecked ?? false;

            // If the underline button is checked, make the selection underlined. Otherwise, make it normal.
            if (isButtonEnabled)
                contentRichTextBox.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, TextDecorations.Underline);
            else
            {
                TextDecorationCollection textDecorations;
                (contentRichTextBox.Selection.GetPropertyValue(Inline.TextDecorationsProperty) as TextDecorationCollection).TryRemove(TextDecorations.Underline, out textDecorations);
                contentRichTextBox.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, textDecorations);
            }
        }

        private void contentRichTextBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            // Update the text style based on the buttons and combo box changes.
            var selectedWeight = contentRichTextBox.Selection.GetPropertyValue(Inline.FontWeightProperty);
            boldButton.IsChecked = (selectedWeight != DependencyProperty.UnsetValue) && (selectedWeight.Equals(FontWeights.Bold));

            var selectedStyle = contentRichTextBox.Selection.GetPropertyValue(Inline.FontStyleProperty);
            italicButton.IsChecked = (selectedStyle != DependencyProperty.UnsetValue) && (selectedStyle.Equals(FontStyles.Italic));

            var selectedDecoration = contentRichTextBox.Selection.GetPropertyValue(Inline.TextDecorationsProperty);
            underlineButton.IsChecked = (selectedDecoration != DependencyProperty.UnsetValue) && (selectedDecoration.Equals(TextDecorations.Underline));

            fontFamilyComboBox.SelectedItem = contentRichTextBox.Selection.GetPropertyValue(Inline.FontFamilyProperty);
            fontSizeComboBox.Text = (contentRichTextBox.Selection.GetPropertyValue(Inline.FontSizeProperty)).ToString();
        }

        private void fontFamilyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Change the selection to the chosen font.
            if (fontFamilyComboBox.SelectedItem != null)
            {
                contentRichTextBox.Selection.ApplyPropertyValue(Inline.FontFamilyProperty, fontFamilyComboBox.SelectedItem);
            }
        }

        private void fontSizeComboBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Change the selection to the chosen font size.
            contentRichTextBox.Selection.ApplyPropertyValue(Inline.FontSizeProperty, fontSizeComboBox.Text);
        }

        private async void saveButton_Click(object sender, RoutedEventArgs e)
        {
            // Save the file locally.
            string rtfFile = System.IO.Path.Combine([Environment.CurrentDirectory, "notes", $"{viewModel.SelectedNote.Id}.rtf"]);
            FileStream fs = new FileStream(rtfFile, FileMode.Create);
            TextRange contents = new TextRange(contentRichTextBox.Document.ContentStart, contentRichTextBox.Document.ContentEnd);
            contents.Save(fs, DataFormats.Rtf);
            fs.Close();

            // Upload the file to S3.
            string result = await AmazonS3Helper.uploadToS3(viewModel.SelectedNote.Id, contents.Text);
            viewModel.SelectedNote.FileLocation = result;
            await DatabaseHelper.Update(viewModel.SelectedNote);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // On application close, delete the local notes files downloaded from the database.
            Directory.Delete(localNotes, true);
        }
    }
}
