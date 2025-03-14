  using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia;
using ReactiveUI;
using UP2.Models;
using System.Windows.Input;
using MsBox.Avalonia.ViewModels.Commands;

namespace UP2.ViewModels
{
	public class ShowOrderVM : ViewModelBase
    {
        public ICommand SortByAscendingCommand { get; }
        public ICommand SortByDescendingCommand { get; }

        private List<Orderproduct> _filteredOrderList; 
        public List<Orderproduct> FilteredOrderList
        {
            get => _filteredOrderList;
            private set => this.RaiseAndSetIfChanged(ref _filteredOrderList, value);
        }

        private string _selectedFilter;
        public string SelectedFilter
        {
            get => _selectedFilter;
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedFilter, value);
                FilterOrders();
            }
        }

        public List<string> Filters { get; } = new List<string>
        {
            "Все",
            "0-10%",
            "11-14%",
            "15% и более"
        };

        List<Orderproduct> _orderList;
        public List<Orderproduct> OrderList { get => _orderList; set => this.RaiseAndSetIfChanged(ref _orderList, value); }
       
        public ICommand Filter0To10Command { get; }
        public ICommand Filter11To14Command { get; }
        public ICommand Filter15AndMoreCommand { get; }
        public ICommand ResetFilterCommand { get; }

        public ShowOrderVM()
        {
            OrderList = MainWindowViewModel.myConnection.Orderproducts.
                                                               Include(x => x.ProductarticlenumberNavigation).
                                                               Include(x => x.Order).
                                                               ToList();
            FilteredOrderList = new List<Orderproduct>(OrderList); // Изначально показываем все заказы
            SelectedFilter = Filters[0];
            SortByAscendingCommand = new RelayCommand(SortByAscending);
            SortByDescendingCommand = new RelayCommand(SortByDescending);
        }


        private void SortByAscending(object parameter)
        {
            FilteredOrderList = FilteredOrderList.OrderBy(orderProduct =>
                orderProduct.ProductarticlenumberNavigation.Cost ?? 0).ToList();
        }

        private void SortByDescending(object parameter)
        {
            FilteredOrderList = FilteredOrderList.OrderByDescending(orderProduct =>
                orderProduct.ProductarticlenumberNavigation.Cost ?? 0).ToList();
        }
        public decimal TotalOrderCost => CalculateTotalOrderCost();
        private decimal CalculateTotalOrderCost()
        {
            decimal total = 0;

            foreach (var orderProduct in OrderList)
            {
                var product = orderProduct.ProductarticlenumberNavigation;
                if (product != null && product.Cost.HasValue)
                {
                    total += orderProduct.Quantity * product.Cost.Value;
                }
            }

            return total;
        }
        public void ToLast()
        {
            MainWindowViewModel.Instance.PageContent = new ShowProduct();
        }
        public async void Delete(int id)
        {
            ButtonResult otvet = await MessageBoxManager.GetMessageBoxStandard("Сonfirmation", "Вы уверены, что хотите удалить?", ButtonEnum.YesNo).ShowAsync();
            Orderproduct delete = MainWindowViewModel.myConnection.Orderproducts
                .FirstOrDefault(x => x.Id == id);
            if (otvet == ButtonResult.Yes)
            {
                if (delete != null)
                {
                   MainWindowViewModel.myConnection.Orderproducts.Remove(delete);
                   MainWindowViewModel.myConnection.SaveChanges();
                   MainWindowViewModel.Instance.PageContent = new ShowOrder();
                }
            }
        }
        private void FilterOrders()
        {
            switch (SelectedFilter)
            {
                case "0-10%":
                    FilteredOrderList = OrderList
                        .Where(orderProduct =>
                            orderProduct.ProductarticlenumberNavigation.CurrentDiscount.HasValue &&
                            orderProduct.ProductarticlenumberNavigation.CurrentDiscount.Value >= 0 &&
                            orderProduct.ProductarticlenumberNavigation.CurrentDiscount.Value <= 10)
                        .ToList();
                    break;
                case "11-14%":
                    FilteredOrderList = OrderList
                        .Where(orderProduct =>
                            orderProduct.ProductarticlenumberNavigation.CurrentDiscount.HasValue &&
                            orderProduct.ProductarticlenumberNavigation.CurrentDiscount.Value >= 11 &&
                            orderProduct.ProductarticlenumberNavigation.CurrentDiscount.Value <= 14)
                        .ToList();
                    break;
                case "15% и более":
                    FilteredOrderList = OrderList
                        .Where(orderProduct =>
                            orderProduct.ProductarticlenumberNavigation.CurrentDiscount.HasValue &&
                            orderProduct.ProductarticlenumberNavigation.CurrentDiscount.Value >= 15)
                        .ToList();
                    break;
                default: // "Все"
                    FilteredOrderList = new List<Orderproduct>(OrderList);
                    break;
            }
        }
    }
}
