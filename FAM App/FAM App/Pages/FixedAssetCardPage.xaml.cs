using System;
using System.Collections.Generic;
using System.Data;
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
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Win32;

namespace FAM_App.Pages
{
    /// <summary>
    /// Logika interakcji dla klasy FixedAssetCardPage.xaml
    /// </summary>
    public partial class FixedAssetCardPage : Page
    {
        DataTable fixedAsset = new DataTable("emp");
        DataTable history = new DataTable("emp");
        public FixedAssetCardPage()
        {
            InitializeComponent();
        }

        private void ShowHistory_Button_Click(object sender, RoutedEventArgs e)
        {

            string inventoryNumber = InventoryNumber_TxtBox.Text;
            DataBase dataBase = new DataBase();

            GetFixedAsset(inventoryNumber, dataBase);
            GetAssetHistory(inventoryNumber, dataBase);
            ClearTextBoxes();

        }

        private void GetFixedAsset(string inventoryNumber, DataBase dataBase)
        {
            String query = "SELECT dbo.Srodek_Trwaly.Nr_Inwentarzowy, dbo.Srodek_Trwaly.Kod_Srodka, dbo.Srodek_Trwaly.Stan_Status, dbo.Srodek_Trwaly.Data_Nabycia, dbo.Srodek_Trwaly.Data_Likwidacji, dbo.Srodek_Trwaly.Data_Wprowadzenia, dbo.Srodek_Trwaly.Wartosc_Poczatkowa_Netto, dbo.Srodek_Trwaly.Wartosc_Poczatkowa_Brutto, dbo.Srodek_Trwaly.Faktura, dbo.Srodek_Trwaly.Gwarancja FROM dbo.Srodek_Trwaly WHERE Nr_Inwentarzowy = '" + inventoryNumber + "';";
            fixedAsset = dataBase.DataBaseShowSelectedData(fixedAsset, query);
            FixedAssetGrid.ItemsSource = fixedAsset.DefaultView;
            FixedAssetGrid.CanUserAddRows = false;
        }

        private void GetAssetHistory(string inventoryNumber, DataBase dataBase)
        {
            history = dataBase.DataBaseShowFixedAssetHistory(history, inventoryNumber);
            HistoryDataGrid.ItemsSource = history.DefaultView;
            HistoryDataGrid.CanUserAddRows = false;
        }

        private void ClearTextBoxes()
        {
            InventoryNumber_TxtBox.Clear();
        }

        private void ExportToPDF(DataTable fixedAsset, DataTable history, string fileName)
        {
            try
            {

                if (history.Rows.Count > 0)
                {
                    Font tableHeader = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 12, Font.BOLD);
                    Font cellFont = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 12, Font.NORMAL);

                    PdfPTable pdfPTable = new PdfPTable(fixedAsset.Columns.Count);
                    pdfPTable.DefaultCell.Padding = 2;
                    pdfPTable.WidthPercentage = 100;
                    pdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

                    foreach (DataColumn col in fixedAsset.Columns)
                    {
                        pdfPTable.AddCell(new Phrase(col.ColumnName,tableHeader));
                    }
                    foreach (DataRow row in fixedAsset.Rows)
                    {
                        for (int i = 0; i < fixedAsset.Columns.Count; i++)
                        {
                            pdfPTable.AddCell(new Phrase(row[i].ToString(), cellFont));
                        }
                    }

                    PdfPCell cell = new PdfPCell(new Phrase("--"));
                    cell.Border = 0;
                    cell.Colspan = fixedAsset.Columns.Count;
                    pdfPTable.AddCell(cell);

                    PdfPTable pdfPTable2 = new PdfPTable(history.Columns.Count);
                    pdfPTable2.DefaultCell.Padding = 2;
                    pdfPTable2.WidthPercentage = 100;
                    pdfPTable2.HorizontalAlignment = Element.ALIGN_LEFT;

                    foreach (DataColumn col in history.Columns)
                    {
                        pdfPTable2.AddCell(new Phrase(col.ColumnName, tableHeader));
                    }
                    foreach (DataRow row in history.Rows)
                    {
                        for (int i = 0; i < history.Columns.Count; i++)
                        {
                            pdfPTable2.AddCell(new Phrase(row[i].ToString(), cellFont));
                        }
                    }


                    FileStream myStream;
                    SaveFileDialog saveFileDialog1 = new SaveFileDialog();

                    saveFileDialog1.Filter = "PDF files (*.pdf)|*.pdf|All files (*.*)|*.*";
                    saveFileDialog1.FilterIndex = 2;
                    saveFileDialog1.RestoreDirectory = true;
                    saveFileDialog1.FileName = fileName;
                    bool errorMessage = false;
                    if (saveFileDialog1.ShowDialog() == true)
                    {
                        if (File.Exists(saveFileDialog1.FileName))
                        {
                            try { File.Delete(saveFileDialog1.FileName); }
                            catch (Exception ex) { errorMessage = true; MessageBox.Show(ex.Message); }
                        }
                        if (!errorMessage)
                        {
                            try
                            {
                                string filename = saveFileDialog1.FileName;
                                MessageBox.Show(filename);
                                using (myStream = new FileStream(saveFileDialog1.FileName, FileMode.Create, FileAccess.Write, FileShare.Read))
                                {
                                    Document document = new Document(PageSize.A4.Rotate(), 8f, 16f, 16f, 8f);
                                    PdfWriter.GetInstance(document, myStream);
                                    document.Open();
                                    document.Add(pdfPTable);
                                    document.Add(pdfPTable2);
                                    document.Close();
                                    myStream.Close();
                                }
                            }
                            catch (Exception ex) { MessageBox.Show(ex.Message); }
                        }
                    }
                }else { MessageBox.Show("Nie wybrano danych!"); }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void ExportToPDF_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ExportToPDF(fixedAsset,history,"Nazwa");
            }
            catch(Exception ex) { MessageBox.Show(ex.Message); }
        }
    }
}
