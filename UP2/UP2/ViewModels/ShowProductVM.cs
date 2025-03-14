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
                string welcomeMessage = $"����� ����������, {currentUser.FullName}!";
                string userRole = $"���� ����: {currentUser.Role}";
                Console.WriteLine(welcomeMessage);
                Console.WriteLine(userRole);
            }
            else
            {
                Console.WriteLine("������������ �� �����������.");
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
            "��� ���������",
            "�� 0 �� 10",
            "�� 10 �� 15",
            "�� 15 �� 20",
            "20 � �����"
        };

        // �������� ��� ��������� ���� ���������� ������
        public List<string> DiscountRanges => _discountRangeLabels;

        // ������ ���������
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
            // ������������� ������ ��������� �� ���� ������
            LoadProducts();
            NewZakaz = new Orderproduct();
        }

        // ����� ��� �������� ��������� �� ���� ������
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

        // ����� �� ����� ��������
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

        // ����������
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

        // ����� ��������� ������
        private string _selectedDiscountRange = "��� ���������";
        public string SelectedDiscountRange
        {
            get => _selectedDiscountRange;
            set
            {
                _selectedDiscountRange = value;
                AllFilters();
            }
        }

        // ����� ��� ���������� ���� ��������
        private void AllFilters()
        {
            // ��������� ���� ��������� �� ���� ������
            LoadProducts();

            // ���������� �� �����
            if (!string.IsNullOrWhiteSpace(_search))
            {
                ProductList = ProductList.Where(x => x.Name.ToLower().Contains(_search.ToLower())).ToList();
            }

            // ����������
            ProductList = _sortDown
                ? ProductList.OrderByDescending(x => x.Cost).ToList()
                : ProductList.OrderBy(x => x.Cost).ToList();

            // ���������� �� ��������� ������
            switch (_selectedDiscountRange)
            {
                case "�� 0 �� 10":
                    ProductList = ProductList.Where(x => x.CurrentDiscount.HasValue && x.CurrentDiscount.Value < 10).ToList();
                    break;
                case "�� 10 �� 15":
                    ProductList = ProductList.Where(x => x.CurrentDiscount.HasValue && x.CurrentDiscount.Value >= 10 && x.CurrentDiscount.Value < 15).ToList();
                    break;
                case "�� 15 �� 20":
                    ProductList = ProductList.Where(x => x.CurrentDiscount.HasValue && x.CurrentDiscount.Value >= 15 && x.CurrentDiscount.Value < 20).ToList();
                    break;
                case "20 � �����":
                    ProductList = ProductList.Where(x => x.CurrentDiscount.HasValue && x.CurrentDiscount.Value >= 20).ToList();
                    break;
                case "��� ���������":
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
                return IsEditMode ? "��������� ���������" : "���������";
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
            ButtonResult result = await MessageBoxManager.GetMessageBoxStandard("����", "�� ������ ��������� ���������?", ButtonEnum.YesNo).ShowAsync();
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
                ButtonResult resultok = await MessageBoxManager.GetMessageBoxStandard("����", "��������� ������� ���������", ButtonEnum.Ok).ShowAsync();
            }
            else if (result == ButtonResult.No)
            {
                MainWindowViewModel.Instance.PageContent = new ShowProduct();
            }
        }

    }
}