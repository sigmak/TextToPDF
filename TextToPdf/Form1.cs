using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TextToPdf
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private  void Form1_Load(object sender, EventArgs e)
        {
            this.Text = "Text To PDF"; //Form1 캡션 변경
        }

        //1.Text 파일 찾기
        private void button1_Click(object sender, EventArgs e)
        {
            //참고 : https://mainia.tistory.com/1906
            OpenFileDialog openFileDlg = new OpenFileDialog();
            openFileDlg.DefaultExt = "txt";
            openFileDlg.Filter = "Text Files(*.txt)|*.txt";
            openFileDlg.ShowDialog();
            if(openFileDlg.FileName.Length>0)
            {
                foreach (string filename in openFileDlg.FileNames)
                {
                    this.label1.Text = filename;
                }
            }
        }

        //2.Text To PDF  변환
        private void button2_Click(object sender, EventArgs e)
        {
            string pdfFileName = "";
            string textFilePath = "";
            string textFileName = "";
            if (label1.Text=="" || label1.Text=="파일없음")
            {
                return; //함수 탈출
            }

			//출처 : https://mainia.tistory.com/2349
            textFilePath = Path.GetDirectoryName(label1.Text); //경로 추출
            textFileName = Path.GetFileNameWithoutExtension(label1.Text); // 확장자제외한 파일명만

            pdfFileName = textFilePath + "\\"+ textFileName + ".pdf";
            //label2.Text = pdfFileName;
            //참고 : http://csharp.net-informations.com/file/txttopdf.htm
            try
            {
                string line = null;
                System.IO.TextReader readFile = new StreamReader(label1.Text);
                int yPoint = 0;

                PdfDocument pdf = new PdfDocument();
                pdf.Info.Title = textFileName; // "TXT to PDF";
                PdfPage pdfPage = pdf.AddPage();
                XGraphics graph = XGraphics.FromPdfPage(pdfPage);
                
                string GulimFont = Environment.GetFolderPath(Environment.SpecialFolder.System) + @"\..\Fonts\gulim.ttc";

                XFont font = new XFont(GulimFont, 20, XFontStyle.Regular);
                while(true)
                {
                    line = readFile.ReadLine();
                    if (line == null)
                    {
                        break;//  TODO: might not be correct. Was : Exit While
                    } else
                    {
                        graph.DrawString(line,
                            font,
                            XBrushes.Black,
                            new XRect(40, yPoint, pdfPage.Width.Point, pdfPage.Height.Point),
                            XStringFormats.TopLeft);
                        yPoint = yPoint + 40;
                    }
                }
                pdf.Save(pdfFileName);
                readFile.Close();
                readFile = null;
                Process.Start(pdfFileName);
            } catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }
    }
}
