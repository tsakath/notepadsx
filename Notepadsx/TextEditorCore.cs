using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

namespace Notepadsx;

public partial class TextEditorCore : RichEditBox
{
    private const string RootGridName = "RootGrid";
    Grid _rootGrid;
    private const string LineHighlighterCanvasName = "LineHighlighterCanvas";
    Canvas _lineHighlighterCanvas;
    private const string LineHighlighterName = "LineHighlighter";
    Grid _lineHighlighter;

    public TextEditorCore()
    {
        this.DefaultStyleKey = typeof(TextEditorCore);

        SelectionChanged += TextEditorCore_SelectionChanged;
    }

    private void TextEditorCore_SelectionChanged(object sender, RoutedEventArgs e)
    {
        UpdateLineHighlighter();
    }

    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        _rootGrid = GetTemplateChild(RootGridName) as Grid;
        _lineHighlighterCanvas = GetTemplateChild(LineHighlighterCanvasName) as Canvas;
        _lineHighlighter = GetTemplateChild(LineHighlighterName) as Grid;

        UpdateLineHighlighter();
    }

    public void UpdateLineHighlighter()
    {
        Document.Selection.GetRect(Microsoft.UI.Text.PointOptions.ClientCoordinates,
            out Windows.Foundation.Rect selectionRect, out var _);

        var singleLineHeight = GetSingleLineHeight();
        var thickness = new Thickness(0.08 * singleLineHeight);

        var height = selectionRect.Height;

        if  (height < singleLineHeight) height = singleLineHeight;

        if (height < singleLineHeight * 1.5f)
        {
            _lineHighlighter.Height = height;
            _lineHighlighter.Width = Math.Clamp(_rootGrid.ActualWidth, 0, Double.PositiveInfinity);
            _lineHighlighter.Margin = new Thickness(0, selectionRect.Y + Padding.Top, 0, 0);
        }
    }

    public double GetSingleLineHeight()
    {
        Document.GetRange(0, 0).GetRect(Microsoft.UI.Text.PointOptions.ClientCoordinates, out var rect, out _);
        return rect.Height <= 0 ? 1.35 * FontSize : rect.Height;
    }
}
