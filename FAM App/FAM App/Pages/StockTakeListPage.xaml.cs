using iTextSharp.text.pdf;
using iTextSharp.text;
using Microsoft.Win32;
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

namespace FAM_App.Pages
{
    /// <summary>
    /// Logika interakcji dla klasy StockTakeListPage.xaml
    /// </summary>
    public partial class StockTakeListPage : Page
    {
        DataTable stocktakeData = new DataTable();
        DataTable StocktakeList = new DataTable();
        public StockTakeListPage()
        {
            InitializeComponent();
        }

        private void StocktakeDateStart_Box_Loaded(object sender, RoutedEventArgs e)
        {
            DataBase dataBase = new DataBase();
            DataTable depreciations = new DataTable();
            depreciations = dataBase.DataBaseGetDepreciation(depreciations);

            StocktakeDateStart_Box.ItemsSource = depreciations.AsDataView();
            StocktakeDateStart_Box.DisplayMemberPath = "Data Rozpoczęcia";
            StocktakeDateStart_Box.SelectedValuePath = "ID_Inwentaryzacji";
        }

        private void ShowStocktakeList_Button_Click(object sender, RoutedEventArgs e)
        {
            if(StocktakeDateStart_Box.SelectedValue != null)
            {
                LoadStocktake();
            }
            else
            {
                MessageBox.Show("Nie wybrano daty");
            }
        }

        private void LoadStocktake()
        {
            try
            {
                String queryData = "SELECT FORMAT(dbo.Inwentaryzacja.Data_Rozpoczecia, 'yyyy-MM-dd') AS 'Data Rozpoczęcia', FORMAT(dbo.Inwentaryzacja.Data_Zakonczenia, 'yyyy-MM-dd') AS 'Data Zakończenia', dbo.Inwentaryzacja.Uwagi, CONCAT(dbo.Pracownik.Imie, ' ', dbo.Pracownik.Nazwisko) AS 'Osoba przeprowadzająca inwentaryzację' " +
                    "FROM dbo.Inwentaryzacja " +
                    "LEFT JOIN dbo.Inwentaryzacja_Srodek ON dbo.Inwentaryzacja.ID_Inwentaryzacji = dbo.Inwentaryzacja_Srodek.id_inwentaryzacji " +
                    "LEFT JOIN dbo.Srodek_Trwaly ON dbo.Inwentaryzacja_Srodek.id_srodka = dbo.Srodek_Trwaly.ID_Srodka " +
                    "INNER JOIN dbo.Pracownik ON dbo.Inwentaryzacja.id_pracownika = dbo.Pracownik.ID_Pracownika " +
                    "WHERE (dbo.Inwentaryzacja.Data_Rozpoczecia = '"+StocktakeDateStart_Box.Text+"');";
                String queryList = "SELECT dbo.Inwentaryzacja_Srodek.Potwierdzenie, dbo.Srodek_Trwaly.Kod_Srodka, dbo.Srodek_Trwaly.Nr_Inwentarzowy, dbo.Produkt.Nazwa, dbo.Produkt.Marka, dbo.Produkt.Model, dbo.Srodek_Trwaly.Stan_Status, dbo.Srodek_Trwaly.Opis " +
                    "FROM dbo.Inwentaryzacja " +
                    "LEFT JOIN dbo.Inwentaryzacja_Srodek ON dbo.Inwentaryzacja.ID_Inwentaryzacji = dbo.Inwentaryzacja_Srodek.id_inwentaryzacji " +
                    "LEFT JOIN dbo.Srodek_Trwaly ON dbo.Inwentaryzacja_Srodek.id_srodka = dbo.Srodek_Trwaly.ID_Srodka " +
                    "INNER JOIN dbo.Pracownik ON dbo.Inwentaryzacja.id_pracownika = dbo.Pracownik.ID_Pracownika INNER JOIN dbo.Produkt ON dbo.Srodek_Trwaly.id_produktu = dbo.Produkt.ID_Produktu " +
                    "WHERE (dbo.Inwentaryzacja.Data_Rozpoczecia = '"+StocktakeDateStart_Box.Text+"')";

                DataBase dataBase = new DataBase();
                stocktakeData = new DataTable();
                StocktakeList = new DataTable();
                stocktakeData = dataBase.DataBaseShowSelectedData(stocktakeData, queryData);
                StocktakeList = dataBase.DataBaseShowSelectedData(StocktakeList, queryList);

                StocktakeDataGrid.ItemsSource = stocktakeData.DefaultView;
                StocktakeDataGrid.CanUserAddRows = false;

                StocktakeListDataGrid.ItemsSource= StocktakeList.DefaultView;
                StocktakeListDataGrid.CanUserAddRows= false;
            }
            catch(Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void ExportToPDF_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ExportToPDF(stocktakeData, StocktakeList, "Nazwa");
            }
            catch(Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void ExportToPDF(DataTable stocktakeData, DataTable stocktakeList, string fileName)
        {
            try
            {
                if (stocktakeList.Rows.Count > 0 & stocktakeData.Rows.Count > 0)
                {
                    Font tableHeader = FontFactory.GetFont(FontFactory.TIMES_ROMAN, BaseFont.CP1250, 11, Font.BOLD);
                    Font cellFont = FontFactory.GetFont(FontFactory.TIMES_ROMAN, BaseFont.CP1250, 11, Font.NORMAL);

                    PdfPTable pdfPTable = new PdfPTable(stocktakeData.Columns.Count);
                    pdfPTable.DefaultCell.Padding = 2;
                    pdfPTable.WidthPercentage = 100;
                    pdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

                    foreach (DataColumn col in stocktakeData.Columns)
                    {
                        pdfPTable.AddCell(new Phrase(col.ColumnName, tableHeader));
                    }
                    foreach (DataRow row in stocktakeData.Rows)
                    {
                        for (int i = 0; i < stocktakeData.Columns.Count; i++)
                        {
                            pdfPTable.AddCell(new Phrase(row[i].ToString(), cellFont));
                        }
                    }

                    PdfPCell cell = new PdfPCell(new Phrase("--"));
                    cell.Border = 0;
                    cell.Colspan = stocktakeData.Columns.Count;
                    pdfPTable.AddCell(cell);

                    PdfPTable pdfPTable2 = new PdfPTable(stocktakeList.Columns.Count);
                    pdfPTable2.DefaultCell.Padding = 2;
                    pdfPTable2.WidthPercentage = 100;
                    pdfPTable2.HorizontalAlignment = Element.ALIGN_LEFT;

                    foreach (DataColumn col in stocktakeList.Columns)
                    {
                        pdfPTable2.AddCell(new Phrase(col.ColumnName, tableHeader));
                    }
                    foreach (DataRow row in stocktakeList.Rows)
                    {
                        for (int i = 0; i < stocktakeList.Columns.Count; i++)
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
                                    Document document = new Document(PageSize.A4, 8f, 8f, 16f, 16f);
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
                }
                else
                {
                    MessageBox.Show("Nie wybrano danych");
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
    }
}
