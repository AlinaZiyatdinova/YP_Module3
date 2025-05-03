using ParnerDiscount.Models;
using System.Windows;
using System.Windows.Controls;

namespace ParnerDiscount
{
	public partial class EditPartnerWindow : Window
	{
		public Partner partnerModel  { get; set; } = new Partner();
		public bool isEditMode { get; set; }	
		public EditPartnerWindow(Partner ParnerModel, bool isEdit)
		{

			InitializeComponent();

			try
			{
				//проверка на тип окна 
				if (ParnerModel != null && isEdit == true)
				{
					this.partnerModel = ParnerModel;
					InitFields(ParnerModel);
					isEditMode = true;
				}

				if (ParnerModel == null && !isEdit == false) 
				{
					this.partnerModel = ParnerModel;
					isEditMode = false;
				}
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
			}

		}

		public EditPartnerWindow()
		{
			InitializeComponent();
		}

		public void InitFields(Partner partner)
		{
			NameBox.Text = partner.Name;
			RaitingBox.Text = partner.Raitning;
			EmailBox.Text = partner.Email;
			PhoneNumberBox.Text = partner.PhoneNumber;
			AddressBox.Text = partner.Address;
			DirectorBox.Text = partner.Director;
			INNBox.Text = partner.INN;

			foreach (ComboBoxItem item in PartnerTypeBox.Items)
			{
				if (item.Content.ToString() == partner.PatrnerType)
				{
					PartnerTypeBox.SelectedItem = item;
					break;
				}
			}
		}

		private void SaveButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				var message = MessageBox.Show("Вы уверены, что хотите сохранить данные?",
								  "Предупреждение",
								  MessageBoxButton.YesNo,
								  MessageBoxImage.Question);

				if (message == MessageBoxResult.Yes)
				{
					if (isEditMode)
					{
						if (checkValidation())
						{
							partnerModel.Name = NameBox.Text;
							partnerModel.Raitning = RaitingBox.Text;
							partnerModel.Email = EmailBox.Text;
							partnerModel.PhoneNumber = PhoneNumberBox.Text;
							partnerModel.Address = AddressBox.Text;
							partnerModel.INN = INNBox.Text;
							partnerModel.Director = DirectorBox.Text;
							partnerModel.PatrnerType = ((ComboBoxItem)PartnerTypeBox.SelectedItem)?.Content.ToString();
							DialogResult = true;
							Close();
						}
					}
					else
					{
						if (checkValidation())
						{
							Partner partner = new Partner();
							partner.Name = NameBox.Text;
							partner.Raitning = RaitingBox.Text;
							partner.Email = EmailBox.Text;
							partner.PhoneNumber = PhoneNumberBox.Text;
							partner.Address = AddressBox.Text;
							partner.INN = INNBox.Text;
							partner.Director = DirectorBox.Text;
							partner.PatrnerType = ((ComboBoxItem)PartnerTypeBox.SelectedItem)?.Content.ToString();
							partnerModel = partner;
							DialogResult = true;
							Close();
						}
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private bool checkValidation()
		{
			if (string.IsNullOrWhiteSpace(NameBox.Text))
			{
				throw new InvalidOperationException("Поля должны быть заполнены. Введите наименование!");
			}
			if(NameBox.Text.Length <= 1 || NameBox.Text.Length >50)
			{
				throw new InvalidOperationException("Длина наименования должна быть в диапазоне от 2-50 символов");
			}
			if (string.IsNullOrWhiteSpace(RaitingBox.Text))
			{
				throw new InvalidOperationException("Поля должны быть заполнены. Введите рейтинг!");
			}
			if(int.Parse(RaitingBox.Text) > 20 || int.Parse(RaitingBox.Text) < 0)
			{
				throw new InvalidOperationException("Рейтинг должен быть неотрицательным числов в диапазоне от 0 до 20");
			}
			if (string.IsNullOrWhiteSpace(EmailBox.Text))
			{
				throw new InvalidOperationException("Поля должны быть заполнены. Введите почту!");
			}
			if (string.IsNullOrWhiteSpace(INNBox.Text))
			{
				throw new InvalidOperationException("Поля должны быть заполнены. Введмте ИНН!");
			}
			if (!EmailBox.Text.Contains("@"))
			{
				throw new InvalidOperationException("Почта должна содержать '@'");
			}
			if (string.IsNullOrWhiteSpace(AddressBox.Text))
			{
				throw new InvalidOperationException("Поля должны быть заполнены. Введите адрес!");
			}
			if (string.IsNullOrWhiteSpace(DirectorBox.Text))
			{
				throw new InvalidOperationException("Поля должны быть заполнены. Введите ФИО директора!");
			}
			if (string.IsNullOrWhiteSpace(PhoneNumberBox.Text))
			{
				throw new InvalidOperationException("Поля должны быть заполнены. Введите номер телефона!");
			}
			if (PartnerTypeBox.SelectedItem == null)
			{
				throw new InvalidOperationException("Выберите тип партнёра");
			}
			return true;
		}

		private void CancelButton_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = false;
			Close();
		}
	}
}
