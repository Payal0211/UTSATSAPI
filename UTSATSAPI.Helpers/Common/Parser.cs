using GrapeCity.Documents.Pdf;
using iText.Html2pdf;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using Microsoft.Extensions.Configuration;
using System.IO.Packaging;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using UTSATSAPI.Models.Models;

namespace UTSATSAPI.Helpers.Common
{
    public class Parser
    {
        #region Variables
        IConfiguration _configuration;
        TalentConnectAdminDBContext _talentConnectAdminDBContext;
        #endregion

        #region Constructors
        public Parser(IConfiguration configuration, TalentConnectAdminDBContext talentConnectAdminDBContext)
        {
            _configuration = configuration;
            _talentConnectAdminDBContext = talentConnectAdminDBContext;
        }
        #endregion

        #region Public Methods


        public string ExtractPDFText(string filePath)
        {
            StringBuilder text = new StringBuilder();
            if (System.IO.File.Exists(filePath))
            {
                using (PdfReader reader = new PdfReader(filePath))
                {
                    for (int i = 1; i <= reader.NumberOfPages; i++)
                    {
                        text.Append(PdfTextExtractor.GetTextFromPage(reader, i));
                    }
                }
            }
            return text.ToString();
        }

        public string ExtractTextFromDocument(string filePath)
        {
            List<string> pairs = new List<string>();

            var wrdf = new Aspose.Words.Document(filePath);

            if (System.IO.File.Exists(System.IO.Path.Combine("Media/JDParsing/JDFiles", "output.docx")))
                System.IO.File.Delete(System.IO.Path.Combine("Media/JDParsing/JDFiles", "output.docx"));
            wrdf.Save(System.IO.Path.Combine("Media/JDParsing/JDFiles", "output.docx"), Aspose.Words.SaveFormat.Docx);



            string fileName = System.IO.Path.Combine("Media/JDParsing/JDFiles", "output.docx");

            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
            const string documentRelationshipType =
              "http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument";
            const string stylesRelationshipType =
              "http://schemas.openxmlformats.org/officeDocument/2006/relationships/styles";
            const string wordmlNamespace =
              "http://schemas.openxmlformats.org/wordprocessingml/2006/main";
            XNamespace w = wordmlNamespace;

            XDocument xDoc = null;
            XDocument styleDoc = null;

            using (System.IO.Packaging.Package wdPackage = Package.Open(fileName, FileMode.Open, FileAccess.Read))
            {
                PackageRelationship docPackageRelationship =
                  wdPackage
                  .GetRelationshipsByType(documentRelationshipType)
                  .FirstOrDefault();
                if (docPackageRelationship != null)
                {
                    Uri documentUri =
                        PackUriHelper
                        .ResolvePartUri(
                           new Uri("/", UriKind.Relative),
                                 docPackageRelationship.TargetUri);
                    PackagePart documentPart =
                        wdPackage.GetPart(documentUri);

                    //  Load the document XML in the part into an XDocument instance.
                    xDoc = XDocument.Load(XmlReader.Create(documentPart.GetStream()));

                    //  Find the styles part. There will only be one.
                    PackageRelationship styleRelation =
                      documentPart.GetRelationshipsByType(stylesRelationshipType)
                      .FirstOrDefault();
                    if (styleRelation != null)
                    {
                        Uri styleUri = PackUriHelper.ResolvePartUri(documentUri, styleRelation.TargetUri);
                        PackagePart stylePart = wdPackage.GetPart(styleUri);

                        //  Load the style XML in the part into an XDocument instance.
                        styleDoc = XDocument.Load(XmlReader.Create(stylePart.GetStream()));
                    }
                }
            }

            string defaultStyle =
                (string)(
                    from style in styleDoc.Root.Elements(w + "style")
                    where (string)style.Attribute(w + "type") == "paragraph" &&
                          (string)style.Attribute(w + "default") == "1"
                    select style
                ).First().Attribute(w + "styleId");

            // Find all paragraphs in the document.
            var paragraphs =
                from para in xDoc
                             .Root
                             .Element(w + "body")
                             .Descendants(w + "p")
                let styleNode = para
                                .Elements(w + "pPr")
                                .Elements(w + "pStyle")
                                .FirstOrDefault()

                select new
                {
                    ParagraphNode = para,
                    StyleName = styleNode != null ?
                        (string)styleNode.Attribute(w + "val") :
                        defaultStyle,
                };

            // Retrieve the text of each paragraph.
            var paraWithText =
                from para in paragraphs
                select new
                {
                    para.ParagraphNode,
                    para.StyleName,
                    Text = GoogleDriveParagraphText(para.ParagraphNode)+"\n",
                };

            string psAllText = "";
            foreach (var p in paraWithText)
            {
                if (!p.Text.Contains("Created with Aspose.Words. Copyright"))
                    psAllText += p.Text + " ";
            }


            if (System.IO.File.Exists(System.IO.Path.Combine("Media/JDParsing/JDFiles", "output.docx")))
                System.IO.File.Delete(System.IO.Path.Combine("Media/JDParsing/JDFiles", "output.docx"));

            return psAllText;
        }
        public string ExtractTextUsingPython(string psText)
        {
            string pythonURL = _configuration["PaythonURL"];
            string content;
            if (string.IsNullOrWhiteSpace(pythonURL) || string.IsNullOrWhiteSpace(psText))
                return null;


            System.IO.File.WriteAllText(System.IO.Path.Combine("Media/JDParsing/JDFiles", "File1.txt"), psText);

            using (WebResponse wr = WebRequest.Create(pythonURL).GetResponse())
            {
                using (StreamReader sr = new StreamReader(wr.GetResponseStream()))
                {
                    content = sr.ReadToEnd();
                }
            }

            if (System.IO.File.Exists(System.IO.Path.Combine("Media/JDParsing/JDFiles", "File1.txt")))
                System.IO.File.Delete(System.IO.Path.Combine("Media/JDParsing/JDFiles", "File1.txt"));

            return content;

        }

        public string CompareWordsWithSkills(string text)
        {
            string strText = "";

            text = Regex.Replace(text, @"[\d-]", string.Empty);

            var excludeWordFilePath = System.IO.Path.Combine("Media/JDParsing", "ExcludeWords.txt");

            string excludeWordContent = System.IO.File.ReadAllText(excludeWordFilePath);

            List<string> wordList = excludeWordContent.Split(',').ToList();
            List<string> FinalList = new List<string>();
            List<string> SkillList = text.Split(',').ToList();

            SkillList = SkillList != null && SkillList.Count > 0 ? SkillList.Select(t => t.Trim()).ToList() : null;

            List<string> NewSkillList = new List<string>();

            if (SkillList != null && SkillList.Count > 0)
            {
                foreach (string s in SkillList.ToList())
                {
                    List<string> split_skills = s.Trim().Split(' ').ToList();
                    int splitSkillsCount = split_skills.Count;
                    if (split_skills.Count > 1)
                    {
                        foreach (string sk in split_skills.ToList())
                        {
                            foreach (string word in wordList)
                            {
                                if (sk != "")
                                {
                                    if (sk.ToLower().Trim().Contains(word.ToLower().Trim()))
                                    {
                                        split_skills.RemoveAll(x => x.ToLower().Trim().Contains(sk.ToLower().Trim()));
                                    }
                                }
                            }
                        }
                        if (splitSkillsCount != split_skills.Count)
                        {
                            SkillList.RemoveAll(x => x.ToLower().Trim().Contains(s.ToLower().Trim()));
                            if (split_skills.Count > 0)
                            {
                                string FinalSkill = string.Join(" ", split_skills);
                                NewSkillList.Add(FinalSkill);
                            }
                        }
                    }
                    else
                    {
                        foreach (string word in wordList)
                        {
                            if (s.ToLower().Trim().Contains(word.ToLower().Trim()))
                            {
                                SkillList.RemoveAll(x => x.ToLower().Trim().Contains(s.ToLower().Trim()));
                            }
                        }
                    }
                }
            }

            if (NewSkillList.Count > 0)
            {
                SkillList = SkillList.Union(NewSkillList).ToList();
            }

            if (SkillList.Count > 0)
                SkillList = SkillList.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct(StringComparer.CurrentCultureIgnoreCase).ToList();

            strText = string.Join(",", SkillList);
            return strText;
        }

        public List<string> ExtractTextFromPDF(string filePath)
        {

            using (var fs = File.OpenRead(filePath))
            {
                StringBuilder sb = new StringBuilder();
                List<string> rawdata = new List<string>();
                //Load Sample PDF document  
                GcPdfDocument doc = new GcPdfDocument();
                doc.Load(fs);

                //Extract All text from PDF document  
                var text = doc.GetText();
                //Display the results:  
                sb.Append(text);

                rawdata.AddRange(sb.ToString().Replace("\r","").Split("\n").ToList());
                return rawdata;
            }

        }

        public List<string> ExtractRoleAndResponsbillity(List<string> paragraphList)
        {
            
            List<string> strReturnList = new List<string>();
            string[] rolesAndResponsilbities = _talentConnectAdminDBContext.PrgRolesAndResponsilbities.Select(p => p.RoleText.ToLower()).ToArray();

            if (paragraphList != null && paragraphList.Count > 0)
            {
                ///String[] lines = new String[] { string.Join(" ", paragraphList.ToArray()) };
                foreach (string line in paragraphList)
                {
                    if (!string.IsNullOrEmpty(line))
                    {
                        if (GetSearchWords(line, rolesAndResponsilbities))
                        {
                            strReturnList.Add(line.ToString());
                        }
                    }
                }
                return strReturnList;
            }
            else
            {
                return strReturnList;
            }

        }

        public List<string> ExtractRequirement(List<string> paragraphList)
        {

            List<string> strReturnList = new List<string>();
            string[] requirements = _talentConnectAdminDBContext.PrgRequirements.Select(p => p.ReqText.ToLower()).ToArray();

            if (paragraphList != null && paragraphList.Count > 0)
            {
                //String[] lines = new String[] { string.Join(" ", paragraphList.ToArray()) };
                foreach (string line in paragraphList)
                {
                    if (!string.IsNullOrEmpty(line))
                    {
                        if (GetSearchWords(line, requirements))
                        {
                            strReturnList.Add(line.ToString());
                        }
                    }
                }
                return strReturnList;
            }
            else
            {
                return strReturnList;
            }

        }

        public bool GetSearchWords(string text, string[] strData)
        {
            
            foreach (string word in strData)
            {
                if (Regex.IsMatch(text.ToLower(), word))
                {
                    return true;
                }
            }
            
            return false;
        }


        public bool GenerateJDPdf(string filePath, string htmlcontent)
        {
            try
            {
                iText.Kernel.Pdf.PdfDocument pdfDocument = new iText.Kernel.Pdf.PdfDocument(new iText.Kernel.Pdf.PdfWriter(filePath));

                // Convert HTML to PDF
                ConverterProperties converterProperties = new ConverterProperties();
                HtmlConverter.ConvertToPdf(htmlcontent, pdfDocument, converterProperties);

                // Close the PDF document
                pdfDocument.Close();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        #region Private Method

        private string GoogleDriveParagraphText(XElement e)
        {
            XNamespace w = e.Name.Namespace;
            return e
                   .Elements(w + "r")
                   .Elements(w + "t")
                   .StringConcatenate(element => (string)element);
        }



        #endregion

    }
}
