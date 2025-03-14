using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ReactiveUI;
using UP2.Models;
using Avalonia;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia;
using System.Reflection.Metadata;

namespace UP2.ViewModels
{
	public class ShowProductVM : ViewModelBase
    {
        private AvtorizationVM _avtorizationVM;

        public ShowProductVM(AvtorizationVM avtorizationVM)
        {
            _avtorizationVM = avtorizationVM;
        }
        public void ToLast()
        {
            MainWindowViewModel.Instance.PageContent = new Avtorization();
        }
        public void ToOrder()
        {
            MainWindowViewModel.Instance.PageContent = new ShowOrder();
        }
        public CurrentUser CurrentUser => _avtorizationVM.CurrentUser;
        public void SomeMethod()
        {
            var currentUser = _avtorizationVM.CurrentUser;
            if (currentUser != null)
            {
                string welcomeMessage = $"Добро пожаловать, {currentUser.FullName}!";
                string userRole = $"Ваша роль: {currentUser.Role}";
                Console.WriteLine(welcomeMessage);
                Console.WriteLine(userRole);
            }
            else
            {
                Console.WriteLine("Пользователь не авторизован.");
            }
        }

        private int _totalRecords;
        public int TotalRecords
        {
            get => _totalRecords;
            set => this.RaiseAndSetIfChanged(ref _totalRecords, value);
        }

        private int _sortRecords;
        public int SortRecords
        {
            get => _sortRecords;
            set => this.RaiseAndSetIfChanged(ref _sortRecords, value);
        }
        private readonly List<string> _discountRangeLabels = new List<string>
        {
            "Все диапазоны",
            "от 0 до 10",
            "от 10 до 15",
            "от 15 до 20",
            "20 и более"
        };

        // Свойство для получения всех диапазонов скидок
        public List<string> DiscountRanges => _discountRangeLabels;

        // Список продуктов
        private List<Product> _productList;
        public List<Product> ProductList
        {
            get => _productList;
            set => this.RaiseAndSetIfChanged(ref _productList, value);
        }
        Orderproduct? _newZakaz;
        public Orderproduct? NewZakaz { get => _newZakaz; set => this.RaiseAndSetIfChanged(ref _newZakaz, value); }
        public ShowProductVM()
        {
            // Инициализация списка продуктов из базы данных
            LoadProducts();
            NewZakaz = new Orderproduct();
        }

        // Метод для загрузки продуктов из базы данных
        private void LoadProducts()
        {
            ProductList = MainWindowViewModel.myConnection.Products
                .Include(x => x.KategoryNavigation)
                .Include(x => x.ProductionNavigation)
                .Include(x => x.ProviderNavigation)
                .Include(x => x.Orderproducts).ThenInclude(x => x.Order)
                .OrderBy(x => x.Cost)
                .ToList();
            TotalRecords = ProductList.Count;
            SortRecords = ProductList.Count;
            TotalRecords = MainWindowViewModel.myConnection.Products.Count();
        }

        // Поиск по имени продукта
        private string _search;
        public string Search
        {
            get => _search;
            set
            {
                _search = value;
                AllFilters();
            }
        }

        // Сортировка
        private bool _sortUp = true;
        public bool SortUp
        {
            get => _sortUp;
            set
            {
                this.RaiseAndSetIfChanged(ref _sortUp, value);
                AllFilters();
            }
        }

        private bool _sortDown = false;
        public bool SortDown
        {
            get => _sortDown;
            set
            {
                this.RaiseAndSetIfChanged(ref _sortDown, value);
                AllFilters();
            }
        }

        // Выбор диапазона скидок
        private string _selectedDiscountRange = "Все диапазоны";
        public string SelectedDiscountRange
        {
            get => _selectedDiscountRange;
            set
            {
                _selectedDiscountRange = value;
                AllFilters();
            }
        }

        // Метод для применения всех фильтров
        private void AllFilters()
        {
            // Получение всех продуктов из базы данных
            LoadProducts();

            // Фильтрация по имени
            if (!string.IsNullOrWhiteSpace(_search))
            {
                ProductList = ProductList.Where(x => x.Name.ToLower().Contains(_search.ToLower())).ToList();
            }

            // Сортировка
            ProductList = _sortDown
                ? ProductList.OrderByDescending(x => x.Cost).ToList()
                : ProductList.OrderBy(x => x.Cost).ToList();

            // Фильтрация по диапазону скидок
            switch (_selectedDiscountRange)
            {
                case "от 0 до 10":
                    ProductList = ProductList.Where(x => x.CurrentDiscount.HasValue && x.CurrentDiscount.Value < 10).ToList();
                    break;
                case "от 10 до 15":
                    ProductList = ProductList.Where(x => x.CurrentDiscount.HasValue && x.CurrentDiscount.Value >= 10 && x.CurrentDiscount.Value < 15).ToList();
                    break;
                case "от 15 до 20":
                    ProductList = ProductList.Where(x => x.CurrentDiscount.HasValue && x.CurrentDiscount.Value >= 15 && x.CurrentDiscount.Value < 20).ToList();
                    break;
                case "20 и более":
                    ProductList = ProductList.Where(x => x.CurrentDiscount.HasValue && x.CurrentDiscount.Value >= 20).ToList();
                    break;
                case "Все диапазоны":
                default:
                    break;
            }
            SortRecords = ProductList.Count;
        }


        private bool _isEditMode;
        public bool IsEditMode
        {
            get => _isEditMode;
            set => this.RaiseAndSetIfChanged(ref _isEditMode, value);
        }
        public string ButtonText
        {
            get
            {
                return IsEditMode ? "Сохранить изменения" : "Сохранить";
            }
        }
        private Product _selectedProduct;

        
        private string _quantitys;
        public string Quantitys
        {
            get => _quantitys;
            set
            {
                if (_quantitys != value)
                {
                    _quantitys = value;
                    this.RaiseAndSetIfChanged(ref _quantitys, value);
                }
            }
        }
        public async void AddZakaz(object param)
        {
            var selectedProduct = param as Product;
            int a;
            ButtonResult result = await MessageBoxManager.GetMessageBoxStandard("Окно", "Вы хотите сохранить изменения?", ButtonEnum.YesNo).ShowAsync();
            if (result == ButtonResult.Yes)
            {
                if (NewZakaz != null && NewZakaz.Id == 0) 
                {
                    Orderproduct newOrderProduct = new Orderproduct
                    {
                        Orderid = selectedProduct.Id,
                        Productarticlenumber = selectedProduct.ArticleNumber,
                    };
                    MainWindowViewModel.myConnection.Orderproducts.Add(newOrderProduct);
                }
                await MainWindowViewModel.myConnection.SaveChangesAsync();
                MainWindowViewModel.Instance.PageContent = new ShowProduct();
                ButtonResult resultok = await MessageBoxManager.GetMessageBoxStandard("Окно", "Изменения успешно сохранены", ButtonEnum.Ok).ShowAsync();
            }
            else if (result == ButtonResult.No)
            {
                MainWindowViewModel.Instance.PageContent = new ShowProduct();
            }
        }

    }
}