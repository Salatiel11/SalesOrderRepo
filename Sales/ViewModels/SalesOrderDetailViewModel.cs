using Core.Models;
using Infrastructure.Services;
using Infrastructure.ViewModels;
using Prism.Commands;
using Prism.Regions;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

public class SalesOrderDetailViewModel : AbstractDetailViewModel<SalesOrder, ISalesOrderService>
{
    private readonly ICustomerService _customerService;
    private readonly IProductService _productService;
    private readonly ISalesOrderService _salesOrderService;

    private ObservableCollection<SalesOrderLine> _temporaryOrderLines;
    private decimal _totalAmount;
    private bool _isOperationInProgress;

    public ObservableCollection<SalesOrderLine> TemporaryOrderLines
    {
        get => _temporaryOrderLines;
        set
        {
            if (_temporaryOrderLines != value)
            {
                if (_temporaryOrderLines != null)
                {
                    _temporaryOrderLines.CollectionChanged -= OnTemporaryOrderLinesChanged;
                }

                _temporaryOrderLines = value;

                if (_temporaryOrderLines != null)
                {
                    _temporaryOrderLines.CollectionChanged += OnTemporaryOrderLinesChanged;
                }

                RaisePropertyChanged(nameof(TemporaryOrderLines));
                RecalculateTotalAmount();
            }
        }
    }

    public ObservableCollection<Customer> Customers { get; } = new();
    public ObservableCollection<Product> Products { get; } = new();

    private bool _isCustomerSelectionEnabled = true;
    public bool IsCustomerSelectionEnabled
    {
        get => _isCustomerSelectionEnabled;
        set => SetProperty(ref _isCustomerSelectionEnabled, value);
    }

    public Customer SelectedCustomer
    {
        get => Customers.FirstOrDefault(c => c.Id == Item.CustomerId);
        set
        {
            if (value != null)
            {
                Item.CustomerId = value.Id;
                IsCustomerSelectionEnabled = false;
                RaisePropertyChanged(nameof(SelectedCustomer));
            }
        }
    }

    public Product SelectedProduct { get; set; }
    public int ProductQuantity { get; set; } = 1;

    public decimal TotalAmount
    {
        get => _totalAmount;
        set
        {
            _totalAmount = value;
            RaisePropertyChanged(nameof(TotalAmount));
        }
    }

    public bool IsOperationInProgress
    {
        get => !_isOperationInProgress;
        set
        {
            _isOperationInProgress = value;
            RaisePropertyChanged(nameof(IsOperationInProgress));
        }
    }

    public DelegateCommand AddLineCommand { get; }

    public SalesOrderDetailViewModel(
        IRegionManager regionManager,
        ISalesOrderService orderService,
        ICustomerService customerService,
        IProductService productService,
        IBaseDialogViewModel baseDialogViewModel)
        : base(regionManager, orderService, baseDialogViewModel)
    {
        _customerService = customerService;
        _productService = productService;
        _salesOrderService = orderService;

        AddLineCommand = new DelegateCommand(OnAddLine);

        TemporaryOrderLines = new ObservableCollection<SalesOrderLine>();
    }

    private async void LoadCustomersAndProducts()
    {
        Customers.Clear();
        Products.Clear();
        var customers = await _customerService.GetAllAsync();
        Customers.AddRange(customers);

        var products = await _productService.GetAllAsync();
        Products.AddRange(products);
    }

    private void OnAddLine()
    {
        if (SelectedProduct == null || ProductQuantity <= 0) return;

        var newLine = new SalesOrderLine
        {
            ProductId = SelectedProduct.Id,
            Product = SelectedProduct,
            Quantity = ProductQuantity,
            UnitPrice = SelectedProduct.Price
        };

        TemporaryOrderLines.Add(newLine);
    }

    private void OnTemporaryOrderLinesChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        RecalculateTotalAmount();
    }

    private void RecalculateTotalAmount()
    {
        TotalAmount = TemporaryOrderLines.Sum(line => line.Quantity * line.UnitPrice);
    }

    protected override async Task<SalesOrder> GetItemAsync(int id)
    {
        var salesOrder = await _salesOrderService.GetWithItemsAsync(id);

        TemporaryOrderLines.Clear();
        foreach (var line in salesOrder.OrderLines)
        {
            TemporaryOrderLines.Add(line);
        }
        SelectedCustomer = Customers.FirstOrDefault(c => c.Id == salesOrder.CustomerId);

        return salesOrder;
    }

    protected override async Task AddItemAsync(SalesOrder item)
    {
        item.TotalAmount = TotalAmount;
        var salesOrderId = await _salesOrderService.AddAsync(item);
        foreach (var line in TemporaryOrderLines)
        {
            line.OrderId = salesOrderId;
        }
        await _salesOrderService.AddOrderLinesAsync(TemporaryOrderLines);
    }

    protected override async Task UpdateItemAsync(SalesOrder item)
    {
        await _salesOrderService.UpdateAsync(item);
        var salesOrderId = item.Id == 0 ? await _salesOrderService.AddAsync(item) : item.Id;

        foreach (var line in TemporaryOrderLines)
        {
            line.OrderId = salesOrderId;
        }
        if (item.Id == 0)
        {
            await _salesOrderService.AddOrderLinesAsync(item.OrderLines);
        }
        await _salesOrderService.UpdateOrderLinesAsync(TemporaryOrderLines);
    }

    protected override async Task DeleteItemAsync(SalesOrder item)
    {
        IsOperationInProgress = true;
        try
        {
            await _salesOrderService.DeleteAsync(item.Id);
        }
        finally
        {
            IsOperationInProgress = false;
        }
    }

    protected override bool ValidateItem()
    {
        return Item.CustomerId > 0 && TemporaryOrderLines.Any();
    }

    public override void OnNavigatedTo(NavigationContext navigationContext)
    {
        base.OnNavigatedTo(navigationContext);

        LoadCustomersAndProducts();

        if (_mode == "add")
        {
            TemporaryOrderLines.Clear();
            IsCustomerSelectionEnabled = true;
            Item = new SalesOrder();
        }
    }
}