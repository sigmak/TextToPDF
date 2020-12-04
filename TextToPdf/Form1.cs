using iTextSharp.text;
using iTextSharp.text.pdf;
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
            if (label1.Text == "" || label1.Text == "파일없음")
            {
                return; //함수 탈출
            }

            //출처 : https://mainia.tistory.com/2349
            textFilePath = Path.GetDirectoryName(label1.Text); //경로 추출
            textFileName = Path.GetFileNameWithoutExtension(label1.Text); // 확장자제외한 파일명만

            pdfFileName = textFilePath + "\\" + textFileName + ".pdf";
            label2.Text = pdfFileName;


            //출처 : https://www.c-sharpcorner.com/blogs/convert-text-document-to-pdf-file1
            //Read the Data from Input File
            //StreamReader rdr = new StreamReader(label1.Text);
            System.IO.TextReader readFile = new StreamReader(label1.Text);

            //Create a New instance on Document Class
            Document doc = new Document();

            //Create a New instance of PDFWriter Class for Output File
            PdfWriter.GetInstance(doc, new FileStream(pdfFileName, FileMode.Create));

            //Open the Document
            doc.Open();

            // 사용할 폰트의 경로를 가져옵니다. 여기서는 굴림체를 가져왔어요 ㅎ
            string GulimFont = Environment.GetFolderPath(Environment.SpecialFolder.System) + @"\..\Fonts\gulim.ttc";

            // 폰트의 경로를 iTextSharp의 FontFactory에 등록합니다.
            FontFactory.Register(GulimFont);

            // iTextSharp의 Font 객체를 생성합니다. 생성할 때에 폰트의 크기를 지정합니다.
            iTextSharp.text.Font HeaderFont = FontFactory.GetFont("바탕체", BaseFont.IDENTITY_H, 16);

            //System.Drawing.Font textFont = new System.Drawing.Font(baseFont, DEFAULT_FONTSIZE, Font.NORMAL, BaseColor.BLACK);
            //System.Drawing.Font headingFont = new System.Drawing.Font(baseFont, DEFAULT_HEADINGSIZE, Font.BOLD, BaseColor.BLACK);

            string line = null;
            while (true)
            {
                line = readFile.ReadLine();
                if (line == null)
                {
                    break;//  TODO: might not be correct. Was : Exit While
                }
                else
                {
                    Paragraph para = new Paragraph(line + "\n", FontFactory.GetFont("굴림체", BaseFont.IDENTITY_H, 14));
                    doc.Add(para);
                }
            }

            //Close the Document
            doc.Close();

            MessageBox.Show("Conversion Successful...");

            //Open the Converted PDF File
            System.Diagnostics.Process.Start(pdfFileName);



        }
    }
}
