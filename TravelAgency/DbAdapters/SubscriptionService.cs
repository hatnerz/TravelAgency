using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TravelAgency.model;
using System.Net;
using System.Net.Mail;
using MailKit.Net.Imap;
using MailKit.Search;
using MailKit;
using System.Windows.Interop;
using TravelAgency.view.pages;
using iTextSharp.text.pdf;
using static System.Net.Mime.MediaTypeNames;
using System.IO;

namespace TravelAgency.DbAdapters
{
    internal static class SubscriptionService
    {
        static public int GetLastId()
        {
            string sqlExpression = "SELECT CONVERT(int, IDENT_CURRENT('tour_subscription'))";
            using (SqlConnection connection = new SqlConnection(App.GetConnectionStringByName("DefaultConnection")))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                var a = command.ExecuteScalar();
                return (int)a;
            }
        }


        public static TourSubscription GetTourSubscription(int id)
        {
            TourSubscription tourSubscription = new TourSubscription();
            SqlConnection connection = new SqlConnection(App.GetConnectionStringByName("DefaultConnection"));
            string commandStr = "SELECT * FROM tour_subscription WHERE tour_subscription_id=" + id.ToString();
            SqlCommand command = new SqlCommand(commandStr, connection);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            tourSubscription.Id = (int)reader["tour_subscription_id"];
            tourSubscription.RegistrationDate = (DateTime)reader["registration_date"];
            tourSubscription.MaxPrice = (decimal)reader["max_price"];
            tourSubscription.City = reader["city"].ToString();
            tourSubscription.Country = reader["country"].ToString();
            tourSubscription.Client = ClientsAdapter.GetClient((int)reader["client_id"]);
            connection.Close();
            return tourSubscription;
        }

        static public void FillSubscriptionsByClient(Client client, DataTable subscriptionDataTable)
        {
            string sqlExpression = "SELECT * FROM tour_subscription WHERE client_id = " + client.Id;
            using (SqlConnection connection = new SqlConnection(App.GetConnectionStringByName("DefaultConnection")))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlDataAdapter oda = new SqlDataAdapter(command);
                subscriptionDataTable.Clear();
                oda.Fill(subscriptionDataTable);
                connection.Close();
            }
        }

        static public void FillSubscriptions(DataTable subscriptionDataTable)
        {
            string sqlExpression = "SELECT * FROM tour_subscription";
            using (SqlConnection connection = new SqlConnection(App.GetConnectionStringByName("DefaultConnection")))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlDataAdapter oda = new SqlDataAdapter(command);
                subscriptionDataTable.Clear();
                oda.Fill(subscriptionDataTable);
                connection.Close();
            }
        }

        static public void InsertSubscription(TourSubscription tourSubscription)
        {
            try
            {
                string sqlExpression =
               "INSERT INTO tour_subscription (registration_date, country, city, max_price, client_id) " +
               "VALUES (@registrationDate, @country, @city, @maxPrice, @clientId)";
                using (SqlConnection connection = new SqlConnection(App.GetConnectionStringByName("DefaultConnection")))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    command.Parameters.Add(new SqlParameter("@registrationDate", tourSubscription.RegistrationDate));
                    command.Parameters.Add(new SqlParameter("@country", tourSubscription.Country));
                    command.Parameters.Add(new SqlParameter("@city", tourSubscription.City));
                    command.Parameters.Add(new SqlParameter("@maxPrice", tourSubscription.MaxPrice));
                    command.Parameters.Add(new SqlParameter("@clientId", tourSubscription.Client.Id));
                    command.ExecuteNonQuery();
                }
                tourSubscription.Id = GetLastId();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        static public void DeleteSubscription(TourSubscription tourSubscription)
        {
            SqlConnection connection = new SqlConnection(App.GetConnectionStringByName("DefaultConnection"));
            string commandStr = "DELETE FROM tour_subscription WHERE tour_subscription_id = " + tourSubscription.Id;
            connection.Open();
            SqlCommand command = new SqlCommand(commandStr, connection);
            command.ExecuteNonQuery();
        }

        static public void DeleteSubscription(int tourSubscriptionId)
        {
            SqlConnection connection = new SqlConnection(App.GetConnectionStringByName("DefaultConnection"));
            string commandStr = "DELETE FROM tour_subscription WHERE tour_subscription_id = " + tourSubscriptionId;
            connection.Open();
            SqlCommand command = new SqlCommand(commandStr, connection);
            command.ExecuteNonQuery();
        }

        static public void SendSubscriptionToClient(TourSubscription tourSubscription)
        {
            MailAddress fromAdress = new MailAddress("tour_agency_nure@outlook.com", "TourAgency");
            MailAddress toAdress = new MailAddress(tourSubscription.Client.Email);
            MailMessage message = new MailMessage(fromAdress, toAdress);
            message.Subject = "Підписка на тур №" + tourSubscription.Id;
            message.Body = "Шановний клієнт!\nВи щойно оформили підписку на тур за вказаними критеріями:\nКраїна: " + tourSubscription.Country +
            "\nМісто: " + tourSubscription.City + "\nМаксимальна вартість: " + (tourSubscription.MaxPrice <= 0 ? tourSubscription.MaxPrice.ToString() : "без обмежень") + "\nЯк тільки тур за вказаними критеріями заявиться, ми обов'язково вас проінформуємо!";
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Host = "smtp-mail.outlook.com";
            smtpClient.Port = 587;
            smtpClient.EnableSsl = true;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.UseDefaultCredentials = true;
            smtpClient.Credentials = new NetworkCredential(fromAdress.Address, "rdf234bddDf");

            smtpClient.Send(message);
        }

        static public void SendSuccessToClient(Trip trip)
        {
            MailAddress fromAdress = new MailAddress("tour_agency_nure@outlook.com", "TourAgency");
            MailAddress toAdress = new MailAddress(trip.Client.Email);
            MailMessage message = new MailMessage(fromAdress, toAdress);
            message.Subject = "Успішне підтвердження бронювання";
            message.Body = "Вітаємо! Ви успішно забронювали тур (№" + trip.Tour.Id + ")\nДля оплати та обрання послуг для туру вам треба звернутися до менеджера.";
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Host = "smtp-mail.outlook.com";
            smtpClient.Port = 587;
            smtpClient.EnableSsl = true;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.UseDefaultCredentials = true;
            smtpClient.Credentials = new NetworkCredential(fromAdress.Address, "rdf234bddDf");

            smtpClient.Send(message);
        }


        static public void SendSubscriptionsByTour(Tour tour)
        {
            DataTable subscriptionDataTable = new DataTable();
            FillSubscriptions(subscriptionDataTable);

            MailAddress fromAdress = new MailAddress("tour_agency_nure@outlook.com", "TourAgency");
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Host = "smtp-mail.outlook.com";
            smtpClient.Port = 587;
            smtpClient.EnableSsl = true;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.UseDefaultCredentials = true;
            smtpClient.Credentials = new NetworkCredential(fromAdress.Address, "rdf234bddDf");
            int count = 0;
            foreach (DataRow subscriptionRow in subscriptionDataTable.Rows)
            {
                TourSubscription tourSubscription = new TourSubscription(subscriptionRow);
                if((tourSubscription.City == "" || tourSubscription.City == tour.Hotel.City) &&
                    (tourSubscription.Country == "" || tourSubscription.Country == tour.Hotel.Country) &&
                    (tourSubscription.MaxPrice <= tour.MinPrice || tourSubscription.MaxPrice <= 0))
                {
                    MailAddress toAdress = new MailAddress("vlcvik03@gmail.com");
                    MailMessage message = new MailMessage(fromAdress, toAdress);
                    message.Body = "Шановний клієнт. Ви оформлювали підписку на тур (№"+tourSubscription.Id+") за критеріями" +
                        "\nКраїна: " + tourSubscription.Country + "\nМісто: " + tourSubscription.City + "\nМаксимальна вартість: " + (tourSubscription.MaxPrice <= 0 ? tourSubscription.MaxPrice.ToString() : "без обмежень") + "\n" +
                        "Такий тур було знайдено!\n\nПропонуємо вам подорож:\n Готель: " + tour.Hotel.Name + ". " + tour.Hotel.City + ", " + tour.Hotel.Country +
                        "\nВартість: від " + tour.MinPrice + "\nІдентифікатор туру: " + tour.Id + "\n\nДля підтвердження бронювання відправте лист з текстом \"Підтверджую\". Для оплати та обрання послуг для туру вам треба буде звернутися до менеджера.";
                    message.Subject = "Тур за підпискою знайдено";
                    smtpClient.Send(message);
                    count++;
                }
            }
            MessageBox.Show("Було відправлено " + count + " електронних листів за підписками.");
        }

        static public void CheckSubsctiptionAccepts()
        {
            int count = 0;
            using (var imapClient = new ImapClient())
            {
                imapClient.Connect("outlook.office365.com", 993, true);
                imapClient.Authenticate("tour_agency_nure@outlook.com", "rdf234bddDf");

                imapClient.Inbox.Open(FolderAccess.ReadOnly);

                string path = "email_check.dat";
                string dateText = "";
                using (StreamReader writer = new StreamReader(path))
                {
                    dateText = writer.ReadLine();
                }
                DateTime lastEmailCheckDate = DateTime.Parse(dateText);

                var uids = imapClient.Inbox.Search(SearchQuery.SentSince(lastEmailCheckDate));

                var messages = imapClient.Inbox.Fetch(uids, MessageSummaryItems.Envelope | MessageSummaryItems.BodyStructure);
               
                if (messages != null && messages.Count > 0)
                {
                    foreach(var msg in messages)
                    {
                        var body = imapClient.Inbox.GetBodyPart(msg.UniqueId, msg.BodyParts.First());
                        var text = ((MimeKit.TextPart)body).Text;
                        if (msg.Envelope.Date >= lastEmailCheckDate && text.ToLower().Contains("підтверджую") && msg.IsReply)
                        {
                            int tourIdIndex = text.LastIndexOf("Ідентифікатор туру: ");
                            if (tourIdIndex != -1)
                            {
                                string tourIdStr = "";
                                char currentSymbol = text[tourIdIndex + "Ідентифікатор туру: ".Length];
                                int i = 0;
                                while (Char.IsDigit(currentSymbol))
                                {
                                    tourIdStr += currentSymbol;
                                    i++;
                                    currentSymbol = text[tourIdIndex + i + "Ідентифікатор туру: ".Length];
                                }
                                Tour tour = new Tour(Convert.ToInt32(tourIdStr));
                                int subscriptionIdIndex = text.LastIndexOf("тур (№");
                                string subscriptionIdStr = "";
                                currentSymbol = text[subscriptionIdIndex + "тур (№".Length];
                                i = 0;
                                while (Char.IsDigit(currentSymbol))
                                {
                                    subscriptionIdStr += currentSymbol;
                                    i++;
                                    currentSymbol = text[subscriptionIdIndex + i + "тур (№".Length];
                                }
                                TourSubscription tourSubscription = GetTourSubscription(Convert.ToInt32(subscriptionIdStr));
                                Trip trip = new Trip();
                                trip.Tour = tour;
                                trip.Client = tourSubscription.Client;
                                trip.RegistrationDate = DateTime.Now;
                                TripsAdapter.InsertTripWithoutSettings(trip);
                                SendSuccessToClient(trip);
                                count++;
                            }
                        }
                    }
                }
                MessageBox.Show("Було відправлено " + count + " листів з підтвердженням бронювання.");
                using (StreamWriter writer = new StreamWriter(path,false))
                {
                    writer.WriteLine(DateTime.Now);
                }

            }
        }
    }
}
