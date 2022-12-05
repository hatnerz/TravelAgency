using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using TravelAgency.DbAdapters;

namespace TravelAgency.model
{
    public class Trip
    {
        public int Id { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string FoodType { get; set; }
        public string PlaneClass { get; set; }
        public Tour Tour { get; set; }
        public Client Client { get; set; }
        public Trip(int id, DateTime registrationDate, string foodType, string planeClass, Tour tour, Client client)
        {
            Id = id;
            RegistrationDate = registrationDate;
            FoodType = foodType;
            PlaneClass = planeClass;
            Tour = tour;
            Client = client;
        }

        public Trip(DataRow selectedTrip)
        {
            this.Id = (int)selectedTrip["trip_id"];
            RegistrationDate = (DateTime)selectedTrip["registration_date"];
            FoodType = selectedTrip["food_type"].ToString();
            PlaneClass = selectedTrip["plane_class"].ToString();
            Tour = new Tour((int)selectedTrip["tour_id"]);
            Client = ClientsAdapter.GetClient((int)selectedTrip["client_id"]);
        }
        public Trip()
        { }

        public void CreateTicket()
        {
            Document document = new Document();
            //Создаем объект записи пдф-документа в файл
            PdfWriter.GetInstance(document, new FileStream("reports/ticket.pdf", FileMode.Create));
            //Открываем документ
            document.Open();

            //Определение шрифта необходимо для сохранения кириллического текста
            //Иначе мы не увидим кириллический текст
            //Если мы работаем только с англоязычными текстами, то шрифт можно не указывать
            //BaseFont baseFont = BaseFont.CreateFont("C:\\Windows\\Fonts\\arial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            //new Font(Font.FontFamily.TIMES_ROMAN, 16, Font.NORMAL, new BaseColor(Color.Orange))));
            //Font font = new Font(baseFont, Font.DEFAULTSIZE, Font.NORMAL);
            BaseFont baseFont = BaseFont.CreateFont("C:/Windows/Fonts/arial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            Font font = new Font(baseFont, Font.DEFAULTSIZE, iTextSharp.text.Font.NORMAL);

            //Создаем объект таблицы и передаем в нее число столбцов таблицы из нашего датасета

            PdfPTable table = new PdfPTable(2);
            PdfPCell cell = new PdfPCell(new Phrase("Путівка 🏖", font)) { Colspan = 2, HorizontalAlignment = 1};
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("ПІБ: " + Client.FirstName + " " + Client.LastName + " " + Client.PatronymicName, font)) { Colspan = 2 };
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Країна: " + Tour.Hotel.Country, font));
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Місто: " + Tour.Hotel.City, font));
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Готель: " + Tour.Hotel.Name, font)) { Colspan = 2};
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Дата відправлення: " + Tour.DepartureDate.ToLongDateString(), font));
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Дата повернення: " + Tour.ArrivingDate.ToLongDateString(), font));
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Дата оформлення: " + RegistrationDate, font)) { Colspan = 2};
            table.AddCell(cell);

            document.Add(table);
            document.Close();
            //Добавим в таблицу общий заголовок
            //PdfPCell cell = new PdfPCell(new Phrase("БД " + fileName + ", таблица №" + (i + 1), font));

            //cell.Colspan = MyDataSet.Tables[i].Columns.Count;
            //cell.HorizontalAlignment = 1;
            //Убираем границу первой ячейки, чтобы балы как заголовок
            //cell.Border = 0;
            //table.AddCell(cell);
        }
    }
}
