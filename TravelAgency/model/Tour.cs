using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Windows.Controls;
using TravelAgency.DbAdapters;
using TravelAgency.view.pages;

namespace TravelAgency.model
{
    public class Tour
    {
        public int Id { get; set; }
        public DateTime DepartureDate { get; set; }
        public DateTime ArrivingDate { get; set; }
        public decimal BaseCost { get; set; }
        public decimal FlightCost { get; set; }
        public decimal FoodCost { get; set; }
        public Hotel Hotel { get; set; }
        public decimal MinPrice
        {
            get { return BaseCost + FlightCost; }
        }

        public Tour(int id, DateTime departureDate, DateTime arrivingDate, decimal baseCost, decimal flightCost, decimal foodCost, Hotel hotel)
        {
            Id = id;
            DepartureDate = departureDate;
            ArrivingDate = arrivingDate;
            BaseCost = baseCost;
            FlightCost = flightCost;
            FoodCost = foodCost;
            Hotel = hotel;
        }

        public Tour() { }
        
        public Tour(DataRow selectedTour)
        {
            this.Id = (int)selectedTour["tour_id"];
            DepartureDate = (DateTime)selectedTour["departure_date"];
            ArrivingDate = (DateTime)selectedTour["arriving_date"];
            BaseCost = (decimal)selectedTour["base_cost"];
            FlightCost = (decimal)selectedTour["flight_cost"];
            FoodCost = (decimal)selectedTour["food_cost"];
            if (selectedTour["hotel_id"].GetType() != typeof(DBNull))
                Hotel = HotelsAdapter.GetHotel((int)selectedTour["hotel_id"]);
        }

        public void FormTourMembers()
        {
            Document document = new Document(PageSize.LETTER, 10f, 10f, 30f, 10f);
            PdfWriter.GetInstance(document, new FileStream("reports/tourMembers.pdf", FileMode.Create));
            document.Open();
            BaseFont baseFont = BaseFont.CreateFont(@"C:\Windows\Fonts\arial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            Font font = new Font(baseFont, Font.DEFAULTSIZE, Font.NORMAL);
            DataTable tourMembers = new DataTable();
            ToursAdapter.FillClientsByTour(this, tourMembers);
            PdfPTable table = new PdfPTable(tourMembers.Columns.Count);
            table.SetWidths(new float[] { 0.5f, 3, 3, 3, 2, 2});

            PdfPCell cell = new PdfPCell(new Phrase("Тур: " + this.Id + "  " + this.Hotel.Name + ", " + this.Hotel.Country + ", " + this.Hotel.City, font));
            cell.Colspan = tourMembers.Columns.Count;
            cell.HorizontalAlignment = 1;

            cell.Border = 0;
            cell.Padding = 5;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Сформовано: " + DateTime.Now, font));
            cell.Colspan = tourMembers.Columns.Count;
            cell.HorizontalAlignment = 1;

            cell.Border = 0;
            cell.Padding = 10;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Дата відправлення: " + this.DepartureDate.ToShortDateString(), font));
            cell.Colspan = (int)(tourMembers.Columns.Count/2);
            cell.HorizontalAlignment = 1;

            cell.Border = 0;
            cell.Padding = 10;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Дата повернення: " + this.ArrivingDate.ToShortDateString(), font));
            cell.Colspan = tourMembers.Columns.Count - (int)(tourMembers.Columns.Count / 2);
            cell.HorizontalAlignment = 1;

            cell.Border = 0;
            cell.Padding = 10;
            table.AddCell(cell);

            for (int i = 0; i < tourMembers.Columns.Count; i++)
            {
                cell = new PdfPCell(new Phrase(tourMembers.Columns[i].ColumnName, font));
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                table.AddCell(cell);
            }

            for (int i = 0; i < tourMembers.Rows.Count; i++)
            {
                for (int j = 0; j < tourMembers.Columns.Count; j++)
                {
                    table.AddCell(new Phrase(tourMembers.Rows[i][j].ToString(), font));
                }
            }
            document.Add(table);
            document.Close();
            
        }

    }
}
