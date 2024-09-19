using Microsoft.Extensions.Configuration;
using System.Text;
using UTSATSAPI.Models.Models;
using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Data;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using UTSATSAPI.Helpers.Common;
using Microsoft.AspNetCore.Http.Internal;


namespace UTSATSAPI.Helpers.Common
{
    public class ParserNew : LocationTextExtractionStrategy
    {
        #region Variables
        IConfiguration _configuration;
        TalentConnectAdminDBContext _talentConnectAdminDBContext;
        private StringBuilder TextBuffer { get; set; }
        private List<FormattedTextChunk> formattedChunks = new List<FormattedTextChunk>();
        
        public ParserNew()
        {
            TextBuffer = new StringBuilder();
        }

        #endregion

        #region Constructors
        public ParserNew(IConfiguration configuration, TalentConnectAdminDBContext talentConnectAdminDBContext)
        {
            _configuration = configuration;
            _talentConnectAdminDBContext = talentConnectAdminDBContext;
        }
        public ParserNew(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        #endregion



        #region MyRegion
        public static void ParsingMethod(string pdffilePath)
        {
            string pdfPath = pdffilePath;
            List<FormattedTextChunk> formattedText = ExtractFormattedTextFromPdf(pdfPath);

            // Print extracted text with formatting information
            foreach (var chunk in formattedText)
            {
                Console.WriteLine($"Text: {chunk.Text}");
                Console.WriteLine($"Font: {chunk.FontName}, Size: {chunk.FontSize:F2}");
                Console.WriteLine($"Color: R:{chunk.Color.R:F2} G:{chunk.Color.G:F2} B:{chunk.Color.B:F2}");
                Console.WriteLine($"Position: X:{chunk.X:F2} Y:{chunk.Y:F2}");
                Console.WriteLine($"Is Bold: {chunk.IsBold}, Is Italic: {chunk.IsItalic}");
                Console.WriteLine($"Is Underline: {chunk.IsUnderline}");
                Console.WriteLine("--------------------");
            }
        }


        public override void EventOccurred(IEventData data, EventType type)
        {
            if (type == EventType.RENDER_TEXT)
            {
                TextRenderInfo renderInfo = (TextRenderInfo)data;
                string text = renderInfo.GetText();

                if (!string.IsNullOrWhiteSpace(text))
                {
                    var font = renderInfo.GetFont();
                    float fontSize = renderInfo.GetFontSize();
                    var fontName = font.GetFontProgram().GetFontNames().GetCidFontName;
                    var color = GetColor(renderInfo);
                    var baselinePoint = renderInfo.GetBaseline().GetStartPoint();

                    formattedChunks.Add(new FormattedTextChunk
                    {
                        Text = text,
                        FontName = fontName.ToString(),
                        FontSize = fontSize,
                        Color = color,
                        X = baselinePoint.Get(0),
                        Y = baselinePoint.Get(1),
                        IsBold = font.GetFontProgram().GetFontNames().IsBold(),
                        //IsItalic = fontName.ToLowerInvariant().Contains("italic"),
                        IsUnderline = IsUnderline(renderInfo)
                    });
                }
            }
            base.EventOccurred(data, type);
        }

        private Color GetColor(TextRenderInfo renderInfo)
        {
            var fillColor = renderInfo.GetFillColor();
            if (fillColor is DeviceRgb rgb)
            {
                return new Color(rgb.GetColorValue()[0], rgb.GetColorValue()[1], rgb.GetColorValue()[2]);
            }
            return Color.Black; // Default to black if color information is not available
        }

        private bool IsUnderline(TextRenderInfo renderInfo)
        {
            var gs = renderInfo.GetGraphicsState();
            return gs.GetTextRenderingMode() == 1 || gs.GetTextRenderingMode() == 3;
        }

        public List<FormattedTextChunk> GetFormattedText()
        {
            return formattedChunks;
        }

        private static List<FormattedTextChunk> ExtractFormattedTextFromPdf(string pdfPath)
        {
            List<FormattedTextChunk> formattedText = new List<FormattedTextChunk>();

            using (PdfReader pdfReader = new PdfReader(pdfPath))
            using (PdfDocument pdfDoc = new PdfDocument(pdfReader))
            {
                for (int page = 1; page <= pdfDoc.GetNumberOfPages(); page++)
                {
                    ParserNew strategy = new ParserNew();
                    PdfCanvasProcessor parser = new PdfCanvasProcessor(strategy);
                    parser.ProcessPageContent(pdfDoc.GetPage(page));

                    formattedText.AddRange(strategy.GetFormattedText());
                }
            }

            return formattedText;
        }
        #endregion

    }
}

public class FormattedTextChunk
{
    public string Text { get; set; }
    public string FontName { get; set; }
    public float FontSize { get; set; }
    public Color Color { get; set; }
    public float X { get; set; }
    public float Y { get; set; }
    public bool IsBold { get; set; }
    public bool IsItalic { get; set; }
    public bool IsUnderline { get; set; }
}

public class Color
{
    public float R { get; set; }
    public float G { get; set; }
    public float B { get; set; }

    public Color(float r, float g, float b)
    {
        R = r;
        G = g;
        B = b;
    }

    public static Color Black => new Color(0, 0, 0);
}



