using ParnerDiscount.Models;
using ParnerDiscount.Services;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ParnerDiscount
{

    public partial class MainWindow : Window
    {
		public ObservableCollection<Partner> partners = new();
		public MainWindow()
        {
            InitializeComponent();
            try
            {
                LoadPartners();
            }
            catch (Exception ex)
            {
				MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
			}
        }

        private void LoadPartners()
        {
            try
            {
                using var Connection = new SqlConnection(DBContext.ConnectionString);
                Connection.Open();

				partners.Clear();
                string Query_getPartners = "Select Id, Name, Raiting, INN, PatrnerType, Email, PhoneNumber, Address, Director FROM Partner";
                using var Command = new SqlCommand(Query_getPartners, Connection);
                using var Reader = Command.ExecuteReader();
                var TempList = new List<Partner>();
                while (Reader.Read())
                {
					TempList.Add(new Partner
                    {
                        Id = Reader.GetInt32(0),
                        Name = Reader.GetString(1),
                        Raitning = Reader.GetString(2),
						PatrnerType = Reader.GetString(4),
                        INN = Reader.GetString(3),
                        PhoneNumber = Reader.GetString(6),
                        Email = Reader.GetString(5),
                        Address = Reader.GetString(7),
                        Director = Reader.GetString(8)
                    });
                }
				Reader.Close();

                foreach (var p in TempList)
                {
                    var salesQuery = @"SELECT SUM(pp.Count) from PartnerProduct pp join Product p ON pp.ProductId = p.Id where pp.PartnerId = @id";
                    using var salesCommand = new SqlCommand(salesQuery, Connection);
                    salesCommand.Parameters.AddWithValue("@id", p.Id);
                    var result = salesCommand.ExecuteScalar();
                    var totalSales = result != DBNull.Value ? Convert.ToDecimal(result) : 0;

                    p.TotalSales = totalSales;
                    p.Discount = PartnerService.GetDiscount(totalSales);
					partners.Add(p);    
                }

                if(partners.Count > 0)
                {
					PartnerListView.ItemsSource = partners;
                }
            }
            catch (Exception ex)
            {
				MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
			}
        }

		private void ButtonEdit_Click(object sender, RoutedEventArgs e)
		{
            try
            {
                if (PartnerListView.SelectedItem is Partner selectPartner)
                {
                    var editWindow = new EditPartnerWindow(selectPartner, true);

                    if (editWindow.ShowDialog() == true)
                    {
                        UpdatePartnerDB(editWindow.partnerModel);
						partners.Clear();
                        LoadPartners();
                    }
                }
            }
            catch(Exception ex)
            {
				MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
			}

        }

        private void UpdatePartnerDB(Partner partner)
        {
            try
            {
                using var connection = new SqlConnection(DBContext.ConnectionString);
                connection.Open();

                string query = @"update Partner set Name=@Name, Raiting = @Raiting, Email = @Email,
PhoneNumber = @PhoneNumber, Address= @Address, Director = @Director, PatrnerType = @Type, INN = @INN Where Id = @Id";
                using var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Name", partner.Name);
                command.Parameters.AddWithValue("@Raiting", partner.Raitning);
                command.Parameters.AddWithValue("@Email", partner.Email);
                command.Parameters.AddWithValue("@PhoneNumber", partner.PhoneNumber);
                command.Parameters.AddWithValue("@Address", partner.Address);
                command.Parameters.AddWithValue("@Director", partner.Director);
                command.Parameters.AddWithValue("@Type", partner.PatrnerType);
                command.Parameters.AddWithValue("@Id", partner.Id);
                command.Parameters.AddWithValue("INN", partner.INN);
                command.ExecuteNonQuery();
            }
            catch (Exception ex) {
				MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
			}

        }
		private void AddPartnerDB(Partner partner)
		{
			try
			{
				using var connection = new SqlConnection(DBContext.ConnectionString);
				connection.Open();

				string query = @"INSERT INTO Partner (Name, Raiting, Email, PhoneNumber, Address, Director, PatrnerType, INN)
                         VALUES (@Name, @Raiting, @Email, @PhoneNumber, @Address, @Director, @Type, @INN)";

				using var command = new SqlCommand(query, connection);
				command.Parameters.AddWithValue("@Name", partner.Name);
				command.Parameters.AddWithValue("@Raiting", partner.Raitning);
				command.Parameters.AddWithValue("@Email", partner.Email);
				command.Parameters.AddWithValue("@PhoneNumber", partner.PhoneNumber);
				command.Parameters.AddWithValue("@Address", partner.Address);
				command.Parameters.AddWithValue("@Director", partner.Director);
				command.Parameters.AddWithValue("@Type", partner.PatrnerType);
				command.Parameters.AddWithValue("@INN", partner.INN);
				command.ExecuteNonQuery();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}
        //добавление парнёра
		private void AddPartnerButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
					var editWindow = new EditPartnerWindow(null, false);
					if (editWindow.ShowDialog() == true)
					{
						AddPartnerDB(editWindow.partnerModel);
						partners.Clear();
						LoadPartners();
					}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}
	}
}