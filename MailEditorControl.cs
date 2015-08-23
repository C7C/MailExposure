using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using mshtml;

namespace MailExposure
{
    public partial class MailEditorControl : UserControl
    {
        private IHTMLDocument2 doc;
        private string htmlValue;
        private bool isDesign;
        private bool setup;
        private bool updatingFontName;
        private bool updatingFontSize;
        private WebBrowserNavigatedEventHandler Navigated;
        private EventHandler<EnterKeyEventArgs> EnterKeyEvent;
        public delegate void TickDelegate();
        public event TickDelegate Tick;

        public string BodyHtml
        {
            get
            {
                if ((this.wbrContent1.Document != null) && (this.wbrContent1.Document.Body != null))
                {
                    return this.wbrContent1.Document.Body.InnerHtml;
                }
                return string.Empty;
            }
            set
            {
                if (this.wbrContent1.Document.Body != null)
                {
                    this.wbrContent1.Document.Body.InnerHtml = value;
                }
            }
        }

        public string BodyText
        {
            get
            {
                if ((this.wbrContent1.Document != null) && (this.wbrContent1.Document.Body != null))
                {
                    return this.wbrContent1.Document.Body.InnerText;
                }
                return string.Empty;
            }
            set
            {
                if (this.wbrContent1.Document.Body != null)
                {
                    this.wbrContent1.Document.Body.InnerText = value;
                }
            }
        }

        public HtmlDocument Document
        {
            get
            {
                return this.wbrContent1.Document;
            }
        }

        public string DocumentText
        {
            get
            {
                return this.wbrContent1.DocumentText;
            }
            set
            {
                this.wbrContent1.DocumentText = value;
            }
        }


        public Color EditorBackColor
        {
            get
            {
                if (this.ReadyState != ReadyState.Complete)
                {
                    return Color.White;
                }
                return ConvertToColor(this.doc.queryCommandValue("BackColor").ToString());
            }
            set
            {
                string str = string.Format("#{0:X2}{1:X2}{2:X2}", value.R, value.G, value.B);
                this.wbrContent1.Document.ExecCommand("BackColor", false, str);
            }
        }


        public Color EditorForeColor
        {
            get
            {
                if (this.ReadyState != ReadyState.Complete)
                {
                    return Color.Black;
                }
                return ConvertToColor(this.doc.queryCommandValue("ForeColor").ToString());
            }
            set
            {
                string str = string.Format("#{0:X2}{1:X2}{2:X2}", value.R, value.G, value.B);
                this.wbrContent1.Document.ExecCommand("ForeColor", false, str);
            }
        }

        public FontFamily FontName
        {
            get
            {
                if (this.ReadyState != ReadyState.Complete)
                {
                    return null;
                }
                string name = this.doc.queryCommandValue("FontName") as string;
                if (name == null)
                {
                    return null;
                }
                return new FontFamily(name);
            }
            set
            {
                if (value != null)
                {
                    this.wbrContent1.Document.ExecCommand("FontName", false, value.Name);
                }
            }
        }

        public FontSize FontSize
        {
            get
            {
                if (this.ReadyState == ReadyState.Complete)
                {
                    switch ((String)this.doc.queryCommandValue("FontSize").ToString())
                    {
                        case "1":
                            return FontSize.One;

                        case "2":
                            return FontSize.Two;

                        case "3":
                            return FontSize.Three;

                        case "4":
                            return FontSize.Four;

                        case "5":
                            return FontSize.Five;

                        case "6":
                            return FontSize.Six;

                        case "7":
                            return FontSize.Seven;
                    }
                }
                return FontSize.NA;
            }
            set
            {
                int num;
                switch (value)
                {
                    case FontSize.One:
                        num = 1;
                        break;

                    case FontSize.Two:
                        num = 2;
                        break;

                    case FontSize.Three:
                        num = 3;
                        break;

                    case FontSize.Four:
                        num = 4;
                        break;

                    case FontSize.Five:
                        num = 5;
                        break;

                    case FontSize.Six:
                        num = 6;
                        break;

                    case FontSize.Seven:
                        num = 7;
                        break;

                    default:
                        num = 7;
                        break;
                }
                this.wbrContent1.Document.ExecCommand("FontSize", false, num.ToString());
            }
        }

        public SelectionType SelectionType
        {
            get
            {
                switch (this.doc.selection.type.ToLower())
                {
                    case "text":
                        return SelectionType.Text;

                    case "control":
                        return SelectionType.Control;

                    case "none":
                        return SelectionType.None;
                }
                return SelectionType.None;
            }
        }

        private ReadyState ReadyState
        {
            get
            {
                switch (this.doc.readyState.ToLower())
                {
                    case "uninitialized":
                        return ReadyState.Uninitialized;

                    case "loading":
                        return ReadyState.Loading;

                    case "loaded":
                        return ReadyState.Loaded;

                    case "interactive":
                        return ReadyState.Interactive;

                    case "complete":
                        return ReadyState.Complete;
                }
                return ReadyState.Uninitialized;
            }
        }

       
        public MailEditorControl()
        {
            InitializeComponent();
            this.isDesign = true;
            this.htmlValue = string.Empty;
            this.SetupTimer();
            this.SetupBrowser();
            this.SetupFontComboBox();
            this.SetupFontSizeComboBox();
            (this.doc as HTMLDocumentEvents2_Event).onclick += (new HTMLDocumentEvents2_onclickEventHandler(this.DocEvents_onclick));
        }
        private void SetupFontComboBox()
        {
            AutoCompleteStringCollection strings = new AutoCompleteStringCollection();
            foreach (FontFamily family in FontFamily.Families)
            {
                this.fontComboBox.Items.Insert(0, (family.Name));
                strings.Add(family.Name);
            }
            this.fontComboBox.Leave += new EventHandler(this.fontComboBox_TextChanged);
            this.fontComboBox.AutoCompleteMode = AutoCompleteMode.Suggest;
            this.fontComboBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
            this.fontComboBox.AutoCompleteCustomSource = strings;
        }

        private void SetupTimer()
        {
            this.timer.Interval = 200;
            this.timer.Tick += new EventHandler(this.timer_Tick);
            this.timer.Start();
        }

        private void SetupFontSizeComboBox()
        {
            for (int i = 1; i <= 7; i++)
            {
                this.fontSizeComboBox.Items.Add(i.ToString());
            }
            this.fontSizeComboBox.TextChanged += new EventHandler(this.fontSizeComboBox_TextChanged);
            this.fontSizeComboBox.KeyPress += new KeyPressEventHandler(this.fontSizeComboBox_KeyPress);
        }

        private void fontSizeComboBox_TextChanged(object sender, EventArgs e)
        {
            if (!this.updatingFontSize)
            {
                switch (this.fontSizeComboBox.Text.Trim())
                {
                    case "1":
                        this.FontSize = FontSize.One;
                        return;

                    case "2":
                        this.FontSize = FontSize.Two;
                        return;

                    case "3":
                        this.FontSize = FontSize.Three;
                        return;

                    case "4":
                        this.FontSize = FontSize.Four;
                        return;

                    case "5":
                        this.FontSize = FontSize.Five;
                        return;

                    case "6":
                        this.FontSize = FontSize.Six;
                        return;

                    case "7":
                        this.FontSize = FontSize.Seven;
                        return;
                }
                this.FontSize = FontSize.Seven;
            }
        }

        private void fontComboBox_TextChanged(object sender, EventArgs e)
        {
            if (!this.updatingFontName)
            {
                FontFamily family;
                try
                {
                    family = new FontFamily(this.fontComboBox.Text);
                }
                catch (Exception)
                {
                    this.updatingFontName = true;
                    this.fontComboBox.Text = this.FontName.GetName(0);
                    this.updatingFontName = false;
                    return;
                }
                this.FontName = family;
            }
        }

        private void fontSizeComboBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsNumber(e.KeyChar))
            {
                e.Handled = true;
                if ((e.KeyChar <= '7') && (e.KeyChar > '0'))
                {
                    this.fontSizeComboBox.Text = e.KeyChar.ToString();
                }
            }
            else if (!char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            if (this.ReadyState == ReadyState.Complete)
            {
                this.SetupKeyListener();
                this.boldButton.Checked = this.IsBold();
                this.italicButton.Checked = this.IsItalic();
                this.underlineButton.Checked = this.IsUnderline();

                this.justifyLeftButton.Checked = this.IsJustifyLeft();
                this.justifyRightButton.Checked = this.IsJustifyRight();
                this.justifyCenterButton.Checked = this.IsJustifyCenter();
                this.justifyFullButton.Checked = this.IsJustifyFull();
                this.linkButton.Enabled = this.SelectionType == SelectionType.Text;
                this.UpdateFontComboBox();
                this.UpdateFontSizeComboBox();
                if (this.Tick != null)
                {
                    this.Tick();
                }
            }
        }

        private static Color ConvertToColor(string clrs)
        {
            int num;
            int num2;
            int num3;
            if (clrs.StartsWith("#"))
            {
                int num4 = Convert.ToInt32(clrs.Substring(1), 0x10);
                num = (num4 >> 0x10) & 0xff;
                num2 = (num4 >> 8) & 0xff;
                num3 = num4 & 0xff;
            }
            else
            {
                int num5 = Convert.ToInt32(clrs);
                num = num5 & 0xff;
                num2 = (num5 >> 8) & 0xff;
                num3 = (num5 >> 0x10) & 0xff;
            }
            return Color.FromArgb(num, num2, num3);
        }
        private bool IsBold()
        {
            return this.doc.queryCommandState("Bold");
        }

        private bool IsItalic()
        {
            return this.doc.queryCommandState("Italic");
        }

        private bool IsUnderline()
        {
            return this.doc.queryCommandState("Underline");
        }

        private bool IsJustifyLeft()
        {
            return this.doc.queryCommandState("JustifyLeft");
        }

        private bool IsJustifyRight()
        {
            return this.doc.queryCommandState("JustifyRight");
        }

        private bool IsJustifyCenter()
        {
            return this.doc.queryCommandState("JustifyCenter");
        }

        private bool IsJustifyFull()
        {
            return this.doc.queryCommandState("JustifyFull");
        }

        private void UpdateFontComboBox()
        {
            if (!this.fontComboBox.Focused)
            {
                FontFamily fontName = this.FontName;
                if (fontName != null)
                {
                    string name = fontName.Name;
                    if (name != this.fontComboBox.Text)
                    {
                        this.updatingFontName = true;
                        this.fontComboBox.Text = name;
                        this.updatingFontName = false;
                    }
                }
            }
        }

        private void UpdateFontSizeComboBox()
        {
            if (!this.fontComboBox.Focused)
            {
                int num;
                switch (this.FontSize)
                {
                    case FontSize.One:
                        num = 1;
                        break;

                    case FontSize.Two:
                        num = 2;
                        break;

                    case FontSize.Three:
                        num = 3;
                        break;

                    case FontSize.Four:
                        num = 4;
                        break;

                    case FontSize.Five:
                        num = 5;
                        break;

                    case FontSize.Six:
                        num = 6;
                        break;

                    case FontSize.Seven:
                        num = 7;
                        break;

                    case FontSize.NA:
                        num = 0;
                        break;

                    default:
                        num = 7;
                        break;
                }
                string str = Convert.ToString(num);
                if (str != this.fontSizeComboBox.Text)
                {
                    this.updatingFontSize = true;
                    this.fontSizeComboBox.Text = str;
                    this.updatingFontSize = false;
                }
            }
        }
        //**************************************************************************************************
        //**************************************************************************************************
        private void SetupKeyListener()
        {
            if (!this.setup)
            {
                this.wbrContent1.Document.Body.KeyDown += new HtmlElementEventHandler(this.Body_KeyDown);
                this.setup = true;
            }
        }

        private void Body_KeyDown(object sender, HtmlElementEventArgs e)
        {
            if ((e.KeyPressedCode == 13) && !e.ShiftKeyPressed)
            {
                bool cancel = false;
                if (this.EnterKeyEvent != null)
                {
                    EnterKeyEventArgs args = new EnterKeyEventArgs();
                    this.EnterKeyEvent(this, args);
                    cancel = args.Cancel;
                }
                e.ReturnValue = !cancel;
            }
        }

        private bool DocEvents_onclick(IHTMLEventObj pEvtObj)
        {
            IHTMLElement srcElement = pEvtObj.srcElement;
            return true;
        }
        //**************************************************************************************************
        //**************************************************************************************************

        private void SetupBrowser()
        {
            this.wbrContent1.DocumentText = "<html><body></body></html>";
            //  this.wbrConntent2.DocumentText = this.wbrContent1.DocumentText;
            this.wbrContent1.AllowNavigation = false;
            //this.wbrConntent2.AllowNavigation = false;
            this.doc = this.wbrContent1.Document.DomDocument as IHTMLDocument2;
            // this.doc2 = this.wbrConntent2.Document.DomDocument as IHTMLDocument2;
            this.doc.designMode = "On";
            //this.doc2.designMode = "On";
            this.wbrContent1.Document.ContextMenuShowing += new HtmlElementEventHandler(this.Document_ContextMenuShowing);
            //this.wbrConntent2.Navigated += new WebBrowserNavigatedEventHandler(this.wbrConntent2_Navigated);
        }

        private void Document_ContextMenuShowing(object sender, HtmlElementEventArgs e)
        {
            e.ReturnValue = false;
            this.cutToolStripMenuItem1.Enabled = this.CanCut();
            this.copyToolStripMenuItem2.Enabled = this.CanCopy();
            this.pasteToolStripMenuItem3.Enabled = this.CanPaste();
            this.deleteToolStripMenuItem.Enabled = this.CanDelete();
            this.cmsContent.Show(this, e.ClientMousePosition);
        }
        private bool CanCopy()
        {
            return this.doc.queryCommandEnabled("Copy");
        }

        private bool CanCut()
        {
            return this.doc.queryCommandEnabled("Cut");
        }

        private bool CanPaste()
        {
            return this.doc.queryCommandEnabled("Paste");
        }

        private bool CanDelete()
        {
            return this.doc.queryCommandEnabled("Paste");
        }

        public class EnterKeyEventArgs : EventArgs
        {
            // Fields
            private bool _cancel;
            // Properties
            public bool Cancel { get; set; }
            // Methods
            public EnterKeyEventArgs()
            {
            }
        }

        private bool ShowColorDialog(ref Color color)
        {
            bool flag = false;
            using (ColorDialog dialog = new ColorDialog())
            {
                dialog.SolidColorOnly = true;
                dialog.AllowFullOpen = false;
                dialog.AnyColor = false;
                dialog.FullOpen = false;
                dialog.CustomColors = null;
                dialog.Color = color;
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    color = dialog.Color;
                    flag = true;
                }
            }
            return flag;
        }

        private void ToolStripDesignType_Click(object sender, EventArgs e)
        {
            if (this.isDesign)
            {
                this.txtSource.Text = this.BodyHtml;
                this.txtSource.BringToFront();
                ToolStripItemCollection items = this.tspToolMenu.Items;
                for (int i = 0; i < items.Count; i++)
                {
                    items[i].Enabled = !this.isDesign;
                }
                this.ToolStripDesignType.Enabled = true;
            }
            else
            {
                this.BodyHtml = this.txtSource.Text;
                this.wbrContent1.BringToFront();
                ToolStripItemCollection items2 = this.tspToolMenu.Items;
                for (int j = 0; j < items2.Count; j++)
                {
                    items2[j].Enabled = !this.isDesign;
                }
            }
            this.isDesign = !this.isDesign;
        }

        private void boldButton_Click(object sender, EventArgs e)
        {
            this.wbrContent1.Document.ExecCommand("Bold", false, null);
        }

        private void italicButton_Click(object sender, EventArgs e)
        {
            this.wbrContent1.Document.ExecCommand("Italic", false, null);
        }

        private void underlineButton_Click(object sender, EventArgs e)
        {
            this.wbrContent1.Document.ExecCommand("Underline", false, null);
        }

        private void FontColor_Click(object sender, EventArgs e)
        {
            Color editorForeColor = this.EditorForeColor;
            if (this.ShowColorDialog(ref editorForeColor))
            {
                this.EditorForeColor = editorForeColor;
            }
        }

        private void BackgroundColor_Click(object sender, EventArgs e)
        {
            Color editorBackColor = this.EditorBackColor;
            if (this.ShowColorDialog(ref editorBackColor))
            {
                this.EditorBackColor = editorBackColor;
            }
        }

        private void linkButton_Click(object sender, EventArgs e)
        {
            this.wbrContent1.Document.ExecCommand("CreateLink", false, null);
        }

        private void InsertImage_Click(object sender, EventArgs e)
        {
            this.wbrContent1.Document.ExecCommand("InsertImage", true, null);
        }

        private void MailTextPrint_Click(object sender, EventArgs e)
        {
            this.wbrContent1.Document.ExecCommand("Print", true, null);
        }

        private void SaveMailText_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.AddExtension = true;
                dialog.DefaultExt = "html";
                dialog.Filter = "Html files(*.html;*html)|*.html;*html";
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    using (StreamWriter writer = new StreamWriter(dialog.FileName))
                    {
                        writer.Write(this.DocumentText);
                        writer.Close();
                    }
                }
            }
        }

        private void justifyLeftButton_Click(object sender, EventArgs e)
        {
            this.wbrContent1.Document.ExecCommand("JustifyLeft", false, null);
        }

        private void justifyCenterButton_Click(object sender, EventArgs e)
        {
            this.wbrContent1.Document.ExecCommand("JustifyCenter", false, null);
        }

        private void justifyRightButton_Click(object sender, EventArgs e)
        {
            this.wbrContent1.Document.ExecCommand("JustifyRight", false, null);
        }

        private void justifyFullButton_Click(object sender, EventArgs e)
        {
            this.wbrContent1.Document.ExecCommand("JustifyFull", false, null);
        }

        private void indentButton_Click(object sender, EventArgs e)
        {
            this.wbrContent1.Document.ExecCommand("Indent", false, null);
        }

        private void outdentButton_Click(object sender, EventArgs e)
        {
            this.wbrContent1.Document.ExecCommand("Outdent", false, null);
        }

        private void cutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.wbrContent1.Document.ExecCommand("Cut", false, null);
        }

        private void copyToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            this.wbrContent1.Document.ExecCommand("Copy", false, null);
        }

        private void pasteToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            this.wbrContent1.Document.ExecCommand("Paste", false, null);
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.wbrContent1.Document.ExecCommand("Delete", false, null);
        }
    }
}
