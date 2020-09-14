﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Recognition;
using System.Text;
using System.Threading;
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

namespace NotesApp.View
{
    /// <summary>
    /// Interaction logic for NotesWindow.xaml
    /// </summary>
    public partial class NotesWindow : Window
    {
        SpeechRecognitionEngine recognizer;

        public NotesWindow()
        {
            InitializeComponent();
            InitRecognizer();
        }

        private void InitRecognizer()
        {
            var currentCulture = (from r in SpeechRecognitionEngine.InstalledRecognizers() where r.Culture.Equals(Thread.CurrentThread.CurrentCulture) select r).FirstOrDefault();

            recognizer = new SpeechRecognitionEngine(currentCulture);

            GrammarBuilder builder = new GrammarBuilder();
            builder.AppendDictation();

            Grammar grammar = new Grammar(builder);
            recognizer.LoadGrammar(grammar);
            

            recognizer.SpeechRecognized += Recognizer_SpeechRecognized;
        }

        private void Recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string recognizedText = e.Result.Text;

            contentRichTestBox.Document.Blocks.Add(
                new Paragraph(new Run(recognizedText)));
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }


        private void ContentRichTestBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int ammountChars = (new TextRange(contentRichTestBox.Document.ContentStart, contentRichTestBox.Document.ContentEnd)).Text.Length;

            statusTextBlock.Text = $"Document length:{ammountChars} characters";
        }

        private void BoldButton_Click(object sender, RoutedEventArgs e)
        {
            bool isButtonEnabled = (sender as ToggleButton).IsChecked ?? false;

            if (isButtonEnabled)
            {
                //var textToBold = new TextRange(contentRichTestBox.Selection.Start, contentRichTestBox.Selection.End);
                contentRichTestBox.Selection.ApplyPropertyValue(Inline.FontWeightProperty, FontWeights.Bold);
            }
            else
            {
                contentRichTestBox.Selection.ApplyPropertyValue(Inline.FontWeightProperty, FontWeights.Normal);
            }
        }



        private void ItalicButton_Click(object sender, RoutedEventArgs e)
        {

            bool isButtonEnabled = (sender as ToggleButton).IsChecked ?? false;

            if (isButtonEnabled)
            {
                //var textToBold = new TextRange(contentRichTestBox.Selection.Start, contentRichTestBox.Selection.End);
                contentRichTestBox.Selection.ApplyPropertyValue(Inline.FontStyleProperty, FontStyles.Italic);
            }
            else
            {
                contentRichTestBox.Selection.ApplyPropertyValue(Inline.FontStyleProperty, FontStyles.Normal);
            }
        }

        private void UnderlineButton_Click(object sender, RoutedEventArgs e)
        {

            bool isButtonEnabled = (sender as ToggleButton).IsChecked ?? false;

            if (isButtonEnabled)
            {
                
                contentRichTestBox.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, TextDecorations.Underline);
            }
            else
            {
                contentRichTestBox.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, TextDecorations.Baseline);
            }
        }

        private void SpeechButton_Click(object sender, RoutedEventArgs e)
        {
            bool isButtonEnabled = (sender as ToggleButton).IsChecked ?? false ;

            if (isButtonEnabled)
            {
                recognizer.SetInputToDefaultAudioDevice();
                recognizer.RecognizeAsync(RecognizeMode.Multiple);
            }
            else
            {
                recognizer.RecognizeAsyncStop();
            }

        }

        private void ContentRichTestBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            var selectedWeight = contentRichTestBox.Selection.GetPropertyValue(Inline.FontWeightProperty);
            var selectedStyle = contentRichTestBox.Selection.GetPropertyValue(Inline.FontStyleProperty);
            var selectedTextDec = contentRichTestBox.Selection.GetPropertyValue(Inline.TextDecorationsProperty);

            boldButton.IsChecked = (selectedWeight != DependencyProperty.UnsetValue) && (selectedWeight.Equals(FontWeights.Bold));
            italicButton.IsChecked = (selectedStyle != DependencyProperty.UnsetValue) && (selectedStyle.Equals(FontStyles.Italic));
            underlineButton.IsChecked = (selectedStyle != DependencyProperty.UnsetValue) && (selectedTextDec.Equals(TextDecorations.Underline));

        }
    }
}
