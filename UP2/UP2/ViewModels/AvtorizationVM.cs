using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System.Windows;
using Avalonia.Controls;
using Microsoft.EntityFrameworkCore;
using ReactiveUI;
using UP2.Models;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia;
using Tmds.DBus.Protocol;
using System.Threading.Tasks;
using System.Text;

namespace UP2.ViewModels
{
	public class AvtorizationVM : ViewModelBase
    {

        private string _userName;
        private string _password;
        private List<User> _userList;

        private CurrentUser _currentUser;
        public CurrentUser CurrentUser
        {
            get => _currentUser;
            private set => this.RaiseAndSetIfChanged(ref _currentUser, value);
        }
        public string UserName
        {
            get => _userName;
            set => this.RaiseAndSetIfChanged(ref _userName, value);
        }
        public string Password
        {
            get => _password;
            set => this.RaiseAndSetIfChanged(ref _password, value);
        }
        public List<User> UserList { get => _userList; set => this.RaiseAndSetIfChanged(ref _userList, value); }
        public AvtorizationVM()
        {
            UserList = MainWindowViewModel.myConnection.Users.
                                                               Include(x => x.UserroleNavigation).
                                                               ToList();
            EnterButtonEnabled = true; // Блокируем кнопку авторизации
            EnterButtonCommand = ReactiveCommand.CreateFromTask(ExecuteLogin);

            ToShowCommand = ReactiveCommand.Create(ExecuteToShow);

        }
        private bool _enterButtonEnabled;
        public bool EnterButtonEnabled
        {
            get => _enterButtonEnabled;
            set => this.RaiseAndSetIfChanged(ref _enterButtonEnabled, value);
        }
        private bool _enterButtonVisible;
        public bool EnterButtonVisible
        {
            get => _enterButtonVisible;
            set => this.RaiseAndSetIfChanged(ref _enterButtonVisible, value);
        }
        private string _captchaText;
        public string captchaText
        {
            get => _captchaText;
            set => this.RaiseAndSetIfChanged(ref _captchaText, value);
        }

        private string _captcha;
        public string captcha
        {
            get => _captcha;
            set => this.RaiseAndSetIfChanged(ref _captcha, value);
        }
        public ICommand EnterButtonCommand { get; }


        public ICommand ToShowCommand { get; }

        private void ExecuteToShow()
        {
            MainWindowViewModel.Instance.PageContent = new ShowProduct();
        }
        public async Task ExecuteLogin()
        {
            EnterButtonEnabled = true; // Разблокируем кнопку
            EnterButtonVisible = false;
            var user = UserList.FirstOrDefault(u => u.Userlogin == UserName && u.Userpassword == Password);
            if (user != null && captchaText == captcha)
            {
                MainWindowViewModel.Instance.PageContent = new ShowProduct();
                CurrentUser = new CurrentUser
                {
                    FullName = $"{user.Usersurname} {user.Username} {user.Userpatronymic}",
                    Role = user.UserroleNavigation.Rolename
                };
            }
            else
            {
                await MessageBoxManager.GetMessageBoxStandard("Окно", "Авторизация не успешна", ButtonEnum.Ok).ShowAsync();
                EnterButtonEnabled = false; // Разблокируем кнопку
                EnterButtonVisible = true;
                GenerateCaptcha();
                await Task.Delay(10000); // Ждем 10 секунд
                EnterButtonEnabled = true; // Разблокируем кнопку
            }
        }
        private void GenerateCaptcha()
        {
            captcha = GenerateRandomString(6); // Генерируем строку длиной 6 символов
        } 


        private string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            StringBuilder result = new StringBuilder();
            Random random = new Random();
            for (int i = 0; i < length; i++)
            {
                result.Append(chars[random.Next(chars.Length)]);
            }
            return result.ToString();
        }
    }
}